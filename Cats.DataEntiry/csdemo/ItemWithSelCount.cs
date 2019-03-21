using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class ItemWithSelCount
    {
        public int itemId { get; set; }
        public int itemCate { get; set; }
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double itemPrice { get; set; }
        public double itemPriceDdt { get; set; }
        public string itemSpecs { get; set; }
        public int stock { get; set; }
        public string barcode { get; set; }
        public int selCount { get; set; }
        public int buyStatus { get; set; }
        public long totalSold { get; set; }
        List<tbl_shopcart> shopCarts { get; set; }
        public string openId { get; set; }
        public int buystatus2 { get; set; }
    }
}
