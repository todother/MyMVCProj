using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;

using System.Web.Script.Serialization;
using MySql.Data;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using CatsProj.Tools;
using System.Text;
using CatsProj.BLL.Handler;

namespace MyMVCProj.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getUser(string testUser)
        {

            //tbl_user oMycustomclassname = JsonConvert.DeserializeObject<tbl_user>(testUser);
            //Type t = Type.GetType("tbl_user");
            try
            {

                SqlSugarClient db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = "server=127.0.0.1;uid=root;pwd=geyan4024516;database=cats",
                    DbType = DbType.MySql,
                    InitKeyType = InitKeyType.Attribute //初始化主键和自增列信息到ORM的方式
                 });

                //List<tbl_user> result = db.Queryable<tbl_user>().ToList();
                /*tbl_user user = new tbl_user();
                user.userName = "GY";
                user.userId = 102;
                db.Insertable(user).ExecuteCommand();*/
                return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult generateAppid(string content)
        {
            string textContent = content;
            Guid id = Guid.NewGuid();
            return Json(new { result = "123" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult savePic(HttpPostedFileWrapper name, string appid)
        {
            try
            {
                //byte[] byt = System.Text.Encoding.Default.GetBytes(name.);
                //MemoryStream stm = new MemoryStream(byt);
                //Image img = Image.FromStream(stm);
                
                //stm.Close();
                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e }, JsonRequestBehavior.AllowGet);
            }
        }

        public string postWebService(string code)
        {
			UserHandler handler = new UserHandler();
            return handler.postWebService(code);
        }

        public JsonResult wxDecryptData(string sessionKey, string encryptedData, string iv)
        {
			UserHandler handler = new UserHandler();
			handler.wxDecryptData(sessionKey, encryptedData, iv);
			return Json(new { result = "OK" },JsonRequestBehavior.AllowGet);
            //string result = AesHelper.AESDecrypt(encryptedData, sessionKey, iv);
            //return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }


}
