using Cats.DataEntiry.csdemo;
using CatsPrj.Model.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class ShopCartConverter
    {
        public static ShopCartItemModel shopCartEntityToModel(tbl_cartItem entity)
        {
            ShopCartItemModel model = new ShopCartItemModel();
            model.buyCount = entity.buyCount;
            model.itemId = entity.itemId;
            model.itemName = entity.itemName;
            model.itemPrice = entity.itemPrice;
            model.itemPriceDdt = entity.itemPriceDdt;
            model.picName = entity.picName;
            model.Specs = entity.Specs;
            model.scrollLeft = 0;
            model.whichShow = "scroll_" + model.itemId;
            model.cartId = entity.cartId;
            model.totalPrice = Math.Round(model.buyCount * model.itemPriceDdt,2);
            model.selected = true;
            return model;

        }
    }
}
