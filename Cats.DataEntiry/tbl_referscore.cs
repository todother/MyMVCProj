using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_referscore
    {
        public long referId { get; set; }
        public string openId { get; set; }
        public string referer { get; set; }
        public string missionId { get; set; }
        public int score { get; set; }
        public DateTime missionDate { get; set; }
    }
}
