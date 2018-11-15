using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class DailyMission
    {
        public string dailyId { get; set; }
        public int missionId { get; set; }
        public string missionDesc { get; set; }
        public string detailDesc { get; set; }
        public bool finished { get; set; }
        public int score { get; set; }
        public int times { get; set; }
        public string openId { get; set; }
        public DateTime missionDate { get; set; }
        public int received { get; set; }
    }
}
