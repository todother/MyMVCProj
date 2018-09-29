using System;
using SqlSugar;
using System.Web.Configuration;
using MySql.Data;
using CatsProj.Tools;
using System.Data;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace CatsProj.DB
{
    public class SqlSugarInstance
    {

        private static string _key = "lovingCats";
        public static SqlSugarClient newInstance()
        {
            Contract.Ensures(Contract.Result<SqlSugarClient>() != null);
            try
            {
                string DBPath = WebConfigurationManager.AppSettings["DBServer"];
                string user = WebConfigurationManager.AppSettings["user"];
                string pwd1 = WebConfigurationManager.AppSettings["Pwd1"];
                string pwd2 = WebConfigurationManager.AppSettings["Pwd2"];
                string aesKey = _key;

                var expMethods = new List<SqlFuncExternal>();
                expMethods.Add(new SqlFuncExternal()
                {
                    UniqueMethodName = "calcDistance",
                    MethodValue = (expInfo, dbType, expContext) =>
                    {
                        if (dbType == SqlSugar.DbType.MySql)
                            return string.Format("(select sqrt( pow({0}-{2},2)+pow({1}-{3},2)))", expInfo.Args[0].MemberName, expInfo.Args[1].MemberName.ToString().Replace("'", ""), expInfo.Args[2].MemberValue, expInfo.Args[3].MemberValue);
                        else
                            throw new Exception("未实现");
                    }
                });

                SqlSugarClient db = new SqlSugarClient(
                    new ConnectionConfig()
                    {
                        ConnectionString = "server=" + AesDecrypt(DBPath) + ";uid=" + AesDecrypt(user) + ";pwd=" + AesDecrypt(pwd1) + AesDecrypt(pwd2) + ";database=cats;Character Set=utf8mb4",//"server="+DBPath+";uid="+user+";pwd="+pwd1+pwd2+";database=cats",
                        DbType = SqlSugar.DbType.MySql,
                        InitKeyType = InitKeyType.Attribute,
                        IsAutoCloseConnection = true,
                        ConfigureExternalServices = new ConfigureExternalServices()
                        {
                            SqlFuncServices = expMethods//set ext method
                        }//初始化主键和自增列信息到ORM的方式
                    });
                var sth = db.Ado.GetDataTable(@"SET NAMES utf8mb4;
set  character_set_client =utf8mb4;
set  character_set_connection=utf8mb4;
set character_set_results =utf8mb4;
set collation_connection=utf8mb4_general_ci;", new List<SugarParameter>() { new SugarParameter("@id", 1) });
                return db;
            }
            catch (Exception e)
            {
                string DBPath = WebConfigurationManager.AppSettings["DBServer"];
                string user = WebConfigurationManager.AppSettings["user"];
                string pwd1 = WebConfigurationManager.AppSettings["Pwd1"];
                string pwd2 = WebConfigurationManager.AppSettings["Pwd2"];
                string aesKey = _key;
                SqlSugarClient db = new SqlSugarClient(
                    new ConnectionConfig()
                    {
                        ConnectionString = "server=" + AesDecrypt(DBPath) + ";uid=" + AesDecrypt(user) + ";pwd=" + AesDecrypt(pwd1) + AesDecrypt(pwd2) + ";database=cats",//"server="+DBPath+";uid="+user+";pwd="+pwd1+pwd2+";database=cats",
                        DbType = SqlSugar.DbType.MySql,
                        InitKeyType = InitKeyType.Attribute //初始化主键和自增列信息到ORM的方式
                    });
                return db;
            }
        }

        private static string AesDecrypt(string data)
        {
            return AesHelper.AESDecrypt(data, _key);
        }

    }
}
