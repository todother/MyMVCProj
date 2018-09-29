using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatsPrj.Model;
using CatsProj.BLL.Handler;
using CatsProj.BLL.Handlers;

namespace MyMVCProj.Controllers
{
    public class PostsController : Controller
    {
		public JsonResult savePosts(string postsMaker,string postsContent,int picsCount,double longitude, double latitude, string location,int ifOfficial,int ifLY)
		{
			string postsId = Guid.NewGuid().ToString();
			PostsHandler handler=new PostsHandler();
			bool succ=handler.savePosts(postsMaker,postsContent,picsCount,postsId, latitude, longitude, location,"P",ifOfficial,ifLY);
			return Json(new { result = postsId,ifSucc=succ }, JsonRequestBehavior.AllowGet);
		}

        public JsonResult getPosts(string openId, int dataFrom, int count, DateTime refreshTime,int currentSel,double ulo,double ula)
		{
			PostsHandler handler = new PostsHandler();
			List<PostsModel> list = new List<PostsModel>();
			list = handler.getPosts(openId,dataFrom, count,refreshTime, currentSel, ula, ulo);
			return Json(new {result=list},JsonRequestBehavior.AllowGet);
		}

        public JsonResult getPostsDetail(string postsId, string userId, int from, int count, DateTime refreshTime, string openId)
        {
            PostsHandler handler = new PostsHandler();
            handler.viewPosts(openId, postsId);
            PostsModel result = new PostsModel();
            bool ifLoved = false;
            List<RepliesModel> replies = new List<RepliesModel>();
            bool ifFollowed = false;
            string lovedTimes = "0";
            string readCount = string.Empty;
            bool ifLegal = handler.ifLegalPosts(postsId);
            List<RepliesModel> myReplies = new List<RepliesModel>();
            string repliesCount = string.Empty;
            string shareCount = string.Empty;

            result = handler.getPostsDetail(postsId);
            ifLoved = handler.ifUserLoved(postsId, userId);
            repliesCount = handler.getRepliesCount(postsId);
            lovedTimes = handler.postsLoved(postsId);
            myReplies = handler.getMyReplies(postsId, from, count, refreshTime, openId);
            //replies = handler.getReplies(postsId, from, count, refreshTime, openId);
            ifFollowed = new UserHandler().ifFollowed(userId, postsId);
            readCount = handler.getReadCount(postsId);
            ifLegal = ifLegal && result.postsStatus != 1;
            shareCount = handler.getShareCount(postsId);
            bool ifMuted = new UserHandler().getConfigModel(openId).videoMuted == 1 ? true:false ;
            return Json(new { result = result, ifLoved = ifLoved, lovedTimes = lovedTimes, repliesCount= repliesCount,shareCount=shareCount, replies = replies, ifFollowed = ifFollowed, readCount = readCount, ifLegal = ifLegal,myReply=myReplies, ifMuted=ifMuted }, JsonRequestBehavior.AllowGet);
        }

		public JsonResult getMoreReply(string postsId,  int from, int count, DateTime refreshTime,string openId)
		{
			PostsHandler handler = new PostsHandler();
			//List<RepliesModel> replies = handler.getReplies(postsId, from, count, refreshTime,openId);
            List<RepliesModel> myReplies = handler.getMyReplies(postsId, from, count, refreshTime, openId);
            return Json(new {  myReply=myReplies }, JsonRequestBehavior.AllowGet);
		}

        public void userLoved(string postsId,string userId)
		{
			PostsHandler handler = new PostsHandler();
			handler.userLoved(postsId, userId);
		}

		public void userReply(string replyContent,string userId,string postsId)
		{
			PostsHandler handler = new PostsHandler();
			handler.userReply(replyContent,postsId,userId);
		}

        public JsonResult getPostsByMaker(string openId,string userId,int from, int count)
		{
			PostsHandler pHandler = new PostsHandler();
			UserHandler uHandler = new UserHandler();

			List<PostsModel> posts = pHandler.getPostsByMaker(openId, from, count);
			UserModel user = uHandler.getUserInfo(openId);
			bool ifFollowed = uHandler.ifUserFollowedByOpenId(userId, openId);
            string fansCount = uHandler.transferFansCountToString( uHandler.getFansCount(openId));
            string followedCount= uHandler.transferFansCountToString(uHandler.getFollowedCount(openId));
            string lovedCount= uHandler.transferFansCountToString(uHandler.getLovedCount(openId));
            return Json(new { posts = posts, user = user,ifFollowed= ifFollowed, fansCount = fansCount,followedCount=followedCount,lovedCount=lovedCount }, JsonRequestBehavior.AllowGet);

		}

        public void delPics(string postsId,int delType,string openId)
		{
			PicsHandler handler = new PicsHandler();
			handler.delPics(postsId,delType,openId);
		}

		public void viewPosts(string openId,string postsId)
		{
			PostsHandler handler = new PostsHandler();
			handler.viewPosts(openId, postsId);
		}

		public void userLoveReply(string openId,string replyId)
		{
			new PostsHandler().userLoveReply(openId, replyId);
		}

        public void delReply(string openId,string replyId,int delType)
		{
			new PostsHandler().delReply(openId,replyId,delType);
		}


        public JsonResult getReplyNLovedCount(string openId)
		{
			List<List<ReplyNLoveModel>> list = new PostsHandler().getReplyNLoveCount(openId);
            List<List<ReplyNLoveModel>> afterList = new PostsHandler().getAfterReplyNLoveCount(openId);
            long newFansCount = new UserHandler().getNewFansCount(openId);
			return Json(new { replies = list[0], loved = list[1],newFans=newFansCount,afterReplies=afterList[0],replyLoved=afterList[1] }, JsonRequestBehavior.AllowGet);
		}

        public JsonResult getToken()
        {
            TokenHandler handler = new TokenHandler();
            return Json(new { result = handler.getToken() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult checkIfValidContent(string content)
        {
            bool ifValid = new PostsHandler().ifValidContent(content);
            return Json(new { result = ifValid }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult reportPosts(string postsId,string openId)
        {
            PostsHandler handler = new PostsHandler();
            bool ifcanReport = handler.ifCanReport(postsId);
            if (ifcanReport)
            {
                handler.reportPosts(postsId, openId);
            }
            return Json(new { result = ifcanReport }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult getWaitingList()
        {
            long count = new PostsHandler().getWaitingPosts();
            return Json(new { count=count }, JsonRequestBehavior.AllowGet);
        }

        public void verifyPosts(string postsId)
        {
            new PostsHandler().verifyPosts(postsId);
        }

        public JsonResult addReplyAfterReply(string replyId,string openId,string replyContent,string replyToUser)
        {
            string afterReplyId= new PostsHandler().addReplyAfterReply(replyId, replyContent, openId,replyToUser);
            return Json(new { afterReplyId = afterReplyId }, JsonRequestBehavior.AllowGet);
        }

        public void delReplyAfterReply(string afterReplyId,string openId,int delType)
        {
            new PostsHandler().delReplyAfterReply(afterReplyId, openId, delType);
        }

        public JsonResult getReplyDetail(string replyId,string openId,int from, int count,DateTime refreshTime)
        {
            var result = new PostsHandler().getReplyDetail(replyId, from, count,refreshTime);
            bool ifUserLoved = new PostsHandler().ifUserLovedReply(openId, replyId);
            return Json(new { result = result, replyLoved = ifUserLoved }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMoreAfterReplies(string replyId,int from,int count,DateTime refreshTime)
        {
            var result = new PostsHandler().getMoreAfterReply(replyId, from, count,refreshTime);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public void userSharePosts(string openId,string postsId)
        {
            new PostsHandler().userSharePosts(openId, postsId);
        }

        public JsonResult generateQRCode(string openId,string postsId)
        {
            PostsHandler handler = new PostsHandler();
            string qrCode = handler.getQRCode(openId, postsId);
            return Json(new { result = qrCode }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSharePostsId(string shareId)
        {
            PostsHandler handler = new PostsHandler();
            string postsId = handler.getPostsIdFromQRCode(shareId);
            return Json(new { result = postsId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEventList()
        {
            List<EventModel> result = new PostsHandler().getEventsList();
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
