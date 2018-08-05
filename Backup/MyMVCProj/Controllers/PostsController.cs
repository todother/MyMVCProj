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
		public JsonResult savePosts(string postsMaker,string postsContent,int picsCount)
		{
			string postsId = Guid.NewGuid().ToString();
			PostsHandler handler=new PostsHandler();
			handler.savePosts(postsMaker,postsContent,picsCount,postsId);
			return Json(new { result = postsId }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult getPosts(string openId,int dataFrom,int count,DateTime refreshTime)
		{
			PostsHandler handler = new PostsHandler();
			List<PostsModel> list = new List<PostsModel>();
			list = handler.getPosts(openId,dataFrom, count,refreshTime);
			return Json(new {result=list},JsonRequestBehavior.AllowGet);
		}

		public JsonResult getPostsDetail(string postsId,string userId,int from,int count, DateTime refreshTime,string openId)
		{
			PostsHandler handler = new PostsHandler();
			PostsModel result = handler.getPostsDetail(postsId);
			bool ifLoved = handler.ifUserLoved(postsId, userId);
			long lovedTimes = handler.postsLoved( postsId);
			List<RepliesModel> replies = handler.getReplies(postsId,from, count,refreshTime,openId );
			bool ifFollowed = new UserHandler().ifFollowed(userId, postsId);
			string readCount = handler.getReadCount(postsId);
			return Json(new { result = result,ifLoved=ifLoved,lovedTimes=lovedTimes,replies=replies,ifFollowed=ifFollowed,readCount=readCount }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult getMoreReply(string postsId,  int from, int count, DateTime refreshTime,string openId)
		{
			PostsHandler handler = new PostsHandler();
			List<RepliesModel> replies = handler.getReplies(postsId, from, count, refreshTime,openId);
			return Json(new { result = replies }, JsonRequestBehavior.AllowGet);
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
			return Json(new { posts = posts, user = user,ifFollowed= ifFollowed}, JsonRequestBehavior.AllowGet);

		}

        public void delPics(string postsId)
		{
			PicsHandler handler = new PicsHandler();
			handler.delPics(postsId);
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
			long newFansCount = new UserHandler().getNewFansCount(openId);
			return Json(new { replies = list[0], loved = list[1],newFans=newFansCount }, JsonRequestBehavior.AllowGet);
		}
    }
}
