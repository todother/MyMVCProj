using System;
namespace Cats.DataEntiry
{
    public class tbl_userReplyLoved
    {
        public tbl_userReplyLoved()
        {
        }

		public string lovedId { get; set; }
		public string openId { get; set; }
		public string replyId { get; set; }
		public DateTime lovedTime { get; set; }
    }
}
