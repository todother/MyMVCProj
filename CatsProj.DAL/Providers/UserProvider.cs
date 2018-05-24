using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
namespace CatsProj.DAL.Providers
{
	//
    public class UserProvider
    {
        public tbl_user getUser(string userid)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			tbl_user result=db.Queryable<tbl_user>().Where(o =>o.openid==userid).First();
			return result;
		}

		public void newOrUpdateUser(tbl_user user)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == user.openid).First();
			if (curUser != null)
			{
				if (curUser.avantarUrl != user.avantarUrl || curUser.nickName != user.nickName || curUser.gender != user.gender || curUser.country != user.country || curUser.city != user.city || curUser.province != user.province)
				{
					user.lastLoginDate = DateTime.Now;
					user.registerDate = curUser.registerDate;
					user.userStatus = curUser.userStatus;
					db.Updateable<tbl_user>(user).Where(o=>o.openid==user.openid);
				}
				updateLastLoginDate(user.openid);
			}
			else 
			{             
    			user.registerDate = DateTime.Now;
    			user.userStatus = 0;//0 means the user is under active status
    			user.lastLoginDate = DateTime.Now;


    			db.Insertable<tbl_user>(user).ExecuteCommand();
		    }
		}

        public void updateUserStatus(string userId)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == userId).First();
			curUser.userStatus = 1;//disable the user
			db.Updateable<tbl_user>(curUser);
		}

		public void updateLastLoginDate(string userId)
		{
			try
			{
				SqlSugarClient db = SqlSugarInstance.newInstance();
				tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == userId).First();
				curUser.lastLoginDate = DateTime.Now;//update lastlogin date
				db.Updateable(curUser).Where(o=>o.openid==userId).UpdateColumns(arg => new { arg.lastLoginDate }).ExecuteCommand();
			}
            catch(Exception e)
			{
				int i = 1;
			}
		}

        
    }
}
