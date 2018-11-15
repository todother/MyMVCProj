using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_tcmQuestion
    {
        public long questionId { get; set; }
        public string questionMaker { get; set; }
        public long questionAttended { get; set; }
        public long questionSuccessed { get; set; }
        public DateTime questionMakeDate { get; set; }

        public int width { get; set; }
        public int height { get; set; }
    }
}
