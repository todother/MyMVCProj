using CatsProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCProj.Controllers
{
    public class WXPayController : Controller
    {
        // GET: WXPay
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getWXPayResult(string DBPath,string paybody,string openId)
        {
            WXPayHandler handler = new WXPayHandler();
            string result = handler.RaiseWXPay(DBPath,paybody,openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMD5forPay(string prepayid,string DBPath)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            WXPayHandler handler = new WXPayHandler();
            result = handler.generateMD5forPay(prepayid,DBPath);
            return Json(new { nonceStr = result["nonceStr"], paySign = result["sign"], timeStamp=result["timeStamp"] }, JsonRequestBehavior.AllowGet);
        }
    }
}