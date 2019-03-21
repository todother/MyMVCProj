using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class ShopCartItemModel
    {
        public int itemId { get; set; }
        public string itemName { get; set; }
        public double itemPrice { get; set; }
        public double itemPriceDdt { get; set; }
        public string Specs { get; set; }
        public int stock { get; set; }
        public string barcode { get; set; }
        public int buyCount { get; set; }
        public string picName { get; set; }
        public double scrollLeft { get; set; }
        public string whichShow { get; set; }
        public string cartId { get; set; }
        public double totalPrice { get; set; }
        public bool selected { get; set; }
    }
}
