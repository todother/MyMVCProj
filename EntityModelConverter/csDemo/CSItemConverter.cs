using Cats.DataEntiry.csdemo;
using CatsPrj.Model.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class CSItemConverter
    {
        public static CSItemModel csItemEntityToModel(tbl_csItem item,List<tbl_csItemPics> pics)
        {
            CSItemModel model = new CSItemModel();
            model.itemCate = item.itemCate;
            model.itemDesc = item.itemDesc;
            model.itemId = item.itemId;
            model.itemName = item.itemName;
            model.itemPrice = item.itemPrice;
            model.itemPriceDdt = item.itemPriceDdt;
            model.Specs = item.itemSpecs;
            List<CSPicModel> picList = new List<CSPicModel>();
            foreach(var pic in pics)
            {
                CSPicModel picModel = new CSPicModel();
                picModel.itemId = pic.itemId;
                picModel.picId = pic.picId;
                picModel.picIdx = pic.picIdx;
                picModel.picName = pic.picName;
                picList.Add(picModel);
            }
            model.picList = picList;
            return model;
        }
    }
}
