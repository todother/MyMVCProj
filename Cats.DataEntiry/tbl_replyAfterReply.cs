using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_replyAfterReply
    {
        public string afterReplyId { get; set; }
        public string replyId { get; set; }
        public string replyContent { get; set; }
        public string replyMaker { get; set; }
        public DateTime replyDate { get; set; }
        public string replyToUser { get; set; }
    }
}
