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
        public static List<CartItemFullModel> convertShopcartsToFullList(List<tbl_cartItemFull> items)
        {
            List<CartItemFullModel> model = new List<CartItemFullModel>();
            foreach(var item in items)
            {
                if (model.Where(o => o.orderId == item.orderId).Count() == 0)
                {
                    CartItemFullModel newitem = new CartItemFullModel();
                    newitem.orderId = item.orderId;
                    newitem.stepId = item.stepId;
                    newitem.updateTime = item.updateTime.ToString("MM-dd HH:mm");
                    ShopCartItemModel shopcart = new ShopCartItemModel();
                    shopcart.buyCount = item.buyCount;
                    shopcart.cartId = item.cartId;
                    shopcart.itemId = item.itemId;
                    shopcart.itemName = item.itemName;
                    shopcart.itemPrice = item.itemPrice;
                    shopcart.itemPriceDdt = item.itemPriceDdt;
                    shopcart.picName = item.picName;
                    shopcart.Specs = item.Specs;
                    shopcart.barcode = item.barcode;
                    shopcart.stock = item.stock;
                    shopcart.totalPrice = Math.Round(item.itemPriceDdt * item.buyCount, 2);
                    newitem.cartItems = new List<ShopCartItemModel>();
                    newitem.cartItems.Add(shopcart);
                    newitem.totalPrice += shopcart.totalPrice;
                    model.Add(newitem);
                }
                else
                {
                    CartItemFullModel selItem = model.Where(o => o.orderId == item.orderId).First();
                    ShopCartItemModel shopcart = new ShopCartItemModel();
                    shopcart.buyCount = item.buyCount;
                    shopcart.cartId = item.cartId;
                    shopcart.itemId = item.itemId;
                    shopcart.itemName = item.itemName;
                    shopcart.itemPrice = item.itemPrice;
                    shopcart.itemPriceDdt = item.itemPriceDdt;
                    shopcart.picName = item.picName;
                    shopcart.totalPrice = Math.Round(item.itemPriceDdt * item.buyCount, 2);
                    shopcart.Specs = item.Specs;
                    shopcart.barcode = item.barcode;
                    shopcart.stock = item.stock;
                    selItem.totalPrice += shopcart.totalPrice;
                    selItem.cartItems.Add(shopcart);
                }
            }
            return model;
        }

        public static List<CSItemWithAddress> convertShopcartsToAddressList(List<tbl_itemFullWithAddress> items)
        {
            List<CSItemWithAddress> model = new List<CSItemWithAddress>();
            foreach (var item in items)
            {
                if (model.Where(o => o.orderId == item.orderId).Count() == 0)
                {
                    CSItemWithAddress newitem = new CSItemWithAddress();
                    newitem.orderId = item.orderId;
                    newitem.stepId = item.stepId;
                    newitem.receiver = item.receiver;
                    newitem.telNo = item.telNo;
                    newitem.homeAddress = item.homeAddress;
                    newitem.updateTime = item.updateTime.ToString("MM-dd HH:mm");
                    ShopCartItemModel shopcart = new ShopCartItemModel();
                    shopcart.buyCount = item.buyCount;
                    shopcart.cartId = item.cartId;
                    shopcart.itemId = item.itemId;
                    shopcart.itemName = item.itemName;
                    shopcart.itemPrice = item.itemPrice;
                    shopcart.itemPriceDdt = item.itemPriceDdt;
                    shopcart.picName = item.picName;
                    shopcart.Specs = item.Specs;
                    shopcart.barcode = item.barcode;
                    shopcart.stock = item.stock;
                    shopcart.totalPrice = Math.Round(item.itemPriceDdt * item.buyCount, 2);
                    newitem.cartItems = new List<ShopCartItemModel>();
                    newitem.cartItems.Add(shopcart);
                    newitem.totalPrice += shopcart.totalPrice;
                    model.Add(newitem);
                }
                else
                {
                    CSItemWithAddress selItem = model.Where(o => o.orderId == item.orderId).First();
                    ShopCartItemModel shopcart = new ShopCartItemModel();
                    shopcart.buyCount = item.buyCount;
                    shopcart.cartId = item.cartId;
                    shopcart.itemId = item.itemId;
                    shopcart.itemName = item.itemName;
                    shopcart.itemPrice = item.itemPrice;
                    shopcart.itemPriceDdt = item.itemPriceDdt;
                    shopcart.barcode = item.barcode;
                    shopcart.stock = item.stock;
                    shopcart.picName = item.picName;
                    shopcart.totalPrice = Math.Round(item.itemPriceDdt * item.buyCount, 2);
                    shopcart.Specs = item.Specs;
                    selItem.totalPrice += shopcart.totalPrice;
                    selItem.cartItems.Add(shopcart);
                }
            }
            return model;
        }
    }
}
