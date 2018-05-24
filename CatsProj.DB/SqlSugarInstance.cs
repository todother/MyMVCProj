using System;
using SqlSugar;
using System.Web.Configuration;
using MySql.Data;

namespace CatsProj.DB
{
    public class SqlSugarInstance
    {
        private static string _key = "lovingCats";
        public static SqlSugarClient newInstance()
        {
            string DBPath = WebConfigurationManager.AppSettings["DBServer"];
            string user = WebConfigurationManager.AppSettings["user"];
            string pwd1 = WebConfigurationManager.AppSettings["Pwd1"];
            string pwd2 = WebConfigurationManager.AppSettings["Pwd2"];
            string aesKey = _key;
            SqlSugarClient db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = "server=127.0.0.1;uid=root;pwd=geyan4024516;database=cats",//"server="+DBPath+";uid="+user+";pwd="+pwd1+pwd2+";database=cats",
                    DbType = DbType.MySql,
                    InitKeyType = InitKeyType.Attribute //初始化主键和自增列信息到ORM的方式
                });
            return db;
        }

    }
}
