using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatsProj.BLL.Handlers;

namespace MyMVCProj.Controllers
{
    public class PicsController : Controller
    {
		public void savePics(HttpPostedFileWrapper name,string postsId,int idx)
		{
			PicsHandler handler = new PicsHandler();
			handler.savePics(name, postsId,idx);
		}
    }
}
