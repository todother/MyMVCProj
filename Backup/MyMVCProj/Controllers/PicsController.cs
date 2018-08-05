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
		public JsonResult savePics(HttpPostedFileWrapper name,string postsId,int idx)
		{
			try
			{
				PicsHandler handler = new PicsHandler();
				handler.savePics(name, postsId, idx);
				return Json(new { name = name }, JsonRequestBehavior.AllowGet);
			}
            catch(Exception e)
			{
				return Json(new { name = "fail" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}
