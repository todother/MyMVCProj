using System;
using CatsProj.DB;
using CatsDataEntity;
using MySql.Data;
using SqlSugar;
using System.Collections.Generic;
using Cats.DataEntiry;

namespace CatsProj.DAL.Providers
{
    public class PicsProvider
    {
        public void savePics(tbl_postspics entity)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			db.Insertable<tbl_postspics>(entity).ExecuteCommand();
		}

        public List<tbl_postspics> getPics()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_postspics> pics = new List<tbl_postspics>();
            pics = db.Queryable<tbl_postspics>().Where(o => o.picIndex == 0 && (o.picsRate == 0 )).ToList();
            return pics;
        }

        public void updateRate(string picsId,decimal rate)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_postspics pic = new tbl_postspics();
            pic = db.Queryable<tbl_postspics>().Where(o => o.picID == picsId).First();
            pic.picsRate = rate;
            db.Updateable<tbl_postspics>(pic).Where(o => o.picID == picsId).UpdateColumns(arg => new { arg.picsRate }).ExecuteCommand();
        }

        public List<tbl_picEffect>  getEffects()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_picEffect> effects = new List<tbl_picEffect>();
            effects = db.Queryable<tbl_picEffect>().OrderBy(o=>o.effectIndex).ToList();
            return effects;
        }
    }
}
