using Cats.DataEntiry;
using Cats.DataEntiry.csdemo;
using CatsDataEntity;
using CatsPrj.Model;
using CatsPrj.Model.csDemo;
using CatsProj.DAL;
using CatsProj.DAL.csDemo;
using CatsProj.DAL.Providers;
using EntityModelConverter;
using EntityModelConverter.csDemo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace CatsProj.BLL.csDemo
{
    public class CSItemHandler
    {
        public List<CSCateModel> getCates(string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_csCate> cates = provider.getCates(DBPath);
            List<CSCateModel> models = new List<CSCateModel>();
            foreach(var item in cates)
            {
                var sth = new List<CSItemModel>();
                models.Add(CSCateConverter.csCateEntityToModel(item, sth));
            }
            if (models.Count > 8)
            {
                models.RemoveRange(8, models.Count - 8);
            }
            return models;
        }

        public List<CSCateModel> getAllCates(string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_csCate> cates = provider.getCates(DBPath);
            List<CSCateModel> models = new List<CSCateModel>();

            foreach (var item in cates)
            {
                var sth = new List<CSItemModel>();
                models.Add(CSCateConverter.csCateEntityToModel(item, sth));
            }
            return models;
        }

        public List<CSCateModel> getAllItems(string DBPath,string openId,int from,int size)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_csCate> cates = provider.getCates(DBPath,from, size);
            List<CSCateModel> cateModels = new List<CSCateModel>();
            foreach (var item in cates)
            {
                List<ItemWithSelCount> csitems = provider.getItemsByType(item.cateId,DBPath,openId,10);
                List<CSItemModel> itemModel = new List<CSItemModel>();
                foreach(var sth in csitems)
                {
                    List<tbl_csItemPics> pics = new List<tbl_csItemPics>();
                    
                    pics = provider.getItemPicsByItemId(sth.itemId, DBPath);
                    itemModel.Add(CSItemConverter.csItemEntityToModel(sth, pics));
                    
                }
                cateModels.Add(CSCateConverter.csCateEntityToModel(item, itemModel));
            }
            return cateModels;
        }

        public List<CSItemModel> getItemsByCate(string DBPath,string openId,int cateId,int from,int size)
        {
            CSItemProvider provider = new CSItemProvider();
            List<ItemWithSelCount> csitems = provider.getItemsByTypePageList(cateId, DBPath, openId,from,size);
            CSCateModel model = new CSCateModel();
            List<CSItemModel> itemModel = new List<CSItemModel>();
            foreach (var sth in csitems)
            {
                List<tbl_csItemPics> pics = new List<tbl_csItemPics>();

                pics = provider.getItemPicsByItemId(sth.itemId, DBPath);
                itemModel.Add(CSItemConverter.csItemEntityToModel(sth, pics));
            }

            return itemModel;
        }


        public List<CSItemModel> getItemsBySearch(string DBPath, string openId, string search, int from, int size)
        {
            CSItemProvider provider = new CSItemProvider();
            List<ItemWithSelCount> csitems = provider.getItemsBySearch(search, DBPath, openId, from, size);
            CSCateModel model = new CSCateModel();
            List<CSItemModel> itemModel = new List<CSItemModel>();
            foreach (var sth in csitems)
            {
                List<tbl_csItemPics> pics = new List<tbl_csItemPics>();

                pics = provider.getItemPicsByItemId(sth.itemId, DBPath);
                itemModel.Add(CSItemConverter.csItemEntityToModel(sth, pics));
            }

            return itemModel;
        }

        public List<string> getSelfSearch(string DBPath, string openId)
        {
            CSItemProvider provider = new CSItemProvider();
            return provider.getSelfSearch(DBPath,openId);
        }


        public List<string> getHotSearch(string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            return provider.getHotSearch(DBPath);
        }
        

        public CSItemModel getItemDetail(int itemId,string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            tbl_csItem item = provider.getItemDetail(itemId, DBPath);
            List<tbl_csItemPics> pics = new List<tbl_csItemPics>();

            pics = provider.getItemPicsByItemId(itemId, DBPath);
            CSItemModel model = new CSItemModel();
            model = CSItemConverter.csItemEntityToModel(item, pics);
            return model;

        }

        public int getItemTotalSold(string DBPath,int itemId)
        {
            return new CSItemProvider().getItemTotalsold(DBPath, itemId);
        }

        public CSItemModel getItemDetailByBarcode(string barcode,string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            int itemId = provider.getItemIdByBarcode(barcode, DBPath);
            return getItemDetail(itemId, DBPath);
        }

        public int getItemIdByBarcode(string barcode, string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            int itemId = provider.getItemIdByBarcode(barcode, DBPath);
            return itemId;
        }

        public List<CSPicModel> getBanners(string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_banner> banners = provider.getBanners(DBPath);
            List<CSPicModel> result = new List<CSPicModel>();
            foreach(var item in banners)
            {
                CSPicModel model = new CSPicModel();
                model.picName = item.picName;
                model.picId = item.bannerId;
                model.picIdx = item.picIdx;
                result.Add(model);
            }
            return result;
        }

        public string postWebService(string DBPath,string code)
        {
            AppSecretProvider provider = new AppSecretProvider();
            tbl_password password = provider.getAppSecret(DBPath);

            string appid = password.appid;
            string secret = password.appsecret;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            return retXml;

        }


        public bool wxDecryptData(string sessionKey, string encryptedData, string iv, string refer,string DBPath)
        {
            byte[] encryptedDataToByte = Convert.FromBase64String(encryptedData);
            byte[] aesKey = Convert.FromBase64String(sessionKey);
            byte[] aesIV = Convert.FromBase64String(iv);
            byte[] aesCiper = Convert.FromBase64String(encryptedData);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Key = Convert.FromBase64String(sessionKey); // Encoding.UTF8.GetBytes(AesKey);
            rijndaelCipher.IV = Convert.FromBase64String(iv);// Encoding.UTF8.GetBytes(AesIV);
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedDataToByte, 0, encryptedDataToByte.Length);
            string result = Encoding.UTF8.GetString(plainText);
            bool convertresult = userLogin(result, refer, DBPath);
            return convertresult;// check if the userstatus is forbidened
        }

        public bool userLogin(string data, string refer,string DBPath)
        {
            try
            {

                UserModel user = JsonConvert.DeserializeObject<UserModel>(parseInvalid(data));

                CSUserProvider provider = new CSUserProvider();
                provider.newOrUpdateUser(UserConverter.userModelToEntity(user), DBPath);
                tbl_user tbl_User = provider.getUserInfo(DBPath,user.openId);
                if (tbl_User.userStatus == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string parseInvalid(string data)
        {
            string[] list = data.Split(',');
            string needtoReplace = "";
            foreach (var item in list)
            {
                if (item.Contains("nickName"))
                {
                    needtoReplace = item;
                }
            }
            string result = data.Replace(needtoReplace, "\"nickName\":\"\"");
            return result;
        }

        public bool updateNickName(string openid, string nickName,string DBPath)
        {
            CSUserProvider provider = new CSUserProvider(); 
            provider.updateNickName(openid, nickName,DBPath);
            return provider.ifAdmin(DBPath, openid);
        }

        public string addCart(string DBPath,string openId,int buyCount,int itemId)
        {
            CSItemProvider provider = new CSItemProvider();

            string cartId = provider.addShopCart(DBPath, openId, itemId, buyCount);
            return cartId;
        }

        public void delCart(string cartId,string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.delShopCart(DBPath, cartId);
        }
        public List<ShopCartItemModel> getShopCartUnPay(string DBPath,string openId)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_cartItem> shopCarts = provider.getCartItems(DBPath, openId);
            List<ShopCartItemModel> models = new List<ShopCartItemModel>();
            foreach(var item in shopCarts)
            {
                models.Add(ShopCartConverter.shopCartEntityToModel(item));
            }
            return models;
        }

        public void addCartBy1(string DBPath, int itemId, string openId, int buycount)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.addItemBy1(DBPath, itemId, openId, buycount);
        }

        public void addItemBy1(string DBPath,string cartId)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.addItem1(DBPath, cartId);
        }

        public void delItemBy1(string DBPath,string cartId)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.delItem1(DBPath, cartId);
        }

        public string addOrderList(string DBPath,string cartList,string addressId)
        {
            CSItemProvider provider = new CSItemProvider();
            string orderId = provider.addOrderList(cartList, DBPath, addressId);
            
            return orderId;
        }

        public List<CSAddressModel> getAddressList(string DBPath,string openId)
        {
            List<tbl_receiveAddress> address = new List<tbl_receiveAddress>();
            CSItemProvider provider = new CSItemProvider();
            address = provider.getAddressByUser(openId, DBPath);
            List<CSAddressModel> result = new List<CSAddressModel>();
            foreach(var item in address)
            {
                result.Add(CSAddressConverter.convertAddressToModel(item));
            }
            return result;
        }

        public string addAddress(string DBPath, string openId, string receiver, string telNo, string address, bool ifDefault,string addId)
        {
            CSItemProvider provider = new CSItemProvider();
            string resultId=provider.addReceiveAddress(receiver, address, openId, telNo, DBPath, ifDefault,addId);
            return resultId;
        }

        public CSAddressModel getAddressByAddId(string DBPath,string addressId,string openId)
        {
            CSItemProvider provider = new CSItemProvider();
            tbl_receiveAddress address = provider.getAddressByAddId(DBPath, addressId,openId);
            CSAddressModel model = CSAddressConverter.convertAddressToModel(address);
            return model;
        }

        public bool userLoveItem(string DBPath, string openId, int itemId)
        {
            CSItemProvider provider = new CSItemProvider();
            bool result = provider.userLoveItem(DBPath, openId, itemId);
            return result;
        }

        public bool ifUserLoveItem(string DBPath, string openId, int itemId)
        {
            CSItemProvider provider = new CSItemProvider();
            bool result = provider.ifUserLoveItem(DBPath, openId, itemId);
            return result;
        }

        public int userLoveItemCount(string DBPath, string openId)
        {
            CSItemProvider provider = new CSItemProvider();
            int result = provider.userLoveItemCount(DBPath, openId);
            return result;
        }

        public long getItemViewTimes(string DBPath, int itemId)
        {
            CSItemProvider provider = new CSItemProvider();
            long result = provider.getItemViewTimes(DBPath, itemId);
            return result;
        }

        public List<int> getEachStepCount(string openId, string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            List<int> result = provider.getEachStepCount(openId, DBPath);
            return result;
        }

        public List<CartItemFullModel> getItemByStepId(string DBPath,string openId,int stepId)
        {
            List<tbl_cartItemFull> cartItems = new List<tbl_cartItemFull>();
            CSItemProvider provider = new CSItemProvider();
            cartItems = provider.getCartItemByStepId(DBPath, openId, stepId);
            List<CartItemFullModel> model = ShopCartConverter.convertShopcartsToFullList(cartItems);
            return model;
        }

        public List<CSItemWithAddress> getItemForAdmin(string DBPath, string openId, int stepId)
        {
            List<tbl_itemFullWithAddress> cartItems = new List<tbl_itemFullWithAddress>();
            CSItemProvider provider = new CSItemProvider();
            cartItems = provider.getCartItemByStepIdForAdmin(DBPath, openId, stepId);
            List<CSItemWithAddress> model = ShopCartConverter.convertShopcartsToAddressList(cartItems);
            return model;
        }

        public int getUserViewRecord(string openId,string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            int viewTimes = provider.getUserViewRecord(DBPath, openId);
            return viewTimes;
        }

        public void userViewItem(string DBPath,string openId,int itemID)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.userViewItem(DBPath, openId, itemID);
        }

        public void delOrderItems(string DBPath,string orderId)
        {

            CSItemProvider provider = new CSItemProvider();
            provider.delOrderItems(DBPath, orderId);
        }

        public void confirmStep(string DBPath,string orderId)
        {
            CSItemProvider provide = new CSItemProvider();
            provide.confirmStep(DBPath, orderId);
            int curStep = provide.getCurrStep(orderId, DBPath);
            switch (curStep)
            {
                case 2:
                    orderAcceptNotification(orderId, DBPath, 2);
                    break;
                case 3:
                    orderAcceptNotification(orderId, DBPath, 3);
                    break;
            }

        }
        public string findSendMsgId(string openId,string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_sendmessage> messages = provider.getMessageIds(openId, DBPath);
            string messageId = string.Empty;
            foreach(var item in messages)
            {
                if((item.idtype==0 && item.usedTimes==0)||(item.idtype==1 && item.usedTimes < 3))
                {
                    messageId = item.usedId;
                    break;
                }
            }

            return messageId;
        }

        public void addSendMessage(string openId,string DBPath,string msg,int msgType)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.addNewMessageId(openId, msg, msgType, DBPath);
        }

        public int getShopCartCount(string openId,string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            int count = provider.getShopCartCount(openId, DBPath);
            return count;
        }

        public void orderAcceptNotification(string orderId,string DBPath,int tempType)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_cartItemFull> items = provider.getItemsByOrderId(orderId, DBPath);
            string itemNames = string.Empty;
            double totalPrice = 0;
            string fullInfo = string.Empty;
            foreach(var item in items)
            {
                itemNames += item.itemName + "/";
                fullInfo += item.itemName + "*" + item.buyCount.ToString() + "/";
                totalPrice += Math.Round(item.itemPriceDdt * item.buyCount, 2);
            }
            if (itemNames.Length > 20)
            {
                itemNames = itemNames.Substring(0, 20) + "...";
            }

            tbl_userpacket packet = provider.getCouponByOrderId(DBPath, orderId);
            if (packet != null)
            {
                totalPrice = (totalPrice - packet.packetAmt)<=0?0.01: (totalPrice - packet.packetAmt);
            }

            MsgNewOrder model = new MsgNewOrder();
            model.form_id = findSendMsgId(items[0].openId, DBPath);
            model.page = "pages/homepage/homepage";
            //model.template_id = "7cVMG_IHpcuNKc-2yQusMdZpG4AMucRNT8tPxVy9ZOQ";
            model.touser = items[0].openId;
            model.data = new Dictionary<string, KVpari>();
            KVpari keyword1 = new KVpari();
            KVpari keyword2 = new KVpari();
            KVpari keyword3 = new KVpari();
            KVpari keyword4 = new KVpari();
            KVpari keyword5 = new KVpari();

            if (tempType == 0)
            {
                keyword1.value = items[0].updateTime.ToString("MM-dd HH:mm");
                keyword2.value = fullInfo.Substring(0, fullInfo.Length - 1);
                keyword3.value = items[0].userName;
                model.data.Add("keyword1", keyword1);
                model.data.Add("keyword2", keyword2);
                model.data.Add("keyword3", keyword3);
                model.template_id = provider.getMsgTemp(DBPath, tempType).tempId;
            }
            else if (tempType == 1)
            {
                keyword1.value= items[0].updateTime.ToString("MM-dd HH:mm");
                keyword2.value= fullInfo.Substring(0, fullInfo.Length - 1);
                keyword3.value = totalPrice.ToString();
                keyword4.value = "您的订单尚未成功支付，如果需要请在2小时内付款，否则将被删除。";
                model.data.Add("keyword1", keyword1);
                model.data.Add("keyword2", keyword2);
                model.data.Add("keyword3", keyword3);
                model.data.Add("keyword4", keyword4);
                model.template_id = provider.getMsgTemp(DBPath, tempType).tempId;
            }
            else if (tempType == 2)
            {
                keyword1.value = items[0].updateTime.ToString("MM-dd HH:mm");
                keyword2.value = fullInfo.Substring(0, fullInfo.Length - 1);
                keyword3.value = "您的订单已经发货，即将送达。";
                model.data.Add("keyword1", keyword1);
                model.data.Add("keyword2", keyword2);
                model.data.Add("keyword3", keyword3);
                model.template_id = provider.getMsgTemp(DBPath, tempType).tempId;
            }

            else if (tempType == 3  )
            {
                keyword1.value = "您的订单已完成";
                keyword2.value = totalPrice.ToString();
                keyword3.value = fullInfo.Substring(0, fullInfo.Length - 1);
                keyword4.value = "您的订单已送达，若有疑问，请于小程序内联系店家，谢谢。";
                model.data.Add("keyword1", keyword1);
                model.data.Add("keyword2", keyword2);
                model.data.Add("keyword3", keyword3);
                model.data.Add("keyword4", keyword4);
                model.template_id = provider.getMsgTemp(DBPath, tempType).tempId;
            }

            else if (tempType == 4)
            {
                keyword1.value = items[0].userName;
                keyword2.value = totalPrice.ToString();
                keyword3.value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                keyword4.value = fullInfo.Substring(0, fullInfo.Length - 1);
                
                model.data.Add("keyword1", keyword1);
                model.data.Add("keyword2", keyword2);
                model.data.Add("keyword3", keyword3);
                model.data.Add("keyword4", keyword4);
                model.template_id = provider.getMsgTemp(DBPath, tempType).tempId;
                List<string> admins = provider.getAdmins(DBPath);
                foreach(var admin in admins)
                {
                    model.form_id = findSendMsgId(admin, DBPath);
                    model.touser = admin;
                    JavaScriptSerializer jssa = new JavaScriptSerializer();
                    string myJsona = jssa.Serialize(model);

                    string tokena = new TokenProvider().getToken(DBPath);
                    provider.updateUsedTimes(model.form_id, DBPath);
                    var httpWebRequesta = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + tokena);
                    httpWebRequesta.ContentType = "application/json";
                    httpWebRequesta.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequesta.GetRequestStream()))
                    {
                        streamWriter.Write(myJsona);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var httpResponsea = (HttpWebResponse)httpWebRequesta.GetResponse();
                    using (var streamReader = new StreamReader(httpResponsea.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        //return result;
                    }

                }
                return;
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string myJson = jss.Serialize(model);

            string token = new TokenProvider().getToken(DBPath);
            provider.updateUsedTimes(model.form_id, DBPath);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(myJson);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //return result;
            }
        }

        public int saveItemChange(string DBPath, string itemName, string itemDesc, double itemPrice, double itemPriceDdt, int stock, int cateId, int itemId,string barcode)
        {
            CSItemProvider provider = new CSItemProvider();
            itemId = provider.saveItemChange(DBPath, itemName, itemDesc, itemPrice, itemPriceDdt, stock, cateId, itemId,barcode);
            return itemId;
        }
        public void saveItemPic(string DBPath,int itemId, HttpPostedFileWrapper file,int idx)
        {

            string root = WebConfigurationManager.AppSettings["rootFolder"];
            Stream picStream = file.InputStream;
            Image picImg = Image.FromStream(picStream);
            string imgId = new CSItemProvider().saveItemPic(DBPath, idx, itemId);
            if(!Directory.Exists(root+"/" + DBPath + "/itemPics/"))
            {
                Directory.CreateDirectory(root+"/" + DBPath + "/itemPics/");
            }
            string picPath = root+"/" + DBPath + "/itemPics/" + imgId + ".jpg";
            picImg.Save(picPath, ImageFormat.Jpeg);
            
            picImg.Dispose();
            picStream.Close();
            picStream.Dispose();
        }

        public void delImgs(string DBPath,int itemId)
        {
            string root = WebConfigurationManager.AppSettings["rootFolder"];
            List<string> imgs = new CSItemProvider().delPicImgs(DBPath, itemId);
            foreach(var item in imgs)
            {
                if(File.Exists(root+"/" + DBPath + "/itemPics/" + item + ".jpg"))
                {
                    File.Delete(root+"/" + DBPath + "/itemPics/" + item + ".jpg");
                }
            }
        }

        public void saveNewCoupon(string DBPath, int userType, int packetPeriod, double baseline, double packetAmt, int canApplyTimes, int packetCount)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.saveNewCoupon(DBPath, userType, packetPeriod, baseline, packetAmt, canApplyTimes, packetCount);
        }

        public List<UserApplyCouponModel> getUserCoupons(string DBPath,string openId)
        {
            List<UserApplyCoupon> coupons = new CSItemProvider().getCouponForUser(DBPath, openId);
            List<UserApplyCouponModel> result = new List<UserApplyCouponModel>();
            foreach(var item in coupons)
            {
                result.Add(CouponConverter.convertApplyEntityToModel(item));
            }
            return result;
        }

        public bool ifApplyPacketSucc(string DBPath, string packetId, string openId)
        {
            CSItemProvider handler = new CSItemProvider();
            bool ifSucc = handler.ifApplyPacketSucc(DBPath, packetId, openId);
            return ifSucc;
        }

        public List<UserCouponModel> getUserCoupon(string DBPath,string openId,int searchType)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_userpacket> packages = provider.getUserPacket(DBPath, openId, searchType);
            List<UserCouponModel> result = new List<UserCouponModel>();
            foreach(var item in packages)
            {
                result.Add(CouponConverter.convertCouponToModel(item));
            }
            return result;
        }

        public List<CouponCanUseModel> getCouponCanuse(string openId, string DBPath, double totalAmt)
        {
            CSItemProvider provider = new CSItemProvider();
            List<CouponCanUse> entities = provider.getCouponCanuse(openId, DBPath, totalAmt);
            List<CouponCanUseModel> result = new List<CouponCanUseModel>();
            foreach(var item in entities)
            {
                result.Add(CouponConverter.converCanuseToModel(item));
            }
            return result;
        }

        public CouponCanUseModel getCouponByCouponId(string DBPath, string couponId)
        {
            CSItemProvider provider = new CSItemProvider();
            CouponCanUse entity = provider.getCouponByCouponId(DBPath, couponId);
            CouponCanUseModel result = CouponConverter.converCanuseToModel(entity);
            return result;
        }

        public void saveFailPay(string DBPath, string orderId, string paySign,string packegId,string nonceStr,string timeStamp, double totalAmt)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.saveFailPay(DBPath, orderId,paySign,packegId,nonceStr,timeStamp,totalAmt);
        }

        public FailPayModel getFailPayByOrderId(string DBPath,string orderId)
        {
            CSItemProvider provider = new CSItemProvider();
            tbl_failpay failpay = provider.getFailPayByOrderId(DBPath, orderId);
            FailPayModel result = FailPayConverter.convertFailyPayToModel(failpay);
            return result;
        }

        public void saveBanner(string DBPath,Stream stream,int index)
        {
            string root = WebConfigurationManager.AppSettings["rootFolder"];
            Stream picStream = stream;
            Image picImg = Image.FromStream(picStream);
            string picName = Guid.NewGuid().ToString();
            new CSItemProvider().saveBanner(DBPath, picName, index);
            if (!Directory.Exists(root + "/" + DBPath + "/banner/"))
            {
                Directory.CreateDirectory(root + "/" + DBPath + "/banner/");
            }
            string picPath = root + "/" + DBPath + "/banner/" + picName + ".jpg";
            picImg.Save(picPath, ImageFormat.Jpeg);

            picImg.Dispose();
            picStream.Close();
            picStream.Dispose();
        }
        public void delBanner(string DBPath)
        {
            string root = WebConfigurationManager.AppSettings["rootFolder"];

            List<string> imgs = new CSItemProvider().getBanners(DBPath).Select(o=>o.picName).ToList();
            foreach (var item in imgs)
            {
                if (File.Exists(root + "/" + DBPath + "/banner/" + item + ".jpg"))
                {
                    File.Delete(root + "/" + DBPath + "/banner/" + item + ".jpg");
                }
            }
            new CSItemProvider().delBanner(DBPath);
        }
    }
}
