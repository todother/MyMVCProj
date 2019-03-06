using CatsPrj.Model.csDemo;
using CatsProj.BLL.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCProj.Controllers
{
    public class CSController : Controller
    {
        // GET: CS
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getCates(string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSCateModel> result = handler.getCates(DBPath);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllItems(string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSCateModel> model = handler.getAllItems(DBPath);
            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBannerList(string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSPicModel> banner = handler.getBanners(DBPath);
            return Json(new { result = banner }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getItemDetail(int itemId,string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            CSItemModel model = handler.getItemDetail(itemId, DBPath);
            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public string getUserInfo(string DBPath,string code)
        {
            CSItemHandler handler = new CSItemHandler();
            string result = handler.postWebService(DBPath, code);
            return result;
        }

        public JsonResult wxDecryptData(string sessionKey, string encryptedData, string iv, string refer,string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            bool result = handler.wxDecryptData(sessionKey, encryptedData, iv, refer,DBPath);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            //string result = AesHelper.AESDecrypt(encryptedData, sessionKey, iv);
            //return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateNickName(string openid, string nickName,string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.updateNickName(openid, nickName,DBPath);
            //bool isAdmin = handler.isAdmin(openid);
            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addCart(string DBPath,int itemId,int buyCount,string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            string cartId = handler.addCart(DBPath, openId, buyCount, itemId);
            return Json(new { result = cartId }, JsonRequestBehavior.AllowGet);
        }

        public void delCart(string cartId,string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.delCart( cartId, DBPath);
        }

        public void addItem1(string DBPath,string cartId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.addItemBy1(DBPath, cartId);
        }

        public void delItem1(string DBPath,string cartId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.delItemBy1(DBPath, cartId);
        }

        public JsonResult getShopCartItems(string DBPath, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<ShopCartItemModel> result = handler.getShopCartUnPay(DBPath, openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addOrderList(string DBPath,string cartId)
        {
            CSItemHandler handler = new CSItemHandler();
            string orderId = handler.addOrderList(DBPath, cartId);
            return Json(new { orderId = orderId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAddressList(string DBPath,string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSAddressModel> result = handler.getAddressList(DBPath, openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public void addAddress(string DBPath,string openId,string receiver,string telNo,string address,bool ifDefault)
        {

        }
    }
}