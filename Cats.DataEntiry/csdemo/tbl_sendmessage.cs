using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_sendmessage
    {
        public string messageId { get; set; }
        public string usedId { get; set; }//formId or prepayId
        public int idtype { get; set; }//0 formId 1 prepayId
        public int usedTimes { get; set; }//limit formid 1   prepayid 3
        public DateTime genTime { get; set; }
        public string openId { get; set; }
    }
}
