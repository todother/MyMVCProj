using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatsPrj.Model;
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

		public JsonResult getPosts(int dataFrom,int count)
		{
			PostsHandler handler = new PostsHandler();
			List<PostsModel> list = new List<PostsModel>();
			list = handler.getPosts(dataFrom, count);
			return Json(new {result=list},JsonRequestBehavior.AllowGet);
		}
    }
}
