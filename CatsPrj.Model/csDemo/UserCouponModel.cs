using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class UserCouponModel
    {
        public string userpacketId { get; set; }
        public string packetId { get; set; }
        public string openId { get; set; }
        public string deadline { get; set; }
        public int status { get; set; }//0未用，1已用
        public string orderId { get; set; }
        public DateTime? useTime { get; set; }
        public double baseline { get; set; }
        public double packetAmt { get; set; }
    }
}
