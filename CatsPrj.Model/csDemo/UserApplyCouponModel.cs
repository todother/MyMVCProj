using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class UserApplyCouponModel
    {
        public string couponId { get; set; }
        public string openId { get; set; }
        public int applyTimes { get; set; }
        public int canApplyTimes { get; set; }
        public int remainCount { get; set; }
        public double baseline { get; set; }
        public double couponAmt { get; set; }
        public int period { get; set; }
    }
}
