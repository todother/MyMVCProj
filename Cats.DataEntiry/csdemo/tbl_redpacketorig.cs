using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_redpacketorig
    {
        public string packetId { get; set; }
        public int packetType { get; set; }
        public DateTime genDate { get; set; }
        public int packetPeriod { get; set; }
        public double baseline { get; set; }
        public double packetAmt { get; set; }
        public int packetCount { get; set; }
        public int canApplyTimes { get; set; }
    }
}


