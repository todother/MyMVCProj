using Cats.DataEntiry;
using CatsProj.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsProj.DAL
{
    public class WXPayProvider
    {
        public tbl_wxpay getShopInfo(string dbPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance("wxpay");
            tbl_wxpay shopInfo = db.Queryable<tbl_wxpay>().Where(o => o.dbpath == dbPath).First();
            return shopInfo;
        }
    }
}
