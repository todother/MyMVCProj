using CatsPrj.Model;
using CatsProj.BLL.Handler;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyMVCProj.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public string postWebService(string code)
        {
            UserHandler handler = new UserHandler();
            return handler.postWebService(code);
        }

        public JsonResult wxDecryptData(string sessionKey, string encryptedData, string iv)
        {
            UserHandler handler = new UserHandler();
            bool result = handler.wxDecryptData(sessionKey, encryptedData, iv);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            //string result = AesHelper.AESDecrypt(encryptedData, sessionKey, iv);
            //return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateNickName(string openid, string nickName)
        {
            UserHandler handler = new UserHandler();
            handler.updateNickName(openid, nickName);
            bool isAdmin = handler.isAdmin(openid);
            return Json(new { result = "OK", isAdmin = isAdmin }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUserConfig(string openId)
        {
            UserHandler handler = new UserHandler();
            ConfigModel model = new ConfigModel();
            model = handler.getConfigModel(openId);
            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public void updateUserConfig(string config)
        {
            UserHandler handler = new UserHandler();
            handler.updateUserConfig(config);
        }

        public void addFollow(string openId, string postsId)
        {
            UserHandler handler = new UserHandler();
            handler.addFollow(openId, postsId);
        }

        public void delFollow(string openId, string userId)
        {
            UserHandler handler = new UserHandler();
            handler.delFollow(openId, userId);
        }

        public JsonResult getFollowedUser(string openId, int from, int count)
        {
            UserHandler handler = new UserHandler();
            List<UserModel> users = new List<UserModel>();
            long followCount = handler.getFollowedCount(openId);
            users = handler.getFollowedUser(openId, from, count);
            return Json(new { result = users, followedCount = followCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getFans(string openId, int from, int count)
        {
            UserHandler handler = new UserHandler();
            List<UserModel> users = new List<UserModel>();
            users = handler.getFans(openId, from, count);
            long fansCount = handler.getFansCount(openId);
            return Json(new { result = users, fansCount = fansCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getScoredUser(int count)
        {
            UserHandler handler = new UserHandler();
            List<UserModel> users = handler.getScoreUsers(count);
            return Json(new { result = users }, JsonRequestBehavior.AllowGet);
        }

        public void followUserByUserId(string openId, string userId)
        {
            UserHandler handler = new UserHandler();
            handler.followUserByUserId(openId, userId);
        }

        public void delFollowedUserById(string openId, string userId)
        {
            UserHandler handler = new UserHandler();
            handler.delFollowedUserById(openId, userId);
        }

        public void updateLastRefreshDate(string openId)
        {
            new UserHandler().updateLastRefreshDate(openId);
        }

        public void updateLastRefreshFans(string openId)
        {
            new UserHandler().updateLastRefreshFans(openId);
        }

        public JsonResult getCoverPage()
        {
            UserModel user = new UserModel();
            user = new UserHandler().getCoverUser();
            return Json(new { coverUser = user }, JsonRequestBehavior.AllowGet);
        }

        public void userTransPage(string openId,string pageName)
        {
            UserHandler handler = new UserHandler();
            handler.userTransPage(openId, pageName);
        }

    }
}
