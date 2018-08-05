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
    }
}
