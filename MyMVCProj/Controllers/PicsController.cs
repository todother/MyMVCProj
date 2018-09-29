using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatsPrj.Model;
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
                
				string result= handler.savePics(name, postsId, idx);
				return Json(new { result=result }, JsonRequestBehavior.AllowGet);
			}
            catch(Exception e)
			{
				return Json(new { name = "fail" }, JsonRequestBehavior.AllowGet);
			}
		}

        public void generateRate()
        {
            new PicsHandler().calcRate("");
        }

        public JsonResult getPicEffects()
        {
            PicsHandler handler = new PicsHandler();
            List<PicEffectModel> models = handler.getEffects();
            return Json(new { result = models }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult generatePicEffects(HttpPostedFileWrapper name,decimal hrate,decimal srate,decimal lrate,int idx)
        //{
        //    string result = new PicsHandler().generatePicEffects(name, idx, hrate, srate, lrate);
        //    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        //}

        //public void delTempPic(string filePath)
        //{
        //    new PicsHandler().delTempPic(filePath);
        //}

        public void generatePics()
        {
            new PicsHandler().generateFilters();
        }

        public JsonResult getPosterInfo()
        {
            PosterModel layout = new PicsHandler().getPosterLayout();
            PosterContentModel content = new PicsHandler().getPosterContent();
            return Json(new { layout = layout, content = content }, JsonRequestBehavior.AllowGet);
        }
    }
}
