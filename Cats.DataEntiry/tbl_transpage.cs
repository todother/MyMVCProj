using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_transpage
    {
        public string transId { get; set; }
        public string pageName { get; set; }
        public DateTime transTime { get; set; }
        public string openId { get; set; }
    }
}
