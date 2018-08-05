using CatsProj.BLL.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCProj.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public JsonResult getProcessedVideo(HttpPostedFileWrapper name,double startTime,double endTime,string openId,int height,int width)
        {
            string videoId = new VideoHandler().generateVideo(startTime, endTime, name, openId,height,width);
            return Json(new { videoId = videoId, extension=new FileInfo(name.FileName).Extension }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult saveUnprocessedVideo(string openId, HttpPostedFileWrapper name,int height,int width, double latitude, double longitude, string location,string postsContent,int ifOfficial)
        {
            VideoHandler handler = new VideoHandler();
            handler.saveFullVideo(openId, name, height, width,latitude,longitude,location,postsContent,ifOfficial);
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult saveProcessedVideo(string openId,string videoId,string extension,int height,int width,double latitude,double longitude,string location,string postsContent,int ifOfficial)
        {
            VideoHandler handler = new VideoHandler();
            handler.saveProcessedVideo(openId, videoId, latitude, longitude, location, postsContent, extension,ifOfficial);
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}