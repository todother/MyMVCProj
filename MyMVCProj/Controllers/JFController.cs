using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatsPrj.Model;
using CatsProj.BLL.Handlers;

namespace MyMVCProj.Controllers
{
    public class JFController : Controller
    {
        // GET: JF
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getQDTimes(string openId)
        {
            JFHandler handler = new JFHandler();
            return Json(new { result = handler.getQDTimes(openId) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult userQD(string openId)
        {
            JFHandler handler = new JFHandler();
            return Json(new { result = handler.userQD(openId) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDailyMission(string openId)
        {
            JFHandler handler = new JFHandler();
            var result = handler.getDailyMissions(openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDailyScore(string openId)
        {
            int score = new JFHandler().getDailyScore(openId);
            return Json(new { score = score }, JsonRequestBehavior.AllowGet);
        }
    }
}