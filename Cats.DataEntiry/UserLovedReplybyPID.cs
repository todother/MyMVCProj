using System;
namespace Cats.DataEntiry
{
    public class UserLovedReplybyPID
    {
        public UserLovedReplybyPID()
        {
        }

		public string openId { get; set; }
		public string postsId { get; set; }
		public string replyId { get; set; }
		public long lovedCount { get; set; }
    }
}
