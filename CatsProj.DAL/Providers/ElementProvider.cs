using Cats.DataEntiry;
using CatsDataEntity;
using CatsProj.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsProj.DAL.Providers
{
    public class ElementProvider
    {
        public List<tbl_elementgroup> getElementGroup()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_elementgroup> result = db.Queryable<tbl_elementgroup>().ToList();
            return result;
        }

        public List<tbl_elementdetail> getElementGroupDtl(int groupId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_elementdetail> result = db.Queryable<tbl_elementdetail>().Where(o => o.elementGroup == groupId).ToList();
            return result;
        }

        public bool buyElementUsingPoint(string openId,int elementId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            long totalScore = user.totalScore;
            tbl_elementdetail element = db.Queryable<tbl_elementdetail>().Where(o => o.elementId == elementId).First();
            if (totalScore >= element.elementPrice)
            {
                tbl_elementbuyrecord record = new tbl_elementbuyrecord();
                record.buyId = Guid.NewGuid().ToString();
                record.openId = openId;
                record.buyTime = DateTime.Now;
                record.elementId = elementId;
                record.money = 0;
                record.point = element.elementPrice;
                db.Insertable<tbl_elementbuyrecord>(record).ExecuteCommand();
                user.totalScore = user.totalScore - element.elementPrice;
                db.Updateable<tbl_user>(user).Where(o => o.openid == openId).ExecuteCommand();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ifBoughtElement(string openId, int elementId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            bool ifBought = db.Queryable<tbl_elementbuyrecord>().Where(o => o.openId == openId && o.elementId == elementId).Count()>0;
            return ifBought;
        }

        public void saveUserElement(tbl_usersaveelement save)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_usersaveelement>(save).ExecuteCommand();    
        }
    }
}
