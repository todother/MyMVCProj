using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_qdRecord
    {
        public string qdId { get; set; }
        public string openId { get; set; }
        public DateTime qdTime { get; set; }
        public int qdScore { get; set; }
    }
}
