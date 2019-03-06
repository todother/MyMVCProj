using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_wxpay
    {
        public int shopId { get; set; }
        public string dbpath { get; set; }
        public string appid { get; set; }
        public string wxkey { get; set; }
        public string mch_id { get; set; }
    }
}
