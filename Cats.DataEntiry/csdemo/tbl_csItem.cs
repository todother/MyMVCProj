using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_csItem
    {
        public int itemId { get; set; }
        public int itemCate { get; set; }
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double itemPrice { get; set; }
        public double itemPriceDdt { get; set; }
        public string itemSpecs { get; set; }
    }
}
