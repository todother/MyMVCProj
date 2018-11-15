using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_solveQuestion
    {
        public string solveId { get; set; }
        public string openId { get; set; }
        public string questionId { get; set; }
        public DateTime solveTime { get; set; }
    }
}
