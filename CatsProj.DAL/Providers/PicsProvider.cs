using System;
using CatsProj.DB;
using CatsDataEntity;
using MySql.Data;
using SqlSugar;
using System.Collections.Generic;
namespace CatsProj.DAL.Providers
{
    public class PicsProvider
    {
        public void savePics(tbl_postsPics entity)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			db.Insertable<tbl_postsPics>(entity).ExecuteCommand();
		}
    }
}
