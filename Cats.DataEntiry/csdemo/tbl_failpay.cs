using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_failpay
    {
        public string paySign { get; set; }
        public string orderId { get; set; }
        public DateTime submitTime { get; set; }
        public double totalAmt { get; set; }

        public string nonceStr { get; set; }
        public string timeStamp { get; set; }
        public string packageId { get; set; }
    }
}
