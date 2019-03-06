using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_elementbuyrecord
    {
        public string buyId { get; set; }
        public string openId { get; set; }
        public int elementId { get; set; }
        public DateTime buyTime { get; set; }
        public decimal point { get; set; }
        public decimal money { get; set; }

    }
}
