using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_shopcart
    {
        public string cartId { get; set; }
        public string openId { get; set; }
        public DateTime addTime { get; set; }
        public int buystatus { get; set; }//0还未付款，1付款
        public int itemId { get; set; }
        public int buyCount { get; set; }
    }
}
