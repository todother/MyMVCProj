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

        public JsonResult getWXPayResult(string DBPath,string orderId,string openId,string userPacketId)
        {
            WXPayHandler handler = new WXPayHandler();
            string result = handler.RaiseWXPay(DBPath,openId,orderId, userPacketId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMD5forPay(string openId,string orderId, string DBPath, string userPacketId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            WXPayHandler handler = new WXPayHandler();
            string prepayid = handler.RaiseWXPay(DBPath, openId, orderId,userPacketId);
            result = handler.generateMD5forPay(prepayid,DBPath);
            return Json(new { package= result["package"], nonceStr = result["nonceStr"], paySign = result["sign"], timeStamp=result["timeStamp"] }, JsonRequestBehavior.AllowGet);
        }
    }
}