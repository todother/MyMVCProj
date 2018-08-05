using System;
namespace CatsPrj.Model
{
	public class UserReplyModel
	{
		public string replyId { get; set; }
		public string postsId { get; set; }
		public string userId { get; set; }
		public DateTime replyDate { get; set; }
		public string replyContent { get; set; }
		public int replyStatus { get; set; }
    }
}
