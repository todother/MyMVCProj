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


        public JsonResult getAllCates(string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSCateModel> result = handler.getAllCates(DBPath);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllItems(string DBPath, string openId,int from,int size)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSCateModel> model = handler.getAllItems(DBPath, openId,from,size);
            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBannerList(string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSPicModel> banner = handler.getBanners(DBPath);
            return Json(new { result = banner }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getItemDetail(int itemId, string DBPath, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            CSItemModel model = handler.getItemDetail(itemId, DBPath);
            bool ifLoved = handler.ifUserLoveItem(DBPath, openId, itemId);
            int totalSold = handler.getItemTotalSold(DBPath, itemId);
            handler.userViewItem(DBPath, openId, itemId);
            return Json(new { result = model, ifLoved = ifLoved, totalSold= totalSold }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getItemDetailByBarcode(string barcode,string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            CSItemModel model = handler.getItemDetailByBarcode(barcode, DBPath);
            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getItemIdByBarcode(string barcode, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            int itemId = handler.getItemIdByBarcode(barcode, DBPath);
            return Json(new { result = itemId }, JsonRequestBehavior.AllowGet);
        }

        public string getUserInfo(string DBPath, string code)
        {
            CSItemHandler handler = new CSItemHandler();
            string result = handler.postWebService(DBPath, code);
            return result;
        }

        public JsonResult wxDecryptData(string sessionKey, string encryptedData, string iv, string refer, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            bool result = handler.wxDecryptData(sessionKey, encryptedData, iv, refer, DBPath);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            //string result = AesHelper.AESDecrypt(encryptedData, sessionKey, iv);
            //return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateNickName(string openid, string nickName, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            bool isAdmin = handler.updateNickName(openid, nickName, DBPath);

            return Json(new { result = "OK", isAdmin = isAdmin }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addCart(string DBPath, int itemId, int buyCount, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            string cartId = handler.addCart(DBPath, openId, buyCount, itemId);
            return Json(new { result = cartId }, JsonRequestBehavior.AllowGet);
        }

        public void delCart(string cartId, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.delCart(cartId, DBPath);
        }

        public void addItem1(string DBPath, string cartId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.addItemBy1(DBPath, cartId);
        }

        public void delItem1(string DBPath, string cartId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.delItemBy1(DBPath, cartId);
        }

        public void addCartBy1(string DBPath, int itemId, string openId, int buycount)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.addCartBy1(DBPath, itemId, openId, buycount);
        }

        public JsonResult getShopCartItems(string DBPath, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<ShopCartItemModel> result = handler.getShopCartUnPay(DBPath, openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addOrderList(string DBPath, string cartId, string addressId)
        {
            CSItemHandler handler = new CSItemHandler();
            string orderId = handler.addOrderList(DBPath, cartId, addressId);
            return Json(new { orderId = orderId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAddressList(string DBPath, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSAddressModel> result = handler.getAddressList(DBPath, openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addAddress(string DBPath, string openId, string receiver, string telNo, string address, bool ifDefault, string addId)
        {
            CSItemHandler handler = new CSItemHandler();
            string result = handler.addAddress(DBPath, openId, receiver, telNo, address, ifDefault, addId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getAddressByAddressId(string DBPath, string addressId, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            CSAddressModel result = handler.getAddressByAddId(DBPath, addressId, openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult userLoveItem(string DBPath, string openId, int itemId)
        {
            CSItemHandler handler = new CSItemHandler();
            bool result = handler.userLoveItem(DBPath, openId, itemId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ifUserLoveItem(string DBPath, string openId, int itemId)
        {
            CSItemHandler handler = new CSItemHandler();
            bool ifloved = handler.ifUserLoveItem(DBPath, openId, itemId);
            return Json(new { result = ifloved }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult userLoveItemCount(string DBPath, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            int loveCount = handler.userLoveItemCount(DBPath, openId);
            return Json(new { result = loveCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getItemViewTimes(string DBPath, int itemId)
        {
            CSItemHandler handler = new CSItemHandler();
            long viewCount = handler.getItemViewTimes(DBPath, itemId);
            return Json(new { result = viewCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUserShopDetailInfo(string DBPath, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<int> getAllStepCount = handler.getEachStepCount(openId, DBPath);
            int loveCount = handler.userLoveItemCount(DBPath, openId);
            int viewCount = handler.getUserViewRecord(openId, DBPath);
            return Json(new { stepCount = getAllStepCount, loveCount = loveCount, viewCount = viewCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getStepOrderList(string openId, string DBPath, int stepId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CartItemFullModel> model = handler.getItemByStepId(DBPath, openId, stepId);
            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public void delOrder(string DBPath, string orderId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.delOrderItems(DBPath, orderId);
        }

        public JsonResult getItemForAdmin(string openId, string DBPath, int stepId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CSItemWithAddress> list = handler.getItemForAdmin(DBPath, openId, stepId);
            return Json(new { result = list }, JsonRequestBehavior.AllowGet);
        }

        public void confirmStep(string DBPath, string orderId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.confirmStep(DBPath, orderId);
        }

        public void addSendMessage(string DBPath, string openId, string msg, int msgType)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.addSendMessage(openId, DBPath, msg, msgType);
        }

        public void succPayOrder(string orderId, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.confirmStep(DBPath, orderId);
            handler.orderAcceptNotification(orderId, DBPath, 0);
            handler.orderAcceptNotification(orderId, DBPath, 4);
        }

        public void failPayOrder(string DBPath, string orderId, string paySign, string packegId, string nonceStr, string timeStamp, double totalAmt)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.orderAcceptNotification(orderId, DBPath, 1);
            handler.saveFailPay(DBPath, orderId, paySign, packegId, nonceStr, timeStamp, totalAmt);
        }

        public void acceptOrder(string orderId, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.orderAcceptNotification(orderId, DBPath, 2);
        }

        public void finishOrder(string orderId, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.orderAcceptNotification(orderId, DBPath, 3);
        }

        public JsonResult getShopCartCount(string openId, string DBPath)
        {
            CSItemHandler handler = new CSItemHandler();
            int count = handler.getShopCartCount(openId, DBPath);
            return Json(new { result = count }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult saveItemChange(string DBPath,string itemName,string itemDesc,double itemPrice,double itemPriceDdt,int stock,int cateId,int itemId,string barcode)
        {
            CSItemHandler handler = new CSItemHandler();
            itemId = handler.saveItemChange(DBPath, itemName, itemDesc, itemPrice, itemPriceDdt, stock, cateId, itemId,barcode);
            return Json(new { result = itemId }, JsonRequestBehavior.AllowGet);
        }

        public void saveItemPics(string DBPath,HttpPostedFileWrapper name,int itemId,int index)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.saveItemPic(DBPath, itemId, name, index);
        }

        public void delImgs(string DBPath,int itemId)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.delImgs(DBPath, itemId);
        }

        public void saveNewCoupon(string DBPath, int userType, int packetPeriod, double baseline, double packetAmt, int canApplyTimes, int packetCount)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.saveNewCoupon(DBPath, userType, packetPeriod, baseline, packetAmt, canApplyTimes, packetCount);
        }

        public JsonResult getUserCoupons(string DBPath,string openId)
        {
            List<UserApplyCouponModel> result = new List<UserApplyCouponModel>();
            result = new CSItemHandler().getUserCoupons(DBPath, openId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ifApplyPacketSucc(string DBPath, string packetId, string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            bool ifSucc = handler.ifApplyPacketSucc(DBPath, packetId, openId);
            return Json(new { result = ifSucc }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUserCoupon(string DBPath, string openId, int searchType)
        {
            CSItemHandler handler = new CSItemHandler();
            var result = handler.getUserCoupon(DBPath, openId, searchType);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCouponCanuse(string openId, string DBPath, double totalAmt)
        {
            CSItemHandler handler = new CSItemHandler();
            List<CouponCanUseModel> result = handler.getCouponCanuse(openId, DBPath, totalAmt);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCouponByCouponId(string DBPath,string couponId)
        {
            CSItemHandler handler = new CSItemHandler();
            CouponCanUseModel result = handler.getCouponByCouponId(DBPath, couponId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getItemsByCate(string DBPath, string openId, int cateId, int from, int size)
        {
            CSItemHandler handler = new CSItemHandler();
            var result = handler.getItemsByCate(DBPath, openId, cateId, from, size);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public void saveFailPay(string DBPath, string orderId, string paySign, string packegId, string nonceStr, string timeStamp, double totalAmt)
        {
            CSItemHandler handler = new CSItemHandler();
            handler.saveFailPay(DBPath, orderId, paySign, packegId, nonceStr, timeStamp, totalAmt); 
        }

        public JsonResult getFailPayByOrderId(string DBPath,string orderId)
        {
            CSItemHandler handler = new CSItemHandler();
            FailPayModel result = handler.getFailPayByOrderId(DBPath, orderId);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public void delBanner(string DBPath)
        {
            new CSItemHandler().delBanner(DBPath);
        }

        public void saveBanner(string DBPath,HttpPostedFileWrapper name,int index)
        {
            new CSItemHandler().saveBanner(DBPath, name.InputStream, index);
        }

        public JsonResult getSearchContent(string DBPath,string openId)
        {
            CSItemHandler handler = new CSItemHandler();
            List<string> selfSearch = handler.getSelfSearch(DBPath, openId);
            List<string> hotSearch = handler.getHotSearch(DBPath);
            return Json(new { selfSearch = selfSearch, hotSearch = hotSearch }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getItemsBySearch(string DBPath, string openId, string search, int from, int size)
        {
            CSItemHandler handler = new CSItemHandler();
            var result = handler.getItemsBySearch(DBPath, openId, search, from, size);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

    }
}