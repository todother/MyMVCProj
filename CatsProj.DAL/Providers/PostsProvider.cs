using System;
using CatsProj.DB;
using CatsDataEntity;
using MySql.Data;
using SqlSugar;
using System.Collections.Generic;
using Cats.DataEntiry;
using System.Data;
using CatsProj.DataEntiry;

namespace CatsProj.DAL.Providers
{
    public class PostsProvider
    {
        public tbl_posts getPost(string postId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_posts result = db.Queryable<tbl_posts>().Where(o => o.postsID == postId).First();
            return result;
        }

        /*public IList<tbl_posts> getPosts(string userId,int from,int count,int orderby=0)//0 means order by readed times,1 means order by createdate
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			List<tbl_posts> result = new List<tbl_posts>();
			if (orderby == 0)
			{
				result = db.Queryable<tbl_posts>().Where(o => o.postsMaker == userId)
										   .OrderBy(o=>o.postsReaded,OrderByType.Desc).Take(from + count).Skip(from)
										   .ToList();
			}
			else
			{
				result = db.Queryable<tbl_posts>().Where(o => o.postsMaker == userId)
                                           .OrderBy(o => o.postsMakeDate, OrderByType.Desc).Take(from + count).Skip(from)
                                           .ToList();
			}
			return result;
		}*/

        public int calcDistance(double pla, double plo,double ula,double ulo)
        {
            return Convert.ToInt32( Math.Floor( Math.Sqrt(Math.Pow(pla-ula,2)+Math.Pow(plo-ulo,2))));
        }

        public List<PostsPics> getPosts(string openId, int from, int count, int orderby, DateTime refreshTime,int currSel,double lati,double longti)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                List<PostsPics> result = new List<PostsPics>();
                tbl_userConfig config = new tbl_userConfig();
                config = db.Queryable<tbl_userConfig>().Where(o => o.userId == openId).First();
                var postsList = db.Queryable<tbl_posts, tbl_postspics, tbl_user>((po, pp, ur) => new object[] {
                                        JoinType.Left,po.postsID==pp.postsID,
                                        JoinType.Left,po.postsMaker==ur.openid})
                                                .Select((po, pp, ur) => new PostsPics
                                                {
                                                    postsID = po.postsID,
                                                    postsContent = SqlFunc.IIF(SqlFunc.Length(po.postsContent) >= 20, SqlFunc.Substring(po.postsContent, 0, 20), po.postsContent),
                                                    postsPics = pp.picPath,
                                                    postsMaker = ur.nickName,
                                                    postsLoved = SqlFunc.Subqueryable<tbl_userloved>().Where(tl => tl.postsID == po.postsID&&tl.loveStatus==1).Count(),
                                                    postsReaded = SqlFunc.Subqueryable<tbl_userviewed>().Where(tv => tv.postsID == po.postsID).Count(),
                                                    postsStatus = po.postsStatus,
                                                    postsMakeDate = po.postsMakeDate,
                                                    postsPicCount = po.postsPicCount,
                                                    postsReported = po.postsReported,
                                                    postsCollected = po.postsCollected,
                                                    picSimpPath = pp.picSimpPath,
                                                    openId = ur.openid,
                                                    picIndex = pp.picIndex,
                                                    postsType = po.postsType,
                                                    ifOfficial = po.ifOfficial,
                                                    picsRate=pp.picsRate,
                                                    makerPhoto=ur.avantarUrl,
                                                    ifUserLoved=SqlFunc.Subqueryable<tbl_userloved>().Where(tl=>tl.postsID==po.postsID && tl.userID==openId && tl.loveStatus==1).Count()

                                                }).Where(pp => pp.picIndex == 0);
                List<string> followeds = new List<string>();
                followeds = db.Queryable<tbl_userFollowed>().Where(o => o.userId == openId).Select(o => o.followedUser).ToList();
                if (currSel == 1)
                {
                    postsList = postsList.Where(po => (po.postsStatus != 1 || po.ifOfficial == 1)).Where(po=>po.ifLY!=1 || !SqlFunc.HasValue(po.ifLY)).OrderBy(po => po.ifOfficial, OrderByType.Desc).OrderBy(po => po.postsMakeDate, OrderByType.Desc);
                }
                else if (currSel == 3)
                {
                    postsList = postsList.Where(po => (po.postsStatus != 1 || po.ifOfficial == 1)).Where(po => po.ifLY != 1 || !SqlFunc.HasValue(po.ifLY)).OrderBy(po => po.ifOfficial, OrderByType.Desc)
                        .OrderBy(po => SqlFunc.Subqueryable<tbl_userloved>().Where(o => o.postsID == po.postsID && o.lovedTime >= DateTime.Now.AddDays(-3)).Count(), OrderByType.Desc);
                }
                else if (currSel == 2)
                {
                    postsList = postsList.Where(po => po.postsStatus != 1).Where(po => po.ifLY != 1 || !SqlFunc.HasValue(po.ifLY)).Where(ur => followeds.Contains(ur.openId)).OrderBy(po => po.ifOfficial, OrderByType.Desc).OrderBy(po=>po.postsMakeDate,OrderByType.Desc);
                }
                else if (currSel == 4)
                {
                    postsList = postsList.Where(po => po.postsStatus != 1 && po.ifLY == 1).OrderBy(po => calcDistance(po.latitude, po.longitude, lati, longti));
                }

                if (config.onlyVerify == 1)
                {
                    postsList = postsList.Where(po => po.postsStatus == 1).OrderBy(po => po.postsMakeDate, OrderByType.Desc);
                }
                postsList = postsList.Where(po => po.postsMakeDate <= refreshTime);
                return postsList.ToPageList(from / count + 1, count);
            }
            catch (Exception e)
            {
                return new List<PostsPics>();
            }
            /*
			try
			{
				if (orderby == 0)
				{
					result = db.Queryable<tbl_posts, tbl_postspics, tbl_user>((po, pp, ur) => new object[] {
										JoinType.Left,po.postsID==pp.postsID,
										JoinType.Left,po.postsMaker==ur.openid})
											.Select((po, pp, ur) => new PostsPics
											{
												postsID = po.postsID,
												postsContent = po.postsContent,
												postsPics = pp.picPath,
												postsMaker = ur.nickName,
												postsLoved = po.postsLoved,
												postsReaded = po.postsReaded,
												postsStatus = po.postsStatus,
												postsMakeDate = po.postsMakeDate,
												postsPicCount = po.postsPicCount,
												postsReported = po.postsReported,
												postsCollected = po.postsCollected,
                                                picSimpPath=pp.picSimpPath,
                                                openId=ur.openid
						                                           
					}).Where(pp=>pp.picIndex==SqlFunc.Subqueryable<tbl_postspics>().Where(o => o.picIndex == 0).Select(o => o.picIndex))
											   .OrderBy(po => po.postsReaded, OrderByType.Desc).Take(from + count).Skip(from)
											   .ToList();
				}
				else
				{
					result = db.Queryable<tbl_posts, tbl_postspics, tbl_user>((po, pp, ur) => new object[] {
										JoinType.Left,po.postsID==pp.postsID,
										JoinType.Left,po.postsMaker==ur.openid})
											.Select((po, pp, ur) => new PostsPics
											{
												postsID = po.postsID,
												postsContent = po.postsContent,
												postsPics = pp.picPath,
												postsMaker = ur.nickName,
												postsLoved = po.postsLoved,
												postsReaded = po.postsReaded,
												postsStatus = po.postsStatus,
												postsMakeDate = po.postsMakeDate,
												postsPicCount = po.postsPicCount,
												postsReported = po.postsReported,
												postsCollected = po.postsCollected,
						                        picIndex=pp.picIndex,
						                        picSimpPath = pp.picSimpPath,
                                                openId = ur.openid
						                        
					}).Where(pp => pp.picIndex == SqlFunc.Subqueryable<tbl_postspics>().Where(o => o.picIndex == 0).Select(o => o.picIndex))
											   .OrderBy(po => po.postsMakeDate, OrderByType.Desc).Take(from + count).Skip(from)
											   .ToList();
				}
				return result;
			}
            catch(Exception e)
			{
				return result;
			}*/
        }

        public IList<PostsPics> getPostsDetail(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<PostsPics> result = new List<PostsPics>();
            result = db.Queryable<tbl_posts, tbl_postspics, tbl_user>((po, pp, ur) => new object[] {
                                        JoinType.Left,po.postsID==pp.postsID,
                                        JoinType.Left,po.postsMaker==ur.openid})
                                            .Select((po, pp, ur) => new PostsPics
                                            {
                                                postsID = po.postsID,
                                                postsContent = po.postsContent,
                                                postsPics = pp.picPath,
                                                postsMaker = ur.nickName,
                                                makerPhoto = ur.avantarUrl,
                                                postsLoved = po.postsLoved,
                                                postsReaded = po.postsReaded,
                                                postsStatus = po.postsStatus,
                                                postsMakeDate = po.postsMakeDate,
                                                postsPicCount = po.postsPicCount,
                                                postsReported = po.postsReported,
                                                postsCollected = po.postsCollected,
                                                picSimpPath = pp.picSimpPath,
                                                openId = ur.openid,
                                                picIndex = pp.picIndex,
                                                latitude = po.latitude,
                                                longitude = po.longitude,
                                                postsLocation = po.postsLocation,
                                                postsType = po.postsType

                                            }).Where(pp => pp.postsID == SqlFunc.Subqueryable<tbl_posts>().Where(o => o.postsID == postsId).Select(o => o.postsID)).OrderBy(pp => pp.picIndex).ToList();
            return result;
        }

        public void savePosts(tbl_posts posts)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_posts>(posts).ExecuteCommand();
        }


        public void lovePosts(tbl_userloved entity)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_userloved userloved = db.Queryable<tbl_userloved>().Where(o => o.postsID == entity.postsID && o.userID == entity.userID).First();
            if (userloved == null)
            {

                db.Insertable<tbl_userloved>(entity).ExecuteCommand();
            }
            else if (userloved.loveStatus == 1)
            {
                db.Updateable<tbl_userloved>().UpdateColumns(o => new tbl_userloved { loveStatus = 0 }).Where(o => o.postsID == entity.postsID && o.userID == entity.userID).ExecuteCommand();
            }
            else
            {
                db.Updateable<tbl_userloved>().UpdateColumns(o => new tbl_userloved { loveStatus = 1 }).Where(o => o.postsID == entity.postsID && o.userID == entity.userID).ExecuteCommand();
            }
        }

        public Boolean ifUserLoved(string postsId, string userId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            Boolean result = false;
            tbl_userloved entity = db.Queryable<tbl_userloved>().Where(o => o.postsID == postsId && o.userID == userId).First();
            if (entity != null && entity.loveStatus == 1)
            {
                result = true;

            }
            return result;
        }

        public void viewPosts(tbl_userviewed entity)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_userloved>(entity).ExecuteCommand();
            
        }

        public long getUserViewed(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long total = db.Queryable<tbl_userviewed>().Where(o => o.postsID == postsId).Count();
            return total;
        }

        public long getUserLoved(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long total = db.Queryable<tbl_userloved>().Where(o => o.postsID == postsId && o.loveStatus == 1).Count();
            return total;
        }
        public void userReply(tbl_reply entity)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_reply>(entity).ExecuteCommand();
        }

        public UserReply getReplyDetail(string replyId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            UserReply userReplies = db.Queryable<tbl_user, tbl_reply, tbl_posts>((tu, tr, tp) => new object[]{
                JoinType.Left,tu.openid==tr.replyMaker,
                JoinType.Left,tr.postsID==tp.postsID
            }).Select((tu, tr, tp) => new UserReply
            {
                replyMaker = tr.replyMaker,
                avantarUrl = tu.avantarUrl,
                nickName = tu.nickName,
                replyId = tr.replyID,
                replyContent = tr.replyContent,
                postsId = tp.postsID,
                replyDate = tr.replyDate,
                lovedCount = SqlFunc.Subqueryable<tbl_userReplyLoved>().Where(o => o.replyId == tr.replyID).Count()
            }).Where(tr => tr.replyId == replyId).First();
            return userReplies;
        }

        public List<UserReply> getUserReplies(int from, int count, string postsId, DateTime refreshTime, string openId)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                List<UserReply> userReplies = db.Queryable<tbl_user, tbl_reply, tbl_posts>((tu, tr, tp) => new object[]{
                JoinType.Left,tu.openid==tr.replyMaker,
                JoinType.Left,tr.postsID==tp.postsID
            }).Select((tu, tr, tp) => new UserReply
            {
                replyMaker = tr.replyMaker,
                avantarUrl = tu.avantarUrl,
                nickName = tu.nickName,
                replyId = tr.replyID,
                replyContent = tr.replyContent,
                postsId = tp.postsID,
                replyDate = tr.replyDate,
                lovedCount = SqlFunc.Subqueryable<tbl_userReplyLoved>().Where(o => o.replyId == tr.replyID).Count()
            }).Where(tp => tp.postsId == SqlFunc.Subqueryable<tbl_posts>().Where(o => o.postsID == postsId).Select(o => o.postsID)).
                  Where(tr => tr.replyDate <= refreshTime && tr.replyMaker != openId).OrderBy(tr => tr.replyDate, OrderByType.Desc).ToPageList(from / count + 1, count);

                List<string> userLoved = db.Queryable<tbl_posts, tbl_reply, tbl_userReplyLoved>((tp, tr, tl) => new object[]{
                JoinType.Left,tp.postsID==tr.postsID,
                JoinType.Left,tr.replyID==tl.replyId
            }).Select((tp, tr, tl) => new UserLovedReplybyPID
            {
                replyId = tr.replyID,
                postsId = tp.postsID,
                openId = tl.openId
            }).Where(tp => tp.postsId == postsId).Where(tl => tl.openId == openId).Select(tr => tr.replyId).ToList();

                /*string repliesID = string.Empty;
				userReplies.ForEach(o => repliesID=repliesID+"'"+o.replyId+"',");
				repliesID = repliesID.Substring(0, repliesID.Length - 1);

				var lovedCount = db.Ado.GetDataTable(@"select tr.replyId,count(tl.replyId) as lovedCount from tbl_reply tr
                left join tbl_userreplyloved tl on tr.trplyId=tl.replyId
                where tr.replyId in (@condition) group by tl.replyId", new { condition = repliesID });*/

                List<UserReply> replyList = new List<UserReply>();
                foreach (var item in userReplies)
                {
                    if (userLoved.Contains(item.replyId))
                    {
                        item.replyLoved = true;
                    }
                    else
                    {
                        item.replyLoved = false;
                    }
                    replyList.Add(item);
                }


                return replyList;
            }
            catch (Exception e)
            {
                return new List<UserReply>();
            }
        }

        public List<UserReply> getMyReplies(int from, int count, string postsId, DateTime refreshTime, string openId)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                List<string> admins = db.Queryable<tbl_admin>().Select(o => o.openId).ToList();
                List<UserReply> userReplies = db.Queryable<tbl_user, tbl_reply, tbl_posts>((tu, tr, tp) => new object[]{
                JoinType.Left,tu.openid==tr.replyMaker,
                JoinType.Left,tr.postsID==tp.postsID
            }).Select((tu, tr, tp) => new UserReply
            {
                replyMaker = tr.replyMaker,
                avantarUrl = tu.avantarUrl,
                nickName = tu.nickName,
                replyId = tr.replyID,
                replyContent = tr.replyContent,
                postsId = tp.postsID,
                replyType = SqlFunc.IIF(tr.replyMaker == openId, 1, 0),
                lovedCount = SqlFunc.Subqueryable<tbl_userReplyLoved>().Where(o => o.replyId == tr.replyID).Count(),
                isAdmin=SqlFunc.IIF(admins.Contains(tr.replyMaker),true,false)
            }).Where(tp => tp.postsId == SqlFunc.Subqueryable<tbl_posts>().Where(o => o.postsID == postsId).Select(o => o.postsID)).
                  Where(tr => tr.replyDate <= refreshTime).OrderBy(tr => SqlFunc.IIF(admins.Contains(tr.replyMaker), 2, SqlFunc.IIF(tr.replyMaker == openId, 1, 0)), OrderByType.Desc).OrderBy(tr => tr.replyDate, OrderByType.Asc).ToPageList(from / count + 1, count);

                List<string> userLoved = db.Queryable<tbl_posts, tbl_reply, tbl_userReplyLoved>((tp, tr, tl) => new object[]{
                JoinType.Left,tp.postsID==tr.postsID,
                JoinType.Left,tr.replyID==tl.replyId
            }).Select((tp, tr, tl) => new UserLovedReplybyPID
            {
                replyId = tr.replyID,
                postsId = tp.postsID,
                openId = tl.openId
            }).Where(tp => tp.postsId == postsId).Where(tl => tl.openId == openId).Select(tr => tr.replyId).ToList();

                /*string repliesID = string.Empty;
				userReplies.ForEach(o => repliesID=repliesID+"'"+o.replyId+"',");
				repliesID = repliesID.Substring(0, repliesID.Length - 1);

				var lovedCount = db.Ado.GetDataTable(@"select tr.replyId,count(tl.replyId) as lovedCount from tbl_reply tr
                left join tbl_userreplyloved tl on tr.trplyId=tl.replyId
                where tr.replyId in (@condition) group by tl.replyId", new { condition = repliesID });*/

                List<UserReply> replyList = new List<UserReply>();
                foreach (var item in userReplies)
                {
                    if (userLoved.Contains(item.replyId))
                    {
                        item.replyLoved = true;
                    }
                    else
                    {
                        item.replyLoved = false;
                    }
                    replyList.Add(item);
                }


                return replyList;
            }
            catch (Exception e)
            {
                return new List<UserReply>();
            }
        }

        public List<PostsPics> getPostsByMaker(string openId, int from, int count)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<PostsPics> result = new List<PostsPics>();
            result = db.Queryable<tbl_posts, tbl_postspics, tbl_user>((po, pp, ur) => new object[] {
                                        JoinType.Left,po.postsID==pp.postsID,
                                        JoinType.Left,po.postsMaker==ur.openid})
                                            .Select((po, pp, ur) => new PostsPics
                                            {
                                                postsID = po.postsID,
                                                postsContent = po.postsContent,
                                                postsPics = pp.picPath,
                                                postsMaker = ur.nickName,
                                                postsLoved = po.postsLoved,
                                                postsReaded = po.postsReaded,
                                                postsStatus = po.postsStatus,
                                                postsMakeDate = po.postsMakeDate,
                                                postsPicCount = po.postsPicCount,
                                                postsReported = po.postsReported,
                                                postsCollected = po.postsCollected,
                                                picIndex = pp.picIndex,
                                                picSimpPath = pp.picSimpPath,
                                                openId = ur.openid,
                                                postsType = po.postsType

                                            }).Where(ur => ur.openId == SqlFunc.Subqueryable<tbl_user>().Where(o => o.openid == openId).Select(o => o.openid))
                .OrderBy(po => po.postsMakeDate, OrderByType.Desc).OrderBy(pp => pp.picIndex, OrderByType.Asc).ToPageList(from / count + 1, count);

            return result;
        }

        public List<tbl_postspics> delPosts(string postsId, int delType, string openId)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                List<tbl_postspics> postspics = new List<tbl_postspics>();
                tbl_posts posts = new tbl_posts();
                posts = db.Queryable<tbl_posts>().Where(o => o.postsID == postsId).First();

                db.Insertable<tbl_delReason>(new tbl_delReason
                {
                    delId = Guid.NewGuid().ToString(),
                    delContent = posts.postsContent,
                    delTime = DateTime.Now,
                    delType = delType == 1 ? DelType.ReplyDel : DelType.SeriousDel,
                    delUser = openId,
                    delOpenId = posts.postsMaker
                }).ExecuteCommand();

                if (delType == 2)
                {
                    tbl_user user = new tbl_user();
                    user = db.Queryable<tbl_user>().Where(o => o.openid == posts.postsMaker).First();
                    db.Updateable<tbl_user>().UpdateColumns(o => new tbl_user { userStatus = 1 }).Where(o => o.openid == user.openid).ExecuteCommand();
                    List<string> postsList = db.Queryable<tbl_posts>().Where(o => o.postsMaker == posts.postsMaker).Select(o => o.postsID).ToList();
                    db.Deleteable<tbl_postspics>().Where(o => postsList.Contains(o.postsID)).ExecuteCommand();
                    db.Deleteable<tbl_reply>().Where(o => o.replyMaker == posts.postsMaker).ExecuteCommand();
                    db.Deleteable<tbl_reply>().Where(o => postsList.Contains(o.postsID)).ExecuteCommand();
                    db.Deleteable<tbl_replyAfterReply>().Where(o => o.replyMaker == posts.postsMaker).ExecuteCommand();
                }

                postspics = db.Queryable<tbl_postspics>().Where(o => o.postsID == postsId).ToList();
                db.Deleteable<tbl_posts>().Where(o => o.postsID == postsId).ExecuteCommand();
                db.Deleteable<tbl_postspics>().Where(o => o.postsID == postsId).ExecuteCommand();
                db.Deleteable<tbl_userloved>().Where(o => o.postsID == postsId).ExecuteCommand();
                return postspics;
            }
            catch (Exception e)
            {
                return new List<tbl_postspics>();
            }
        }

        public void userViewPosts(string openId, string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_userviewed>(new tbl_userviewed { viewId = Guid.NewGuid().ToString(), postsID = postsId, userID = openId, viewTime = DateTime.Now }).ExecuteCommand();

        }

        public long getReadCount(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long readCount = db.Queryable<tbl_userviewed>().Where(o => o.postsID == postsId).GroupBy(o=>new { o.userID,o.postsID}).Select(o=>o.userID).Count();
            return readCount;
        }

        public void userLoveReply(string openId, string replyId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_userReplyLoved replyLoved = db.Queryable<tbl_userReplyLoved>().Where(o => o.replyId == replyId && o.openId == openId).First();
            if (replyLoved == null)
            {
                db.Insertable<tbl_userReplyLoved>(new tbl_userReplyLoved { lovedId = Guid.NewGuid().ToString(), openId = openId, replyId = replyId, lovedTime = DateTime.Now }).ExecuteCommand();
            }
            else
            {
                db.Deleteable<tbl_userReplyLoved>().Where(o => o.replyId == replyId && o.openId == openId).ExecuteCommand();
            }
        }

        public void delReply(string replyId, string openId, int delType)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_reply reply = new tbl_reply();
            reply = db.Queryable<tbl_reply>().Where(o => o.replyID == replyId).First();
            db.Insertable<tbl_delReason>(new tbl_delReason
            {
                delId = Guid.NewGuid().ToString(),
                delContent = reply.replyContent,
                delTime = DateTime.Now,
                delType = delType == 0 ? DelType.ReplyDel : DelType.SeriousDel,
                delUser = openId,
                delOpenId = reply.replyMaker
            }).ExecuteCommand();

            if (delType == 2)
            {
                tbl_user user = new tbl_user();
                user = db.Queryable<tbl_user>().Where(o => o.openid == reply.replyMaker).First();
                db.Updateable<tbl_user>().UpdateColumns(o => new tbl_user { userStatus = 1 }).Where(o => o.openid == user.openid).ExecuteCommand();
                List<string> postsList = db.Queryable<tbl_posts>().Where(o => o.postsMaker == reply.replyMaker).Select(o => o.postsID).ToList();
                db.Deleteable<tbl_postspics>().Where(o => postsList.Contains(o.postsID)).ExecuteCommand();
                db.Deleteable<tbl_reply>().Where(o => o.replyMaker == reply.replyMaker).ExecuteCommand();
                db.Deleteable<tbl_reply>().Where(o => postsList.Contains(o.postsID)).ExecuteCommand();
                db.Deleteable<tbl_replyAfterReply>().Where(o => o.replyMaker == reply.replyMaker).ExecuteCommand();
            }

            db.Deleteable<tbl_reply>().Where(o => o.replyID == replyId).ExecuteCommand();
            db.Deleteable<tbl_userReplyLoved>().Where(o => o.replyId == replyId).ExecuteCommand();
        }

        public List<List<ReplyNLoveCount>> getReplyNLoveCount(string openId)
        {
            DateTime lastRefresh = new UserProvider().getLastRefreshDate(openId);
            SqlSugarClient db = SqlSugarInstance.newInstance();
            DataTable replyCount = db.Ado.UseStoredProcedure().GetDataTable("proc_getReplyCount", new { openId = openId, fromwhen = lastRefresh });
            var i = 0;
            List<ReplyNLoveCount> replyCountList = new List<ReplyNLoveCount>();
            if (replyCount.Rows.Count > 0)
            {
                for (i = 0; i < replyCount.Rows.Count; i++)
                {
                    replyCountList.Add(new ReplyNLoveCount
                    {
                        postsId = replyCount.Rows[i][0].ToString(),
                        replyCount = Convert.ToInt64(replyCount.Rows[i][1]),
                        picsSimpPath = replyCount.Rows[i][2].ToString(),
                        postsLoveCount = 0
                    });
                }
            }
            DataTable lovedCount = db.Ado.UseStoredProcedure().GetDataTable("proc_getPostsLoved", new { openId = openId, fromwhen = lastRefresh });
            i = 0;
            List<ReplyNLoveCount> lovedCountList = new List<ReplyNLoveCount>();
            if (lovedCount.Rows.Count > 0)
            {
                for (i = 0; i < lovedCount.Rows.Count; i++)
                {
                    lovedCountList.Add(new ReplyNLoveCount
                    {
                        postsId = lovedCount.Rows[i][0].ToString(),
                        postsLoveCount = Convert.ToInt64(lovedCount.Rows[i][1]),
                        picsSimpPath = lovedCount.Rows[i][2].ToString(),
                        replyCount = 0
                    });
                }
            }
            List<List<ReplyNLoveCount>> result = new List<List<ReplyNLoveCount>>();
            result.Add(replyCountList);
            result.Add(lovedCountList);
            return result;
        }

        public List<List<ReplyNLoveCount>> getAfterReplyNLoveCount(string openId)
        {
            DateTime lastRefresh = new UserProvider().getLastRefreshDate(openId);
            SqlSugarClient db = SqlSugarInstance.newInstance();
            DataTable replyCount = db.Ado.UseStoredProcedure().GetDataTable("proc_getAfterReplyCount", new { openId = openId, fromwhen = lastRefresh });
            var i = 0;
            List<ReplyNLoveCount> replyCountList = new List<ReplyNLoveCount>();
            if (replyCount.Rows.Count > 0)
            {
                for (i = 0; i < replyCount.Rows.Count; i++)
                {
                    replyCountList.Add(new ReplyNLoveCount
                    {
                        postsId = replyCount.Rows[i][0].ToString(),
                        replyCount = Convert.ToInt64(replyCount.Rows[i][2]),
                        picsSimpPath = replyCount.Rows[i][1].ToString(),
                        postsLoveCount = 0
                    });
                }
            }
            DataTable lovedCount = db.Ado.UseStoredProcedure().GetDataTable("proc_getAfterReplyLoved", new { openId = openId, fromwhen = lastRefresh });
            i = 0;
            List<ReplyNLoveCount> lovedCountList = new List<ReplyNLoveCount>();
            if (lovedCount.Rows.Count > 0)
            {
                for (i = 0; i < lovedCount.Rows.Count; i++)
                {
                    lovedCountList.Add(new ReplyNLoveCount
                    {
                        postsId = lovedCount.Rows[i][0].ToString(),
                        postsLoveCount = Convert.ToInt64(lovedCount.Rows[i][2]),
                        picsSimpPath = lovedCount.Rows[i][1].ToString(),
                        replyCount = 0
                    });
                }
            }
            List<List<ReplyNLoveCount>> result = new List<List<ReplyNLoveCount>>();
            result.Add(replyCountList);
            result.Add(lovedCountList);
            return result;
        }

        public void newReport(tbl_report report)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            if (db.Queryable<tbl_report>().Where(o => o.postsId == report.postsId && o.userId == report.userId).Count() == 0)
            {
                db.Insertable<tbl_report>(report).ExecuteCommand();
            }
        }

        public long getReportTimes(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long reportTimes = db.Queryable<tbl_report>().Where(o => o.postsId == postsId).Count();
            return reportTimes;
        }

        public long getViewedTimes(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long viewedTimes = db.Queryable<tbl_userviewed>().Where(o => o.postsID == postsId).Count();
            return viewedTimes;
        }

        public bool ifCanRead(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            bool ifValid = db.Queryable<tbl_posts>().Where(o => o.postsID == postsId).First().postsStatus != 1;
            return ifValid;
        }

        public void updatePostsStatus(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Updateable<tbl_posts>().UpdateColumns(o => new tbl_posts { postsStatus = 1 }).Where(o => o.postsID == postsId).ExecuteCommand();
        }

        public long getWaitingList()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long count = db.Queryable<tbl_posts>().Where(o => o.postsStatus == 1).Count();
            return count;
        }

        public void verifyPosts(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Updateable<tbl_posts>().UpdateColumns(o => new tbl_posts { postsStatus = 2 }).Where(o => o.postsID == postsId).ExecuteCommand();
            db.Deleteable<tbl_report>().Where(o => o.postsId == postsId).ExecuteCommand();//将已存在的举报记录删除
        }

        public bool ifCanReport(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            return db.Queryable<tbl_posts>().Where(o => o.postsID == postsId).First().postsStatus <= 1;//2为已审核通过的，不可再次举报
        }

        public string addReplyAfterReply(string replyId, string openId, string replyContent, string replyToUser)
        {
            tbl_replyAfterReply reply = new tbl_replyAfterReply();
            reply.afterReplyId = Guid.NewGuid().ToString();
            reply.replyContent = replyContent;
            reply.replyId = replyId;
            reply.replyDate = DateTime.Now;
            reply.replyMaker = openId;
            reply.replyToUser = replyToUser;
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_replyAfterReply>(reply).ExecuteCommand();
            return reply.afterReplyId;
        }

        public List<UserReply> getAfterReplyList(string replyId, int from, int count, DateTime refreshTime)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<UserReply> replyList = db.Queryable<tbl_replyAfterReply, tbl_user, tbl_user>((tr, tu, tu2) => new object[] {
                JoinType.Left,tr.replyMaker==tu.openid,
                JoinType.Left,tr.replyToUser==tu2.openid
            }).Select((tr, tu, tu2) => new UserReply
            {
                replyId = tr.afterReplyId,
                replyContent = tr.replyContent,
                replyDate = tr.replyDate,
                replyMaker = tr.replyMaker,
                nickName = tu.nickName,
                avantarUrl = tu.avantarUrl,
                replyToUser = tu2.nickName
            }).Where(tr => tr.replyId == replyId && tr.replyDate <= refreshTime).OrderBy(tr => tr.replyDate).ToList();
            return replyList;
        }

        public void delReplyAfterReply(string afterReplyId, string openId, int delType)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();

            tbl_replyAfterReply reply = db.Queryable<tbl_replyAfterReply>().Where(o => o.afterReplyId == afterReplyId).First();
            db.Insertable<tbl_delReason>(new tbl_delReason
            {
                delId = Guid.NewGuid().ToString(),
                delContent = reply.replyContent,
                delTime = DateTime.Now,
                delType = delType == 0 ? DelType.ReplyDel : DelType.SeriousDel,
                delUser = openId,
                delOpenId = reply.replyMaker
            }).ExecuteCommand();

            if (delType == 2)
            {
                tbl_user user = new tbl_user();
                user = db.Queryable<tbl_user>().Where(o => o.openid == reply.replyMaker).First();
                db.Updateable<tbl_user>().UpdateColumns(o => new tbl_user { userStatus = 1 }).Where(o => o.openid == user.openid).ExecuteCommand();
                List<string> postsList = db.Queryable<tbl_posts>().Where(o => o.postsMaker == reply.replyMaker).Select(o => o.postsID).ToList();
                db.Deleteable<tbl_postspics>().Where(o => postsList.Contains(o.postsID)).ExecuteCommand();
                db.Deleteable<tbl_reply>().Where(o => o.replyMaker == reply.replyMaker).ExecuteCommand();
                db.Deleteable<tbl_reply>().Where(o => postsList.Contains(o.postsID)).ExecuteCommand();
                db.Deleteable<tbl_replyAfterReply>().Where(o => o.replyMaker == reply.replyMaker).ExecuteCommand();
            }

            db.Deleteable<tbl_replyAfterReply>().Where(o => o.afterReplyId == afterReplyId).ExecuteCommand();
            //db.Deleteable<tbl_userReplyLoved>().Where(o => o.replyId == replyId).ExecuteCommand();
        }

        public bool ifUserLovedReply(string openId, string replyId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_userReplyLoved loveReply = new tbl_userReplyLoved();
            loveReply = db.Queryable<tbl_userReplyLoved>().Where(o => o.replyId == replyId && o.openId == openId).First();
            if (loveReply != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void sharePosts(string openId,string postsId)
        {
            string shareId = Guid.NewGuid().ToString();
            tbl_userShare userShare = new tbl_userShare();
            userShare.userId = openId;
            userShare.postsId = postsId;
            userShare.shareId = shareId;
            userShare.shareTime = DateTime.Now;
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_userShare>(userShare).ExecuteCommand();
        }

        public long getRepliesCount(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long repliesCount = db.Queryable<tbl_reply>().Where(o => o.postsID == postsId).Count();
            return repliesCount;
        }

        public long saveShareCode(string postsId,string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_sharecode code = new tbl_sharecode();
            code.openId = openId;
            code.postsId = postsId;
            code.shareTime = DateTime.Now;
            long shareId = db.Insertable<tbl_sharecode>(code).ExecuteReturnBigIdentity();
            return shareId;
        }

        public tbl_sharecode getShareCodeRecord(string shareId)
        {
            long shareIdLong = Convert.ToInt64(shareId);
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_sharecode result = db.Queryable<tbl_sharecode>().Where(o => o.shareId == shareIdLong).First();
            return result;
        }

        public List<tbl_event> getEventList()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_event> eventList = new List<tbl_event>();
            eventList = db.Queryable<tbl_event>().OrderBy(o => o.eventIndex).ToList();
            return eventList;
        }

        public long getShareCount(string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long result = db.Queryable<tbl_userShare>().Where(o => o.postsId == postsId).Count() + db.Queryable<tbl_sharecode>().Where(o => o.postsId == postsId).Count();
            return result;
        }
    }
}
