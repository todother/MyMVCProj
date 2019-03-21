using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class CSItemWithAddress
    {
        public List<ShopCartItemModel> cartItems { get; set; }
        public string orderId { get; set; }
        public int stepId { get; set; }
        public double totalPrice { get; set; }
        public string updateTime { get; set; }
        public string receiver { get; set; }
        public string telNo { get; set; }
        public string homeAddress { get; set; }
    }
}
