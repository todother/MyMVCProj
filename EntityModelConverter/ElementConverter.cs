using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class ElementConverter
    {
        public static ElementGroupModel converEleGrpToModel(tbl_elementgroup entity)
        {
            ElementGroupModel model = new ElementGroupModel();
            model.elementImage = entity.groupImage;
            model.elementGrpName = entity.groupName;
            model.groupId = entity.groupId;
            model.selected = false;
            return model;
        }

        public static ElementDetailModel convertEleDtlToModel(tbl_elementdetail entity)
        {
            ElementDetailModel model = new ElementDetailModel();
            model.elementId = entity.elementId;
            model.elementImg = entity.elementImg;
            model.elementMoney = entity.elementMoney;
            model.elementPrice = entity.elementPrice;
            model.selected = false;
            return model;
        }
    }
}
