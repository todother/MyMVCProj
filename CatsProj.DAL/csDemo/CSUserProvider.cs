using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
using Cats.DataEntiry;
using System.Data;
using System.Web.Configuration;
using Cats.DataEntiry.csdemo;

namespace CatsProj.DAL.csDemo
{
    public class CSUserProvider
    {
        //public static string DBPath = WebConfigurationManager.AppSettings["DBServer"];

        public tbl_user getCSUser(string openId,string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            return user;
        }

        public void newOrUpdateUser(tbl_user user, string DBPath)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
                tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == user.openid).First();
                if (curUser != null)
                {
                    if (curUser.avantarUrl != user.avantarUrl || curUser.nickName != user.nickName || curUser.gender != user.gender || curUser.country != user.country || curUser.city != user.city || curUser.province != user.province)
                    {
                        curUser.lastLoginDate = DateTime.Now;
                        //user.registerDate = curUser.registerDate;
                        //user.userStatus = curUser.userStatus;
                        curUser.avantarUrl = user.avantarUrl;
                        curUser.nickName = user.nickName;
                        curUser.gender = user.gender;
                        curUser.country = user.country;
                        curUser.city = user.city;
                        curUser.province = user.province;
                        db.Updateable<tbl_user>(curUser).Where(o => o.openid == user.openid).ExecuteCommand();
                    }
                    updateLastLoginDate(user.openid,DBPath);
                }
                else
                {
                    //user.referBy = refer;
                    user.registerDate = DateTime.Now;
                    user.lastRefreshDate = DateTime.Now;
                    user.lastRefreshFans = DateTime.Now;
                    user.userStatus = 0;//0 means the user is under active status
                    user.lastLoginDate = DateTime.Now;
                    db.Insertable<tbl_user>(user).ExecuteCommand();
                    
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
        }

        public tbl_user getUserInfo(string DBPath,string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            return user;
        }

        public void updateLastLoginDate(string userId,string DBPath)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
                tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == userId).First();
                curUser.lastLoginDate = DateTime.Now;//update lastlogin date
                db.Updateable(curUser).Where(o => o.openid == userId).UpdateColumns(arg => new { arg.lastLoginDate }).ExecuteCommand();
            }
            catch (Exception e)
            {
                int i = 1;
            }
        }


        public List<tbl_banner> getBanner(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_banner> banner = db.Queryable<tbl_banner>().OrderBy(o=>o.picIdx,OrderByType.Desc).ToList();
            return banner;
        }

        public void updateNickName(string openid, string nickName,string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_user cur = db.Queryable<tbl_user>().Where(o => o.openid == openid).First();
            cur.nickName = nickName;
            db.Updateable(cur).Where(o => o.openid == openid).UpdateColumns(arg => new { arg.nickName }).ExecuteCommand();
        }

    }
}
