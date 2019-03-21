using Cats.DataEntiry.csdemo;
using CatsPrj.Model.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class CSCateConverter
    {
        public static CSCateModel csCateEntityToModel(tbl_csCate entity,List<CSItemModel> items)
        {
            CSCateModel model = new CSCateModel();
            model.cateId = entity.cateId;
            model.cateIdx = entity.cateIdx;
            model.cateName = entity.cateName;
            model.catePicName = entity.catePicName;
            model.items = items;
            model.selected = false;
            return model;
        }
    }
}
