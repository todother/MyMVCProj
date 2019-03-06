using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class CSSellModel
    {
        public string sellId { get; set; }
        public string openId { get; set; }
        public DateTime sellTime { get; set; }
        public double sellPrice { get; set; }
        public int itemId { get; set; }
        public string orderId { get; set; }
    }
}
