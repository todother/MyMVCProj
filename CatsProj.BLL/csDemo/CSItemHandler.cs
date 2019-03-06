using Cats.DataEntiry;
using Cats.DataEntiry.csdemo;
using CatsDataEntity;
using CatsPrj.Model;
using CatsPrj.Model.csDemo;
using CatsProj.DAL;
using CatsProj.DAL.csDemo;
using EntityModelConverter;
using EntityModelConverter.csDemo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public List<CSCateModel> getAllItems(string DBPath)
        {
            CSItemProvider provider = new CSItemProvider();
            List<tbl_csCate> cates = provider.getCates(DBPath);
            List<CSCateModel> cateModels = new List<CSCateModel>();
            foreach (var item in cates)
            {
                List<tbl_csItem> csitems = provider.getItemsByType(item.cateId,DBPath);
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

        public void updateNickName(string openid, string nickName,string DBPath)
        {
            CSUserProvider provider = new CSUserProvider(); 
            provider.updateNickName(openid, nickName,DBPath);
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

        public string addOrderList(string DBPath,string cartList)
        {
            CSItemProvider provider = new CSItemProvider();
            string orderId = provider.addOrderList(cartList, DBPath);
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

        public void addAddress(string DBPath, string openId, string receiver, string telNo, string address, bool ifDefault)
        {
            CSItemProvider provider = new CSItemProvider();
            provider.addReceiveAddress(receiver, address, openId, telNo, DBPath, ifDefault);
        }
    }
}
