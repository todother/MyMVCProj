using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_event
    {
        public string eventId { get; set; }
        public string imagePath { get; set; }
        public DateTime eventTime { get; set; }
        public int eventIndex { get; set; }
    }
}
