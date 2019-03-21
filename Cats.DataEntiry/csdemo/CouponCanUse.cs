using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class CouponCanUse
    {
        public string couponId { get; set; }
        public double couponAmt { get; set; }
        public double baseline { get; set; }
        public bool ifCanUse { get; set; }
        public DateTime deadline { get; set; }
    }
}
