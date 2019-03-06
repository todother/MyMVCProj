using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_cartItem
    {
        public int itemId { get; set; }
        public string cartId { get; set; }
        public string openId { get; set; }
        public string itemName { get; set; }
        public double itemPrice { get; set; }
        public double itemPriceDdt { get; set; }
        public string Specs { get; set; }
        public int buyCount { get; set; }
        public string picName { get; set; }
        public int picIdx { get; set; }
        public int buyStatus { get; set; }
    }
}
