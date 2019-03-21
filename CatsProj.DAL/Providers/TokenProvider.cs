using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
using Cats.DataEntiry;
using System.Data;

namespace CatsProj.DAL.Providers
{
    public class TokenProvider
    {
        public void updateToken(string token)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Deleteable<tbl_token>().ExecuteCommand();
            db.Insertable<tbl_token>(new tbl_token { accessToken = token, generateTime = DateTime.Now }).ExecuteCommand();
        }

        public string getToken()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            string token = db.Queryable<tbl_token>().First().accessToken;
            return token;
        }
        public void updateCSToken(string demoName,string token)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance("appsecret");
            tbl_password password = db.Queryable<tbl_password>().Where(o => o.demoname == demoName).First();
            password.token = token;
            db.Updateable<tbl_password>(password).Where(o => o.demoname == demoName).ExecuteCommand();
        }

        public List<tbl_password> getTokens()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance("appsecret");
            List<tbl_password> passwords = new List<tbl_password>();
            passwords = db.Queryable<tbl_password>().ToList();
            return passwords;
        }

        public string getToken(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance("appsecret");
            string token = db.Queryable<tbl_password>().Where(o => o.demoname == DBPath).First().token;
            return token;
        }
    }
}
