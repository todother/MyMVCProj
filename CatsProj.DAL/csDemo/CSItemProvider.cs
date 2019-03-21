using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
using Cats.DataEntiry;
using System.Data;
using System.Web.Configuration;
using Cats.DataEntiry.csdemo;
using System.Linq;

namespace CatsProj.DAL.csDemo
{
    public class CSItemProvider
    {
        //public static string DBPath = WebConfigurationManager.AppSettings["DBServer"];



        public List<ItemWithSelCount> getItemsByType(int cateId, string DBPath, string openId)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
                List<ItemWithSelCount>
                    items = db.Queryable<tbl_csItem, tbl_shopcart>((ti, ts) => new object[] {
                   JoinType.Left,ti.itemId==ts.itemId
            }).Where((ti, ts) => ti.itemCate == cateId)
            .GroupBy((ti, ts) => ti.itemId)
            .Select((ti, ts) => new ItemWithSelCount
            {
                itemCate = ti.itemCate,
                itemDesc = ti.itemDesc,
                itemId = ti.itemId,
                itemName = ti.itemName,
                itemPrice = ti.itemPrice,
                itemPriceDdt = ti.itemPriceDdt,
                itemSpecs = ti.itemSpecs,
                selCount = SqlFunc.AggregateMax(SqlFunc.IIF(ts.buystatus == 0 && ts.openId == openId, SqlFunc.IsNull(ts.buyCount, 0), 0)),
                totalSold = SqlFunc.IIF(ts.buystatus == 1, SqlFunc.AggregateSum(SqlFunc.IsNull(ts.buyCount, 0)), 0)
            })
            .ToList();
                return items;
            }
            catch (Exception e)
            {
                return new List<ItemWithSelCount>();
            }
        }

        public List<ItemWithSelCount> getItemsByType(int cateId, string DBPath, string openId,int count)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
                List<ItemWithSelCount>
                    items = db.Queryable<tbl_csItem, tbl_shopcart>((ti, ts) => new object[] {
                   JoinType.Left,ti.itemId==ts.itemId
            }).Where((ti, ts) => ti.itemCate == cateId)
            .GroupBy((ti, ts) => ti.itemId).OrderBy((ti, ts) => SqlFunc.AggregateCount(ts.buyCount),OrderByType.Desc)
            .Select((ti, ts) => new ItemWithSelCount
            {
                itemCate = ti.itemCate,
                itemDesc = ti.itemDesc,
                itemId = ti.itemId,
                itemName = ti.itemName,
                itemPrice = ti.itemPrice,
                itemPriceDdt = ti.itemPriceDdt,
                itemSpecs = ti.itemSpecs,
                selCount = SqlFunc.AggregateMax(SqlFunc.IIF(ts.buystatus == 0 && ts.openId == openId, SqlFunc.IsNull(ts.buyCount, 0), 0)),
                totalSold = SqlFunc.IIF(ts.buystatus == 1, SqlFunc.AggregateSum(SqlFunc.IsNull(ts.buyCount, 0)), 0)
            })
            .ToList();
                if (items.Count > count)
                {
                    items = items.Take(count).ToList();
                }
                return items;
            }
            catch (Exception e)
            {
                return new List<ItemWithSelCount>();
            }
        }

        public int getItemTotalsold(string DBPath,int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<int> totalSold = db.Queryable<tbl_shopcart>().Where(o => o.itemId == itemId && o.buystatus == 1).Select(o=>o.buyCount).ToList();
            int result = totalSold.Sum();
            return result;
        }

        public List<ItemWithSelCount> getItemsByTypePageList(int cateId, string DBPath, string openId,int from,int size)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<ItemWithSelCount>
                items = db.Queryable<tbl_csItem, tbl_shopcart>((ti, ts) => new object[] {
                   JoinType.Left,ti.itemId==ts.itemId
        }).Where((ti, ts) => ti.itemCate == cateId)
        .GroupBy((ti, ts) => ti.itemId)
        .Select((ti, ts) => new ItemWithSelCount
        {
            itemCate = ti.itemCate,
            itemDesc = ti.itemDesc,
            itemId = ti.itemId,
            itemName = ti.itemName,
            itemPrice = ti.itemPrice,
            itemPriceDdt = ti.itemPriceDdt,
            itemSpecs = ti.itemSpecs,
            selCount = SqlFunc.AggregateMax(SqlFunc.IIF(ts.buystatus == 0 && ts.openId == openId, SqlFunc.IsNull(ts.buyCount, 0), 0)),
            totalSold = SqlFunc.IIF(ts.buystatus == 1, SqlFunc.AggregateSum(SqlFunc.IsNull(ts.buyCount, 0)), 0)
        })
        .ToPageList(from/size+1,size);
            return items;
        }

        public List<ItemWithSelCount> getItemsBySearch(string search, string DBPath, string openId, int from, int size)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<ItemWithSelCount>
                items = db.Queryable<tbl_csItem, tbl_shopcart>((ti, ts) => new object[] {
                   JoinType.Left,ti.itemId==ts.itemId
        }).Where((ti, ts) => SqlFunc.Contains( ti.itemName,search) || SqlFunc.Contains(ti.itemDesc, search))
        .GroupBy((ti, ts) => ti.itemId)
        .Select((ti, ts) => new ItemWithSelCount
        {
            itemCate = ti.itemCate,
            itemDesc = ti.itemDesc,
            itemId = ti.itemId,
            itemName = ti.itemName,
            itemPrice = ti.itemPrice,
            itemPriceDdt = ti.itemPriceDdt,
            itemSpecs = ti.itemSpecs,
            selCount = SqlFunc.AggregateMax(SqlFunc.IIF(ts.buystatus == 0 && ts.openId == openId, SqlFunc.IsNull(ts.buyCount, 0), 0)),
            totalSold = SqlFunc.IIF(ts.buystatus == 1, SqlFunc.AggregateSum(SqlFunc.IsNull(ts.buyCount, 0)), 0)
        })
        .ToPageList(from / size + 1, size);
            if (from == 0)
            {
                saveSearch(DBPath, search, openId);
            }
            return items;
        }

        public void saveSearch(string DBPath,string search,string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_search entity = new tbl_search();
            entity.searchId = Guid.NewGuid().ToString();
            entity.openId = openId;
            entity.searchContent = search;
            entity.searchTime = DateTime.Now;
            db.Insertable(entity).ExecuteCommand();
        }

        public List<string> getSelfSearch(string DBPath,string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<string> search = db.Queryable<tbl_search>().Where(o => o.openId == openId).OrderBy(o => o.searchTime, OrderByType.Desc).GroupBy(o=>o.searchContent).Select(o => o.searchContent).ToList();
            if (search.Count > 20)
            {
                search = search.Take(20).ToList();
            }
            return search;
        }

        public List<string> getHotSearch(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<string> search = db.Queryable<tbl_search>().OrderBy(o => SqlFunc.AggregateCount(o.searchContent), OrderByType.Desc).GroupBy(o => o.searchContent).Select(o => o.searchContent).ToList();
            if (search.Count > 20)
            {
                search = search.Take(20).ToList();
            }
            return search;
        }

        public List<tbl_csCate> getCates(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_csCate> cates = db.Queryable<tbl_csCate>().OrderBy(o=>o.cateIdx,OrderByType.Asc).ToList();
            return cates;
        }

        public List<tbl_csCate> getCates(string DBPath,int from,int size)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_csCate> cates = db.Queryable<tbl_csCate>().OrderBy(o => o.cateIdx, OrderByType.Asc).ToPageList(from/size+1,size);
            return cates;
        }

        public List<tbl_csItemPics> getItemPicsByItemId(int itemId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_csItemPics> pics = db.Queryable<tbl_csItemPics>().Where(o => o.itemId == itemId).OrderBy(o => o.picIdx, OrderByType.Asc).ToList();
            return pics;
        }

        public List<tbl_banner> getBanners(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_banner> banners = db.Queryable<tbl_banner>().OrderBy(o => o.picIdx,OrderByType.Asc).ToList();
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
            }).Where((tc, ti, tp) => tc.openId == openId && tc.buystatus == 0 && tp.picIdx == 0)
            .Select((tc, ti, tp) => new tbl_cartItem
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
            }).ToList();
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

        public void addItemBy1(string DBPath, int itemId, string openId, int buycount)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_shopcart shopcart = db.Queryable<tbl_shopcart>().Where(o => o.openId == openId && o.itemId == itemId && o.buystatus == 0).First();
            if (shopcart != null)
            {
                shopcart.buyCount += buycount;
                if (shopcart.buyCount == 0)
                {
                    db.Deleteable<tbl_shopcart>().Where(o => o.cartId == shopcart.cartId).ExecuteCommand();
                }
                else
                {
                    db.Updateable<tbl_shopcart>(shopcart).Where(o => o.cartId == shopcart.cartId).ExecuteCommand();
                }
            }
            else
            {
                //shopcart = new tbl_shopcart();
                string cartId = Guid.NewGuid().ToString();
                DateTime addTime = DateTime.Now;
                tbl_shopcart cart = new tbl_shopcart();
                cart.itemId = itemId;
                cart.openId = openId;
                cart.cartId = cartId;
                cart.addTime = addTime;
                cart.buystatus = 0;
                cart.buyCount = buycount;
                db.Insertable<tbl_shopcart>(cart).ExecuteCommand();
            }
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

        public string addOrderList(string cartId, string DBPath, string addressId)
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
                buyrecord.addressId = addressId;
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

        public string addReceiveAddress(string receiver, string address, string openId, string telNo, string DBPath, bool ifDefault, string addId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);

            tbl_receiveAddress receive = new tbl_receiveAddress();
            string resultID = "";
            if (addId.Length > 0)
            {
                receive = db.Queryable<tbl_receiveAddress>().Where(o => o.addressId == addId).First();
                List<tbl_receiveAddress> adds = db.Queryable<tbl_receiveAddress>().Where(o => o.openId == openId).ToList();
                foreach (var item in adds)
                {
                    item.selected = 0;
                    db.Updateable<tbl_receiveAddress>(item).Where(o => o.addressId == item.addressId).ExecuteCommand();
                }
                receive.addressId = addId;
                receive.openId = openId;
                receive.receiver = receiver;
                receive.selected = ifDefault == true ? 1 : 0;
                receive.telno = telNo;
                receive.homeaddress = address;

                db.Updateable<tbl_receiveAddress>(receive).Where(o => o.addressId == addId).ExecuteCommand();
                resultID = addId;
            }
            else
            {
                List<tbl_receiveAddress> adds = db.Queryable<tbl_receiveAddress>().Where(o => o.openId == openId).ToList();
                foreach (var item in adds)
                {
                    item.selected = 0;
                    db.Updateable<tbl_receiveAddress>(item).Where(o => o.addressId == item.addressId).ExecuteCommand();
                }
                receive.addressId = Guid.NewGuid().ToString();
                receive.openId = openId;
                receive.receiver = receiver;
                receive.selected = ifDefault == true ? 1 : 0;
                receive.telno = telNo;
                receive.homeaddress = address;

                db.Insertable<tbl_receiveAddress>(receive).ExecuteCommand();
                resultID = receive.addressId;
            }
            return resultID;

        }

        public List<tbl_receiveAddress> getAddressByUser(string openId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_receiveAddress> address = db.Queryable<tbl_receiveAddress>().Where(o => o.openId == openId).OrderBy(o => o.selected, OrderByType.Desc).ToList();
            return address;
        }

        public tbl_receiveAddress getAddressByAddId(string DBPath, string addId, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_receiveAddress address = new tbl_receiveAddress();
            if (addId.Length > 0)
            {
                address = db.Queryable<tbl_receiveAddress>().Where(o => o.addressId == addId).First();
            }
            else
            {
                address = db.Queryable<tbl_receiveAddress>().Where(o => o.openId == openId).OrderBy(o => o.selected, OrderByType.Desc).First();
            }
            return address;
        }

        public bool ifUserLoveItem(string DBPath, string openId, int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_userloveitem loveitem = db.Queryable<tbl_userloveitem>().Where(o => o.openId == openId && o.itemId == itemId).First();
            return loveitem == null ? false : true;

        }

        public bool userLoveItem(string DBPath, string openId, int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);

            tbl_userloveitem loveitem = new tbl_userloveitem();
            loveitem = db.Queryable<tbl_userloveitem>().Where(o => o.openId == openId && o.itemId == itemId).First();
            if (loveitem != null)
            {
                db.Deleteable<tbl_userloveitem>().Where(o => o.openId == openId && o.itemId == itemId).ExecuteCommand();
                return false;
            }
            else
            {
                loveitem = new tbl_userloveitem();
                loveitem.itemId = itemId;
                loveitem.loveId = Guid.NewGuid().ToString();
                loveitem.openId = openId;
                loveitem.loveTime = DateTime.Now;
                db.Insertable<tbl_userloveitem>(loveitem).ExecuteCommand();
                return true;
            }

        }

        public int userLoveItemCount(string DBPath, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            int loveCount = db.Queryable<tbl_userloveitem>().Where(o => o.openId == openId).Count();
            return loveCount;
        }

        public long getItemViewTimes(string DBPath, int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            long viewCount = db.Queryable<tbl_csviewrecord>().Where(o => o.itemId == itemId).Count();
            return viewCount;
        }

        public List<tbl_cartItemFull> getCartItemByStepId(string DBPath, string openId, int stepId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            var result = db.Queryable<tbl_trace, tbl_buyrecord, tbl_shopcart, tbl_csItem, tbl_csItemPics>((tt, tb, ts, ti, tip) => new object[]
            {
                JoinType.Left,tt.orderId==tb.orderId,
                JoinType.Left,tb.cartId==ts.cartId,
                JoinType.Left,ts.itemId==ti.itemId,
                JoinType.Left,ti.itemId==tip.itemId
            });
            List<tbl_cartItemFull> items = new List<tbl_cartItemFull>();
            if (stepId == 99)//99代表全部
            {
                items = result.Where((tt, tb, ts, ti, tip) => ts.openId == openId).OrderBy((tt, tb, ts, ti, tip) => tt.updateTime, OrderByType.Desc).Select((tt, tb, ts, ti, tip) => new tbl_cartItemFull
                {
                    buyCount = ts.buyCount,
                    cartId = tb.cartId,
                    buyStatus = ts.buystatus,
                    itemId = ts.itemId,
                    itemName = ti.itemName,
                    itemPrice = ti.itemPrice,
                    itemPriceDdt = ti.itemPriceDdt,
                    openId = ts.openId,
                    orderId = tt.orderId,
                    picIdx = tip.picIdx,
                    picName = tip.picName,
                    Specs = ti.itemSpecs,
                    stepId = tt.stepId,
                    updateTime = tt.updateTime
                }).ToList();
            }
            else
            {
                items = result.Where((tt, tb, ts, ti, tip) => ts.openId == openId && tt.stepId == stepId).OrderBy((tt, tb, ts, ti, tip) => tt.updateTime, OrderByType.Desc).Select((tt, tb, ts, ti, tip) => new tbl_cartItemFull
                {
                    buyCount = ts.buyCount,
                    cartId = tb.cartId,
                    buyStatus = ts.buystatus,
                    itemId = ts.itemId,
                    itemName = ti.itemName,
                    itemPrice = ti.itemPrice,
                    itemPriceDdt = ti.itemPriceDdt,
                    openId = ts.openId,
                    orderId = tt.orderId,
                    picIdx = tip.picIdx,
                    picName = tip.picName,
                    Specs = ti.itemSpecs,
                    stepId = tt.stepId,
                    updateTime = tt.updateTime
                }).ToList();
            }
            return items;
        }

        public List<string> getAdmins(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<string> admins = db.Queryable<Cats.DataEntiry.csdemo.tbl_admin>().Select(o => o.openId).ToList();
            return admins;
        }

        public List<tbl_itemFullWithAddress> getCartItemByStepIdForAdmin(string DBPath, string openId, int stepId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            var result = db.Queryable<tbl_trace, tbl_buyrecord, tbl_shopcart, tbl_csItem, tbl_csItemPics, tbl_receiveAddress>((tt, tb, ts, ti, tip, tra) => new object[]
             {
                JoinType.Left,tt.orderId==tb.orderId,
                JoinType.Left,tb.cartId==ts.cartId,
                JoinType.Left,ts.itemId==ti.itemId,
                JoinType.Left,ti.itemId==tip.itemId,
                JoinType.Left,tb.addressId==tra.addressId
             });
            List<tbl_itemFullWithAddress> items = new List<tbl_itemFullWithAddress>();
            if (stepId == 99)//99代表全部
            {
                items = result.OrderBy((tt, tb, ts, ti, tip, tra) => tt.updateTime, OrderByType.Desc).Select((tt, tb, ts, ti, tip, tra) => new tbl_itemFullWithAddress
                {
                    buyCount = ts.buyCount,
                    cartId = tb.cartId,
                    buyStatus = ts.buystatus,
                    itemId = ts.itemId,
                    itemName = ti.itemName,
                    itemPrice = ti.itemPrice,
                    itemPriceDdt = ti.itemPriceDdt,
                    openId = ts.openId,
                    orderId = tt.orderId,
                    picIdx = tip.picIdx,
                    picName = tip.picName,
                    Specs = ti.itemSpecs,
                    stepId = tt.stepId,
                    updateTime = tt.updateTime,
                    receiver = tra.receiver,
                    homeAddress = tra.homeaddress,
                    addressId = tra.addressId,
                    telNo = tra.telno
                }).ToList();
            }
            else
            {
                items = result.Where((tt, tb, ts, ti, tip, tra) => tt.stepId == stepId)
                    .OrderBy((tt, tb, ts, ti, tip, tra) => tt.updateTime, OrderByType.Asc)
                    .Select((tt, tb, ts, ti, tip, tra) => new tbl_itemFullWithAddress
                    {
                        buyCount = ts.buyCount,
                        cartId = tb.cartId,
                        buyStatus = ts.buystatus,
                        itemId = ts.itemId,
                        itemName = ti.itemName,
                        itemPrice = ti.itemPrice,
                        itemPriceDdt = ti.itemPriceDdt,
                        openId = ts.openId,
                        orderId = tt.orderId,
                        picIdx = tip.picIdx,
                        picName = tip.picName,
                        Specs = ti.itemSpecs,
                        stepId = tt.stepId,
                        updateTime = tt.updateTime,
                        receiver = tra.receiver,
                        homeAddress = tra.homeaddress,
                        addressId = tra.addressId,
                        telNo = tra.telno
                    }).ToList();
            }
            return items;
        }

        public List<int> getEachStepCount(string openId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            var result = db.Queryable<tbl_trace, tbl_buyrecord, tbl_shopcart>((tt, tb, ts) => new object[] {
                JoinType.Left,tt.orderId==tb.orderId,
                JoinType.Left,tb.cartId==ts.cartId
            });
            List<int> stepCount = new List<int>();
            int i = 0;
            for (i = 0; i < 6; i++)
            {
                int count = result.Where((tt, tb, ts) => ts.openId == openId && tt.stepId == i).Select((tt, tb, ts) => new tbl_cartItemFull
                {
                    orderId = tb.orderId,
                    openId = ts.openId,
                    stepId = tt.stepId
                }).Count();
                stepCount.Add(count);
            }
            return stepCount;
        }

        public int getUserViewRecord(string DBPath, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            int viewTimes = db.Queryable<tbl_csviewrecord>().Where(o => o.openId == openId).GroupBy(o => o.itemId).OrderBy(o => o.viewTime, OrderByType.Desc).Count();
            return viewTimes;
        }

        public void userViewItem(string DBPath, string openId, int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_csviewrecord view = new tbl_csviewrecord();
            view.viewId = Guid.NewGuid().ToString();
            view.openId = openId;
            view.viewTime = DateTime.Now;
            view.itemId = itemId;
            db.Insertable<tbl_csviewrecord>(view).ExecuteCommand();
        }

        public void delOrderItems(string DBPath, string orderId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            db.Deleteable<tbl_buyrecord>().Where(o => o.orderId == orderId).ExecuteCommand();
        }
        public void confirmStep(string DBPath, string orderId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_trace trace = db.Queryable<tbl_trace>().Where(o => o.orderId == orderId).First();
            trace.stepId += 1;
            db.Updateable<tbl_trace>(trace).Where(o => o.orderId == orderId).ExecuteCommand();
        }

        public List<tbl_sendmessage> getMessageIds(string openId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);

            List<tbl_sendmessage> list = db.Queryable<tbl_sendmessage>().Where(o => o.openId == openId && o.genTime.AddDays(7) >= DateTime.Now).OrderBy(o => o.genTime, OrderByType.Asc).ToList();
            return list;
        }

        

        public void updateUsedTimes(string messageId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_sendmessage message = db.Queryable<tbl_sendmessage>().Where(o => o.usedId == messageId).First();
            message.usedTimes += 1;
            if (!messageId.Contains(" mock"))
            {
                db.Updateable<tbl_sendmessage>(message).Where(o => o.usedId == messageId).ExecuteCommand();
            }
        }

        public void addNewMessageId(string openId, string msg, int msgType, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_sendmessage message = new tbl_sendmessage();
            message.genTime = DateTime.Now;
            message.idtype = msgType;
            message.messageId = Guid.NewGuid().ToString();
            message.usedId = msg;
            message.usedTimes = 0;
            message.openId = openId;
            db.Insertable<tbl_sendmessage>(message).ExecuteCommand();

        }

        public List<tbl_cartItemFull> getItemsByOrderId(string orderId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_cartItemFull> items = db.Queryable<tbl_buyrecord, tbl_shopcart, tbl_csItem, tbl_receiveAddress>((tb, ts, ti, tr) => new object[] {
                  JoinType.Left,tb.cartId==ts.cartId,
                  JoinType.Left,ts.itemId==ti.itemId,
                  JoinType.Left,tb.addressId==tr.addressId
            }).Where((tb, ts, ti, tr) => tb.orderId == orderId)
            .Select((tb, ts, ti, tr) => new tbl_cartItemFull
            {
                buyCount = ts.buyCount,
                cartId = tb.cartId,
                itemName = ti.itemName,
                itemPriceDdt = ti.itemPriceDdt,
                orderId = tb.orderId,
                openId = ts.openId,
                updateTime = tb.subTime,
                userName = tr.receiver

            }).ToList();
            return items;
        }

        public tbl_msgTemplate getMsgTemp(string DBPath, int tempType)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_msgTemplate temp = db.Queryable<tbl_msgTemplate>().Where(o => o.tempType == tempType).First();
            return temp;
        }

        public int getCurrStep(string orderId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            int step = db.Queryable<tbl_trace>().Where(o => o.orderId == orderId).First().stepId;
            return step;
        }

        public int getShopCartCount(string openId, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            int count = db.Queryable<tbl_shopcart>().Where(o => o.openId == openId && o.buystatus == 0).Count();
            return count;
        }
        public int saveItemChange(string DBPath, string itemName, string itemDesc, double itemPrice, double itemPriceDdt, int stock, int cateId, int itemId, string barcode)
        {
            tbl_csItem item = new tbl_csItem();
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            item = db.Queryable<tbl_csItem>().Where(o => o.itemId == itemId).First();
            if (item != null)
            {
                item.itemName = itemName;
                item.itemDesc = itemDesc;
                item.itemPrice = itemPrice;
                item.itemPriceDdt = itemPriceDdt;
                item.stock = stock;
                item.itemCate = cateId;
                item.barcode = barcode;
                db.Updateable<tbl_csItem>(item).Where(o => o.itemId == itemId).ExecuteCommand();
            }
            else
            {
                item = new tbl_csItem();
                item.itemName = itemName;
                item.itemDesc = itemDesc;
                item.itemPrice = itemPrice;
                item.itemPriceDdt = itemPriceDdt;
                item.stock = stock;
                item.itemCate = cateId;
                item.barcode = barcode;
                itemId = db.Insertable<tbl_csItem>(item).ExecuteReturnIdentity();
            }
            return itemId;
        }

        public string saveItemPic(string DBPath, int index, int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_csItemPics pic = new tbl_csItemPics();
            pic.itemId = itemId;
            pic.picIdx = index;
            pic.picName = Guid.NewGuid().ToString();
            db.Insertable<tbl_csItemPics>(pic).ExecuteCommand();
            return pic.picName;
        }

        public List<string> delPicImgs(string DBPath, int itemId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<string> imgList = db.Queryable<tbl_csItemPics>().Where(O => O.itemId == itemId).Select(o => o.picName).ToList();
            db.Deleteable<tbl_csItemPics>().Where(o => o.itemId == itemId).ExecuteCommand();
            return imgList;
        }

        public int getItemIdByBarcode(string barCode, string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            int itemId = -1;
            var item = db.Queryable<tbl_csItem>().Where(o => o.barcode == barCode).First();
            if (item != null)
            {
                itemId = db.Queryable<tbl_csItem>().Where(o => o.barcode == barCode).First().itemId;
            }
            return itemId;
        }

        public void saveNewCoupon(string DBPath, int userType, int packetPeriod, double baseline, double packetAmt, int canApplyTimes, int packetCount)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_redpacketorig packet = new tbl_redpacketorig();
            packet.baseline = baseline;
            packet.canApplyTimes = canApplyTimes;
            packet.genDate = DateTime.Now;
            packet.packetAmt = packetAmt;
            packet.packetCount = packetCount;
            packet.packetPeriod = packetPeriod;
            packet.packetId = Guid.NewGuid().ToString();
            packet.packetType = userType;
            db.Insertable<tbl_redpacketorig>(packet).ExecuteCommand();
        }

        public bool ifApplyPacketSucc(string DBPath, string packetId, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_redpacketorig orig = db.Queryable<tbl_redpacketorig>().Where(o => o.packetId == packetId).First();
            tbl_userpacket userpacket = new tbl_userpacket();
            userpacket.openId = openId;
            userpacket.packetAmt = orig.packetAmt;
            userpacket.packetId = packetId;
            userpacket.status = 0;
            userpacket.userpacketId = Guid.NewGuid().ToString();
            userpacket.baseline = orig.baseline;
            userpacket.deadline = DateTime.Now.AddDays(orig.packetPeriod);
            if (orig.packetCount > 0)
            {
                orig.packetCount -= 1;
                db.Updateable(orig).Where(o => o.packetId == packetId).ExecuteCommand();
                db.Insertable(userpacket).ExecuteCommand();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void useCoupon(string DBPath, string openId, string userpacketId, string orderId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_userpacket packet = db.Queryable<tbl_userpacket>().Where(o => o.userpacketId == userpacketId && o.status == 0 && o.openId == openId).First();
            if (packet != null)
            {
                packet.status = 1;
                packet.useTime = DateTime.Now;
                packet.orderId = orderId;
                db.Updateable(packet).Where(o => o.userpacketId == userpacketId).ExecuteCommand();
                //return packet;
            }
            else
            {
                //return new tbl_userpacket();
            }
        }

        public tbl_userpacket getCoupon(string DBPath, string openId, string userpacketId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_userpacket packet = db.Queryable<tbl_userpacket>().Where(o => o.userpacketId == userpacketId && o.status == 0 && o.openId == openId).First();
            return packet;
        }

        public List<tbl_redpacketorig> getAllCoupons(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_redpacketorig> coupons = db.Queryable<tbl_redpacketorig>().Where(o => o.packetCount > 0).ToList();
            return coupons;
        }

        public List<UserApplyCoupon> getCouponForUser(string DBPath, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<UserApplyCoupon> coupons = db.Queryable<tbl_redpacketorig>().Where((tr) => tr.packetCount > 0 && tr.packetType == 1).
               OrderBy((tr) => tr.genDate, OrderByType.Desc).
               Select((tr) => new UserApplyCoupon
               {
                   applyTimes = SqlFunc.Subqueryable<tbl_userpacket>().Where(o => o.packetId == tr.packetId && o.openId == openId).Count(),
                   canApplyTimes = tr.canApplyTimes,
                   baseline = tr.baseline,
                   couponAmt = tr.packetAmt,
                   couponId = tr.packetId,
                   period = tr.packetPeriod,
                   remainCount = tr.packetCount

               }).ToList();
            return coupons;
        }

        public List<tbl_userpacket> getUserPacket(string DBPath, string openId, int searchType)//searchtype 0可用 1过期 2已使用
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<tbl_userpacket> result = new List<tbl_userpacket>();
            var coupons = db.Queryable<tbl_userpacket>();
            if (searchType == 0)
            {
                result = coupons.Where(o => o.deadline >= DateTime.Now && o.openId == openId && o.status == 0).ToList();
            }
            else if (searchType == 1)
            {
                result = coupons.Where(o => o.openId == openId && o.deadline < DateTime.Now && o.status == 0).ToList();
            }
            else if (searchType == 2)
            {
                result = coupons.Where(o => o.openId == openId && o.status == 1).ToList();
            }
            return result;
        }

        public List<CouponCanUse> getCouponCanuse(string openId, string DBPath, double totalAmt)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            List<CouponCanUse> result = db.Queryable<tbl_userpacket>().Where(o => o.status == 0 && o.openId == openId && o.deadline >= DateTime.Now)
                .OrderBy(o => SqlFunc.IIF(totalAmt >= o.baseline, 1, 0), OrderByType.Desc).OrderBy(o => o.packetAmt, OrderByType.Desc)
                .Select(o => new CouponCanUse
                {
                    baseline = o.baseline,
                    couponAmt = o.packetAmt,
                    couponId = o.userpacketId,
                    ifCanUse = SqlFunc.IIF(totalAmt >= o.baseline, true, false),
                    deadline = o.deadline
                }).ToList();
            return result;
        }

        public CouponCanUse getCouponByCouponId(string DBPath, string couponId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            CouponCanUse result = db.Queryable<tbl_userpacket>().Where(o => o.status == 0 && o.userpacketId == couponId && o.deadline >= DateTime.Now)
                .Select(o => new CouponCanUse
                {
                    baseline = o.baseline,
                    couponAmt = o.packetAmt,
                    couponId = o.userpacketId,
                    ifCanUse = true,
                    deadline = o.deadline
                }).First();
            return result;
        }

        public void saveFailPay(string DBPath, string orderId, string paySign, string packegId, string nonceStr, string timeStamp, double totalAmt)
        {
            tbl_failpay failpay = new tbl_failpay();
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            failpay.orderId = orderId;
            failpay.paySign = paySign;
            failpay.totalAmt = totalAmt;
            failpay.timeStamp = timeStamp;
            failpay.nonceStr = nonceStr;
            failpay.packageId = packegId;   
            failpay.submitTime = DateTime.Now;
            db.Insertable<tbl_failpay>(failpay).ExecuteCommand();
        }

        public tbl_failpay getFailPayByOrderId(string DBPath,string orderId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_failpay failpay = db.Queryable<tbl_failpay>().Where(o => o.orderId == orderId).First();
            return failpay;
        }

        public tbl_userpacket getCouponByOrderId(string DBPath,string orderId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            tbl_userpacket userpacket = db.Queryable<tbl_userpacket>().Where(o => o.orderId == orderId).First();
            return userpacket;
        }

        public void saveBanner(string DBPath,string picName,int picIdx)
        {
            tbl_banner banner = new tbl_banner();
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            banner.picName = picName;
            banner.picIdx = picIdx;
            db.Insertable(banner).ExecuteCommand();
        }
        public void delBanner(string DBPath)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance(DBPath);
            db.Deleteable<tbl_banner>().ExecuteCommand();
        }
    }
}
