using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class CouponCanUseModel
    {
        public string couponId { get; set; }
        public double packetAmt { get; set; }
        public double baseline { get; set; }
        public bool ifCanUse { get; set; }
        public string deadline { get; set; }
    }
}
