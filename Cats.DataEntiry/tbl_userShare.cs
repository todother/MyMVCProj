using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_userShare
    {
        public string shareId { get; set; }
        public string userId { get; set; }
        public string postsId { get; set; }
        public DateTime shareTime { get; set; }
    }
}
