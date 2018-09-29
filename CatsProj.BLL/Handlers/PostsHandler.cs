using System;
using System.Collections.Generic;
using Cats.DataEntiry;
using CatsDataEntity;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using CatsProj.DataEntiry;
using EntityModelConverter;
using CatsProj.Tools;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;

namespace CatsProj.BLL.Handlers
{
    public class PostsHandler
    {
        public bool savePosts(string postsMaker,string postsContent,int picsCount,string postsId,double latitude,double longitude,string location,string postsType,int ifOfficial,int ifLY)
		{
			PostsModel model = new PostsModel();
			model.postsMaker = postsMaker;
			model.postsContent = postsContent;
			model.postsID = postsId;
			model.postsMakeDate = DateTime.Now;
			model.postsPicCount = picsCount;
            model.latitude = latitude;
            model.longitude = longitude;
            model.postsLocation = location;
            model.postsType = postsType;
            model.ifOfficial = ifOfficial;
            model.ifLY = ifLY;
			tbl_posts posts =  PostsConverter.postsModelToEntity(model);
			PostsProvider provider = new PostsProvider();
            provider.savePosts(posts);
            return true;
		}
        public string generateCMD(string msg)
        {
            
            string token = new TokenProvider().getToken();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/wxa/msg_sec_check?access_token=" + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"content\":\""+msg+"\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        public string getQRCode(string openId,string postsId)
        {

            string token = new TokenProvider().getToken();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            long shareId = new PostsProvider().saveShareCode(postsId, openId);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"scene\":\"id=" + shareId + "\",\"page\":\"\"}";
                var obj = new { scene = "id=" + shareId, page = "", is_hyaline = true };
                json = JsonConvert.SerializeObject(obj);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    
            //    return result;
            //}
            Image qrcode = Image.FromStream(httpResponse.GetResponseStream());
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/qrCode")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/qrCode"));
            }
            qrcode.Save(HttpContext.Current.Server.MapPath("~/qrCode/" + shareId.ToString() + ".jpeg"));
            return "/qrCode/" + shareId.ToString() + ".jpeg";
        }

        public bool ifValidContent(string postsContent)
        {
            string result = generateCMD(postsContent);
            ReturnValue value = JsonConvert.DeserializeObject<ReturnValue>(result);
            if (value.errcode == 87014) { return false; }
            return true;
        }

        private class ReturnValue
        {
            public long errcode { get; set; }
            public string errMsg { get; set; }
        }

        private class JsonContent
        {
            public string content { get; set; }
        }

        public List<PostsModel> getPosts(string openId,int from,int count,DateTime refreshTime,int currSel, double lati, double longti)
		{
			PostsProvider provider = new PostsProvider();
			IList<PostsPics> pics = new List<PostsPics>();
			pics = provider.getPosts(openId,from, count, 1,refreshTime,currSel,lati,longti);
			List<PostsModel> result = new List<PostsModel>();

            foreach(var item in pics)
			{
                if (item.postsContent.Length == 20)
                {
                    item.postsContent = item.postsContent + "...";
                }
                result.Add(PostsConverter.postsEntityToModel(item));
			}
			return result;
		}

        public bool ifLegalPosts(string postsId)
        {
            PostsProvider provider = new PostsProvider();
            bool ifCanRead = provider.ifCanRead(postsId);
            bool ifLegal = true;
            if (ifCanRead)
            {
                long viewedTimes = provider.getViewedTimes(postsId);
                long reportTimes = provider.getReportTimes(postsId);

                decimal rate=0;
                if (viewedTimes > 0)
                {
                    rate = Convert.ToDecimal(reportTimes) / Convert.ToDecimal(viewedTimes );
                }
                if (viewedTimes <= 100)
                {
                    if(reportTimes>=5)
                    {
                        provider.updatePostsStatus(postsId);
                        ifLegal = false;
                    }
                }
                else if(viewedTimes>=100&&viewedTimes<=1000)
                {
                    if(rate>=Convert.ToDecimal(0.03)||reportTimes>=15)
                    {
                        provider.updatePostsStatus(postsId);
                        ifLegal = false;
                    }
                }
                else
                {
                    if(rate>=Convert.ToDecimal(0.15)||reportTimes>=30)
                    {
                        provider.updatePostsStatus(postsId);
                        ifLegal = false;
                    }
                }
            }
            else
            {
                return false;
            }
            return ifLegal;
        }
		public PostsModel getPostsDetail(string postsId)
		{
			PostsProvider provider = new PostsProvider();
			IList<PostsPics> posts=provider.getPostsDetail(postsId);
			List<PostsModel> result = new List<PostsModel>();
			foreach (var item in posts)
            {
                
                result.Add(PostsConverter.postsEntityToModel(item));
            }
			PostsModel model = new PostsModel();
			string picList = string.Empty;
            foreach ( var item in result)
			{
				picList = picList + item.postsPics + ";";
			}
			result[0].postsPics = picList.Substring(0,picList.Length-1);
            
			return result[0];
		}

        public bool ifUserLoved(string postsId,string userId)
		{
			PostsProvider provider = new PostsProvider();
			UserLovedModel model = new UserLovedModel();
			model.postsId = postsId;
			model.userId = userId;
			model.lovedDate = DateTime.Now;
			bool result = provider.ifUserLoved(postsId,userId);
			return result;
		}

        public void userLoved(string postsId,string userId)
		{
			PostsProvider provider = new PostsProvider();
			UserLovedModel model = new UserLovedModel();
			model.lovedId = Guid.NewGuid().ToString();
			model.userId = userId;
			model.postsId = postsId;
			model.lovedDate = DateTime.Now;
            model.loveStatus = 1;
			provider.lovePosts(UserLovedConverter.userlovedModelToEntity(model));         
		}

        public void userReply(string replyContent,string postsId,string userId)
		{
			UserReplyModel model = new UserReplyModel();
			model.postsId = postsId;
			model.replyContent = replyContent;
			model.userId = userId;
			model.replyDate = DateTime.Now;
			model.replyId = Guid.NewGuid().ToString();
			model.replyStatus = 0;
			PostsProvider provider = new PostsProvider();
			provider.userReply(UserReplyConverter.replyModelToEntity(model));
		}

        public string postsLoved(string postsId)
		{
			PostsProvider provider = new PostsProvider();
			long lovedTimes = provider.getUserLoved(postsId);
			return formatnumber(lovedTimes);  
		}

        public List<RepliesModel> getMyReplies(string postsId, int from, int count, DateTime refreshTime, string openId)
        {
            try
            {
                PostsProvider provider = new PostsProvider();
                List<UserReply> result = provider.getMyReplies(from, count, postsId, refreshTime, openId);
                List<RepliesModel> replies = new List<RepliesModel>();

                foreach (var item in result)
                {

                    replies.Add(RepliesConverter.repliesEntityToModel(item));
                    //replies[replies.Count - 1].afterReplyList = RepliesConverter.repliesEntityToModel(provider.getAfterReplyList(postsId));
                    //provider.getAfterReplyList(item.replyId).ForEach(o => replies[replies.Count - 1].afterReplyList.Add(RepliesConverter.repliesEntityToModel(o)));
                    var tempResult = provider.getAfterReplyList(item.replyId,from, count,refreshTime);
                    foreach(var itemT in tempResult)
                    {
                        replies[replies.Count - 1].afterReplyList.Add(RepliesConverter.repliesEntityToModel(itemT));
                    }
                    replies[replies.Count - 1].afterReplyCount = replies[replies.Count - 1].afterReplyList==null?0:replies[replies.Count - 1].afterReplyList.Count;
                    if (replies[replies.Count - 1].afterReplyCount > 2)
                    {
                        replies[replies.Count - 1].afterReplyList.RemoveRange(2, Convert.ToInt32(replies[replies.Count - 1].afterReplyCount) - 2);
                    }

                }
                return replies;
            }
            catch (Exception e)
            {
                return new List<RepliesModel>();
            }
        }

        public RepliesModel getReplyDetail(string replyId,int from,int count,DateTime refreshTime)
        {
            PostsProvider provider = new PostsProvider();
            UserReply reply = provider.getReplyDetail(replyId);
            RepliesModel result = RepliesConverter.repliesEntityToModel(reply);
            var tempResult = provider.getAfterReplyList(replyId, from, count,refreshTime);
            tempResult.RemoveRange(0, from);
            if (tempResult.Count > count)
            {
                tempResult.RemoveRange(count, tempResult.Count-count);
            }
            foreach (var itemT in tempResult)
            {
                result.afterReplyList.Add(RepliesConverter.repliesEntityToModel(itemT));
            }
            return result;

        }

        public List<RepliesModel> getMoreAfterReply(string replyId,int from,int count,DateTime refreshTime)
        {
            PostsProvider provider = new PostsProvider();
            var tempResult = provider.getAfterReplyList(replyId, from, count,refreshTime);
            tempResult.RemoveRange(0, from);
            if (tempResult.Count > count)
            {
                tempResult.RemoveRange(count, tempResult.Count - count);
            }
            List<RepliesModel> result = new List<RepliesModel>();
            foreach (var itemT in tempResult)
            {
                result.Add(RepliesConverter.repliesEntityToModel(itemT));
            }

            return result;
        }

        public List<RepliesModel> getReplies(string postsId,int from,int count, DateTime refreshTime,string openId)
		{
			try
			{
				PostsProvider provider = new PostsProvider();
				List<UserReply> result = provider.getUserReplies(from, count, postsId, refreshTime, openId);
				List<RepliesModel> replies = new List<RepliesModel>();

				foreach (var item in result)
				{
                    
					replies.Add(RepliesConverter.repliesEntityToModel(item));
                    var tempResult = provider.getAfterReplyList(item.replyId,from, count,refreshTime);
                    foreach (var itemT in tempResult)
                    {
                        replies[replies.Count - 1].afterReplyList.Add(RepliesConverter.repliesEntityToModel(itemT));
                    }
                    replies[replies.Count - 1].afterReplyCount = replies[replies.Count - 1].afterReplyList == null ? 0 : replies[replies.Count - 1].afterReplyList.Count;
                    if (replies[replies.Count - 1].afterReplyCount > 2)
                    {
                        replies[replies.Count - 1].afterReplyList.RemoveRange(2, Convert.ToInt32(replies[replies.Count - 1].afterReplyCount) - 2);
                    }

                }
				return replies;
			}
			catch(Exception e)
			{
				return new List<RepliesModel>();
			}
		}

        public List<PostsModel> getPostsByMaker (string openid,int from, int count)
		{
			PostsProvider provider = new PostsProvider();
			List<PostsPics> entities = new List<PostsPics>();
			entities=provider.getPostsByMaker(openid, from, count);
			List<PostsModel> models = new List<PostsModel>();
            foreach(var item in entities)
			{
				models.Add(PostsConverter.postsEntityToModel(item));            
			}
			return models;
		}

        public void viewPosts(string openId,string postsId)
		{
			PostsProvider provider = new PostsProvider();
			provider.userViewPosts(openId, postsId);
		}

        public string getRepliesCount(string postsId)
        {
            long repliesCount = new PostsProvider().getRepliesCount(postsId);
            return formatnumber(repliesCount);
        }

        public string getReadCount(string postsId)
		{
			return formatnumber(new PostsProvider().getReadCount(postsId));
		}

        public string formatnumber(long count)
		{
			string result = string.Empty;
			if(count>=1000000)
			{
				result = Math.Round(Convert.ToDecimal(count) / 1000000, 2) + "百万";
			}
			else if(count>=10000)
			{
				result = Math.Round(Convert.ToDecimal(count) / 10000, 2) + "万";
			}
			else
			{
				result = Convert.ToString(count);
			}
			return result;
		}

		public void userLoveReply(string openId,string replyId)
		{
			new PostsProvider().userLoveReply(openId, replyId);
		}

        public void delReply(string openId,string replyId,int delType)
		{
			new PostsProvider().delReply(replyId, openId, delType);
		}

        public List<List<ReplyNLoveModel>> getReplyNLoveCount(string openId)
		{
			List<List<ReplyNLoveCount>> counts = new PostsProvider().getReplyNLoveCount(openId);
			List<ReplyNLoveModel> replies = new List<ReplyNLoveModel>();
			List<ReplyNLoveModel> loved = new List<ReplyNLoveModel>();
            foreach(var item in counts[0])
			{
				replies.Add(ReplyNLoveConverter.entityToModel(item));
			}
            foreach(var item in counts[1])
			{
				loved.Add(ReplyNLoveConverter.entityToModel(item));
			}
			List<List<ReplyNLoveModel>> result = new List<List<ReplyNLoveModel>>();
			result.Add(replies);
			result.Add(loved);
			return result;

		}

        public List<List<ReplyNLoveModel>> getAfterReplyNLoveCount(string openId)
        {
            List<List<ReplyNLoveCount>> counts = new PostsProvider().getAfterReplyNLoveCount(openId);
            List<ReplyNLoveModel> replies = new List<ReplyNLoveModel>();
            List<ReplyNLoveModel> loved = new List<ReplyNLoveModel>();
            foreach (var item in counts[0])
            {
                replies.Add(ReplyNLoveConverter.entityToModel(item));
            }
            foreach (var item in counts[1])
            {
                loved.Add(ReplyNLoveConverter.entityToModel(item));
            }
            List<List<ReplyNLoveModel>> result = new List<List<ReplyNLoveModel>>();
            result.Add(replies);
            result.Add(loved);
            return result;

        }

        public void reportPosts(string postsId,string openId)
        {
            tbl_report report = new tbl_report();
            report.postsId = postsId;
            report.reportId = Guid.NewGuid().ToString();
            report.reportReason = "0";
            report.reportTime = DateTime.Now;
            report.userId = openId;
            PostsProvider provider = new PostsProvider();
            provider.newReport(report);
        }

        public long getWaitingPosts()
        {
            return new PostsProvider().getWaitingList();
        }

        public void verifyPosts(string postsId)
        {
            new PostsProvider().verifyPosts(postsId);
        }

        public bool ifCanReport(string postsId)
        {
            return new PostsProvider().ifCanReport(postsId);
        }

        public string addReplyAfterReply(string replyId,string replyContent,string openId,string replyToUser)
        {
            return new PostsProvider().addReplyAfterReply(replyId, openId, replyContent,replyToUser);
        }

        public void delReplyAfterReply(string afterReplyId,string openId,int delType)
        {
            new PostsProvider().delReplyAfterReply(afterReplyId, openId, delType);
        }

        public bool ifUserLovedReply(string openId,string replyId)
        {
            return new PostsProvider().ifUserLovedReply(openId, replyId);
        }

        public void userSharePosts(string openId,string postsId)
        {
            new PostsProvider().sharePosts(openId, postsId);
        }

        public string getPostsIdFromQRCode(string shareId)
        {
            PostsProvider provider = new PostsProvider();
            tbl_sharecode sharecode = provider.getShareCodeRecord(shareId);
            return sharecode.postsId;

        }

        public List<EventModel> getEventsList()
        {
            List<tbl_event> eventList = new PostsProvider().getEventList();
            List<EventModel> modelList = new List<EventModel>();
            foreach (var item in eventList)
            {
                modelList.Add(EventConverter.entityToModel(item));
            }
            return modelList;
        }

        public string getShareCount(string postsId)
        {
            long result = new PostsProvider().getShareCount(postsId);
            string strResult = formatnumber(result);
            return strResult;
        }

        
    }
}
