using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_report
    {
        public string reportId { get; set; }
        public string postsId { get; set; }
        public string userId { get; set; }
        public string reportReason { get; set; }
        public DateTime reportTime { get; set; }
    }
}
