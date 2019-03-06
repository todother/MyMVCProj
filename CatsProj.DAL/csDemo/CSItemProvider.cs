using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
using Cats.DataEntiry;
using System.Data;
using System.Web.Configuration;
using Cats.DataEntiry.csdemo;

namespace CatsProj.DAL.csDemo
{
    public class CSItemProvider
    {
        //public static string DBPath = WebConfigurationManager.AppSettings["DBServer"];



        public List<tbl_csItem> getItemsByType(int cateId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_csItem> items = db.Queryable<tbl_csItem>().Where(o => o.itemCate == cateId).ToList();
            return items;
        }

        public List<tbl_csCate> getCates(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_csCate> cates = db.Queryable<tbl_csCate>().ToList();
            return cates;
        }

        public List<tbl_csItemPics> getItemPicsByItemId(int itemId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_csItemPics> pics = db.Queryable<tbl_csItemPics>().Where(o => o.itemId == itemId).ToList();
            return pics;
        }

        public List<tbl_banner> getBanners(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_banner> banners = db.Queryable<tbl_banner>().OrderBy(o => o.bannerId).ToList();
            return banners;
        }

        public tbl_csItem getItemDetail(int itemId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_csItem item = db.Queryable<tbl_csItem>().Where(o => o.itemId == itemId).First();
            return item;
        }

        public List<tbl_cartItem> getCartItems(string DBPath, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_cartItem> items = db.Queryable<tbl_shopcart, tbl_csItem, tbl_csItemPics>((tc, ti, tp) => new object[] {
                JoinType.Left,tc.itemId==ti.itemId,
                JoinType.Left,ti.itemId==tp.itemId
            }).Select((tc, ti, tp) => new tbl_cartItem
            {
                cartId = tc.cartId,
                itemId = tc.itemId,
                openId = tc.openId,
                itemName = ti.itemName,
                itemPrice = ti.itemPrice,
                itemPriceDdt = ti.itemPriceDdt,
                Specs = ti.itemSpecs,
                buyCount = tc.buyCount,
                picName = tp.picName,
                buyStatus = tc.buystatus,
                picIdx = tp.picIdx
            }).Where(tc => tc.openId == openId && tc.buyStatus == 0).Where(tp => tp.picIdx == 0).ToList();
            return items;
        }

        public string addShopCart(string DBPath, string openId, int itemId, int buyCount)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            string cartId = string.Empty;
            tbl_shopcart ifexist = db.Queryable<tbl_shopcart>().Where(o => o.openId == openId && o.itemId == itemId && SqlFunc.ToInt32(o.buystatus) == 0).First();
            if (ifexist != null)
            {
                ifexist.buyCount += buyCount;
                cartId = ifexist.cartId;
                db.Updateable<tbl_shopcart>(ifexist).Where(o => o.cartId == cartId).ExecuteCommand();
            }
            else
            {
                cartId = Guid.NewGuid().ToString();
                DateTime addTime = DateTime.Now;
                tbl_shopcart cart = new tbl_shopcart();
                cart.itemId = itemId;
                cart.openId = openId;
                cart.cartId = cartId;
                cart.addTime = addTime;
                cart.buystatus = 0;
                cart.buyCount = buyCount;

                db.Insertable<tbl_shopcart>(cart).ExecuteCommand();
            }

            return cartId;
        }

        public void delShopCart(string DBPath, string cartId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            db.Deleteable<tbl_shopcart>().Where(o => o.cartId == cartId).ExecuteCommand();
        }

        public void successBuyCart(string DBPath, string cartId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_shopcart cart = db.Queryable<tbl_shopcart>().Where(o => o.cartId == cartId).First();
            cart.buystatus = 1;//从未购买更新为已购买
            db.Updateable<tbl_shopcart>(cart).Where(o => o.cartId == cartId).ExecuteCommand();

        }

        public void addItem1(string DBPath, string cartId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_shopcart cart = db.Queryable<tbl_shopcart>().Where(o => o.cartId == cartId).First();
            cart.buyCount = cart.buyCount + 1;
            db.Updateable<tbl_shopcart>(cart).Where(o => o.cartId == cartId).ExecuteCommand();
        }

        public void delItem1(string DBPath, string cartId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_shopcart cart = db.Queryable<tbl_shopcart>().Where(o => o.cartId == cartId).First();
            cart.buyCount = (cart.buyCount > 1 ? cart.buyCount : 2) - 1;
            db.Updateable<tbl_shopcart>(cart).Where(o => o.cartId == cartId).ExecuteCommand();
        }

        public List<tbl_shopcart> getShopCartUnPay(string openId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_shopcart> shopCarts = db.Queryable<tbl_shopcart>().Where(o => o.buystatus == 0 && o.openId == openId).ToList();
            return shopCarts;
        }

        public string addOrderList(string cartId, string DBPath)
        {
            cartId = cartId.Substring(0, cartId.Length - 1);
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            string orderId = Guid.NewGuid().ToString();
            string[] carts = cartId.Split('_');
            DateTime subtime = DateTime.Now;
            foreach (var item in carts)
            {
                tbl_buyrecord buyrecord = new tbl_buyrecord();
                buyrecord.orderId = orderId;
                buyrecord.cartId = item;
                buyrecord.subTime = subtime;
                db.Insertable<tbl_buyrecord>(buyrecord).ExecuteCommand();
                tbl_shopcart shopcart = new tbl_shopcart();
                shopcart = db.Queryable<tbl_shopcart>().Where(o => o.cartId == item).First();
                shopcart.buystatus = 1;
                db.Updateable<tbl_shopcart>(shopcart).Where(o => o.cartId == item).ExecuteCommand();
            }
            tbl_trace trace = new tbl_trace();
            trace.orderId = orderId;
            trace.traceId = Guid.NewGuid().ToString();
            trace.stepId = 0;
            trace.updateTime = subtime;
            db.Insertable<tbl_trace>(trace).ExecuteCommand();
            return orderId;
        }

        public void addReceiveAddress(string receiver, string address, string openId, string telNo, string DBPath, bool ifDefault)
        {
            tbl_receiveAddress receive = new tbl_receiveAddress();
            receive.addressId = Guid.NewGuid().ToString();
            receive.openId = openId;
            receive.receiver = receiver;
            receive.selected = ifDefault == true ? 1 : 0;
            receive.telno = telNo;
            receive.homeaddress = address;
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            db.Insertable<tbl_receiveAddress>(receive).ExecuteCommand();

        }

        public List<tbl_receiveAddress> getAddressByUser(string openId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_receiveAddress> address = db.Queryable<tbl_receiveAddress>().Where(o => o.openId == openId).OrderBy(o => o.selected, OrderByType.Desc).ToList();
            return address;
        }
    }
}
