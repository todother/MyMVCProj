using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_trace
    {
        public string traceId { get; set; }
        public string orderId { get; set; }
        public int stepId { get; set; }//0待付款 1已付款未发货 2已发货 3已送达 4申请退货 5已退货 99全部订单
        public DateTime updateTime { get; set; }
    }
}
