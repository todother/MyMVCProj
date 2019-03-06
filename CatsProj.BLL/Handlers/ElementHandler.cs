using Cats.DataEntiry;
using CatsDataEntity;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using EntityModelConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsProj.BLL.Handlers
{
    public class ElementHandler
    {
        public List<ElementGroupModel> getElementGroup()
        {
            List<tbl_elementgroup> entities=  new ElementProvider().getElementGroup();
            List<ElementGroupModel> result = new List<ElementGroupModel>();
            foreach(var item in entities)
            {
                result.Add(ElementConverter.converEleGrpToModel(item));
            }
            return result;
        }

        public List<ElementDetailModel> getElementDtlById(int groupId)
        {
            List<tbl_elementdetail> entities = new ElementProvider().getElementGroupDtl(groupId);
            List<ElementDetailModel> result = new List<ElementDetailModel>();
            foreach(var item in entities)
            {
                result.Add(ElementConverter.convertEleDtlToModel(item));
            }
            return result;
        }

        public bool ifBoughtElement(string openId,int elementId)
        {
            return new ElementProvider().ifBoughtElement(openId, elementId);
        }

        public bool ifSuccBuyElement(string openId,string elementId,int buyScore)
        {
            List<string> eleList = elementId.Split(',').ToList();
            List<int> eleIntList = new List<int>();
            tbl_user user = new UserProvider().getUser(openId);
            long score = user.totalScore;
            ElementProvider provider = new ElementProvider();
            foreach(var item in eleList)
            {
                if (item.Length > 0)
                {
                    eleIntList.Add(Convert.ToInt32(item));
                }
                
            }
            if (score >= buyScore)
            {
                foreach (var item in eleIntList)
                {
                    //eleIntList.Add(Convert.ToInt32(item));
                    provider.buyElementUsingPoint(openId, item);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void saveUserElement(string openId,int elementId)
        {
            tbl_usersaveelement save = new tbl_usersaveelement();
            save.saveId = Guid.NewGuid().ToString();
            save.openId = openId;
            save.elementId = elementId;
            save.saveTime = DateTime.Now;
        }
    }
}
