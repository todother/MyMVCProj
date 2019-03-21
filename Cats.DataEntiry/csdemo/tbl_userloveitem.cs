using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_userloveitem
    {
        public string loveId { get; set; }
        public string openId { get; set; }
        public int itemId { get; set; }
        public DateTime loveTime { get; set; }
    }
}
