using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_dailyMission
    {
        public string dailyId { get; set; }
        public int missionId { get; set; }
        public int finished { get; set; }//0代表未完成1代表完成
        public DateTime missionDate { get; set; }
        public string openId { get; set; }
        public int received { get; set; }//0代表未领取1代表已领取
    }
}
