using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using Cats.DataEntiry;
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
            string appid = WebConfigurationManager.AppSettings["APPID"];
            string secret = WebConfigurationManager.AppSettings["Secret"];
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

		public bool wxDecryptData(string sessionKey,string encryptedData,string iv)
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
			bool convertresult= userLogin(result);
			return convertresult;// check if the userstatus is forbidened
		}

		public bool userLogin(string data)
		{
			try
			{
				
				UserModel user = JsonConvert.DeserializeObject<UserModel>(parseInvalid(data));
				UserProvider provider = new UserProvider();
				provider.newOrUpdateUser(UserConverter.userModelToEntity(user));
				tbl_user tbl_User = provider.getUser(user.openId);
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
            foreach(var item in list)
			{
				if(item.Contains("nickName"))
				{
					needtoReplace = item;
				}
			}
			string result = data.Replace(needtoReplace, "\"nickName\":\"\"");
			return result;
		}

        public void updateNickName(string openid,string nickName)
		{
			UserProvider provider = new UserProvider();
			provider.updateNickName(openid, nickName);
		}

		public UserModel getUserInfo(string openId)
		{
			UserProvider provider = new UserProvider();
			UserModel model = UserConverter.userEntityToModel(provider.getUserInfo(openId));
			return model;
		}

        public UserModel getCoverUser()
        {
            UserProvider provider = new UserProvider();
            UserModel model = UserConverter.userEntityToModel(provider.getCoverPageUser());
            return model;
        }

        public ConfigModel getConfigModel(string openId)
		{
			UserProvider provider = new UserProvider();
			ConfigModel model = ConfigConverter.configEntityToModel(provider.getUserConfig(openId));
			return model;
		}

		public void updateUserConfig(string model)
		{
			ConfigModel user = JsonConvert.DeserializeObject<ConfigModel>(model);
			tbl_userConfig config = ConfigConverter.configModelToEntity(user);
			UserProvider provider = new UserProvider();
			provider.updateUserConfig(config);
		}

        public void addFollow(string openId,string postsId)
		{
			UserProvider provider = new UserProvider();
			provider.addFollow(openId, postsId);
		}

		public void delFollow(string openId,string userId)
		{
			UserProvider provider = new UserProvider();
			provider.delFollow(openId, userId);
		}

        public bool ifFollowed(string openId,string postsId)
		{
			UserProvider provider = new UserProvider();
			return provider.ifFollowed(postsId, openId);
		}

        public List<UserModel> getFollowedUser(string openId,int from,int count)
		{
			UserProvider provider = new UserProvider();
			List<tbl_user> userlist = provider.getFollowedUsers(openId, from, count);
			List<UserModel> result = new List<UserModel>();
            foreach(var item in userlist)
			{
				result.Add(UserConverter.userEntityToModel(item));
			}
			return result;
		}

		public List<UserModel> getFans(string openId, int from, int count)
        {
            UserProvider provider = new UserProvider();
			List<tbl_user> userlist = provider.getFans(openId, from, count);
            List<UserModel> result = new List<UserModel>();
            foreach (var item in userlist)
            {
                result.Add(UserConverter.userEntityToModel(item));
            }
            return result;
        }

        public long getFollowedCount(string openId)
		{
			return new UserProvider().getFollowCount(openId);
		}

		public long getFansCount(string openid)
		{
			return new UserProvider().getFansCount(openid);
		}

        public string transferFansCountToString(long fansCount)
        {
            string result = string.Empty;
            result = string.Format("{0:N}", fansCount);
            var splitNumber = result.Split('.');
            if (splitNumber[1] == "00")
            {
                return splitNumber[0];
            }
            else
            {
                return result;
            }
        }

        public List<UserModel> getScoreUsers(int count)
		{
			UserProvider provider = new UserProvider();
			List<tbl_user> users = provider.getUserScoreCard();
            if(count>users.Count)
			{
				count = users.Count;
			}
			users = GetRandomList<tbl_user>(users);
			users.RemoveRange(count, users.Count-count);
			List<UserModel> models = new List<UserModel>();
            foreach(var item in users)
			{
				models.Add(UserConverter.userEntityToModel(item));
			}
			return models;

		}

		public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }

		public bool ifUserFollowedByOpenId(string openId,string userId)
		{
			return new UserProvider().ifUserFollowedByOpenId(openId, userId);
		}

		public void followUserByUserId(string openId,string userId)
		{
			new UserProvider().followUserByUserId(openId, userId);
		}

		public void delFollowedUserById(string openId,string userId)
		{
			new UserProvider().delFollowedUserById(openId, userId);
		}

        public bool isAdmin(string openId)
		{
			return new UserProvider().isAdmin(openId);
		}

        public void updateLastRefreshDate(string openId)
		{
			new UserProvider().updateLastRefreshDate(openId);
		}

        public long getNewFansCount(string openId)
		{
			return new UserProvider().newFansCount(openId);
		}

		public void updateLastRefreshFans(string openId)
		{
			new UserProvider().updateLastRefreshFans(openId);
		}

        public void userTransPage(string openId,string pageName)
        {
            UserProvider provider = new UserProvider();
            provider.userTransPage(openId, pageName);
        }

        public void saveFormSubmit(string openId,string formId)
        {
            new UserProvider().saveFormSubmit(openId, formId);
        }

        public int needToShowMask(string openId)
        {
            return new UserProvider().needToShowMask(openId);
        }

        public long getLovedCount(string openId)
        {
            return new UserProvider().getLovedCount(openId);
        }

        public void updateSelfIntro(string openId,string selfIntro)
        {
            new UserProvider().updateSelfIntro(openId, selfIntro);
        }
    }
}
