using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
using Cats.DataEntiry;
using System.Data;
using System.Web.Configuration;

namespace CatsProj.DAL
{
    public class AppSecretProvider
    {
        public tbl_password getAppSecret(string demoName)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance("appsecret");
            tbl_password password = db.Queryable<tbl_password>().Where(o => o.demoname == demoName).First();
            return password;
        }
    }
}
