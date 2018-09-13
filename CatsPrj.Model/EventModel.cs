using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model
{
    public class EventModel
    {
        public string eventId { get; set; }
        public string imagePath { get; set; }
        public DateTime eventTime { get; set; }
        public int eventIndex { get; set; }
    }
}
