using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_missionConfig
    {
        public int missionId { get; set; }
        public string missionDesc { get; set; }
        public string detailDesc { get; set; }
        public int score { get; set; }
        public int times { get; set; }
    }
}
