using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatsPrj.Model;
using CatsProj.BLL.Handlers;

namespace MyMVCProj.Controllers
{
    public class ElementController : Controller
    {
        // GET: Element
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getElementGroup()
        {
            List<ElementGroupModel> result = new ElementHandler().getElementGroup();
            result[0].selected = true;
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getElementDtlById(int groupId)
        {
            List<ElementDetailModel> result = new ElementHandler().getElementDtlById(groupId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ifBoughtElement(string openId,int elementId)
        {
            bool ifBought = new ElementHandler().ifBoughtElement(openId, elementId);
            return Json(new { result = ifBought }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ifSuccBuyElement(string openId,string elementId,int buyScore)
        {
            bool ifSucc = new ElementHandler().ifSuccBuyElement(openId, elementId,buyScore);
            return Json(new { result = ifSucc }, JsonRequestBehavior.AllowGet);
        }

        public void saveUserElement(string openId,int elementId)
        {
            ElementHandler handler = new ElementHandler();
            handler.saveUserElement(openId, elementId);
        }
    }
}