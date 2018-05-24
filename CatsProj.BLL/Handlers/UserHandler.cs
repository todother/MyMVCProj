using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using CatsDataEntity;
using CatsPrj.Model;
using CatsProj.DAL;
using CatsProj.DAL.Providers;
using EntityModelConverter;
using Newtonsoft.Json;


namespace CatsProj.BLL.Handler
{
    public class UserHandler
    {
        
		public string postWebService(string code)
        {
            string appid = "wx279f067da507d202";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + "12f53ae598e2dedea19849baa602f6cd" + "&js_code=" + code + "&grant_type=authorization_code");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            return retXml;
        }

		public void wxDecryptData(string sessionKey,string encryptedData,string iv)
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
            string result = Encoding.Default.GetString(plainText);
			userLogin(result);
		}

		public void userLogin(string data)
		{
			UserModel user=JsonConvert.DeserializeObject<UserModel>(data);
			UserProvider provider = new UserProvider();
			provider.newOrUpdateUser(UserConverter.userModelToEntity(user));
		}

    }
}
