using System;
namespace Cats.DataEntiry
{
    public class tbl_userFollowed
    {
        public tbl_userFollowed()
        {
        }

		public string followId { get; set; }
		public string userId { get; set; }
		public string followedUser { get; set; }
		public DateTime followedTime { get; set; }
    }
}
