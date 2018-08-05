using Cats.DataEntiry;
using System;
using System.Collections.Generic;

namespace CatsDataEntity
{
	public class UserReply
	{
		public string replyMaker { get; set; }
		public string nickName { get; set; }
		public string avantarUrl { get; set; }
		public string replyId { get; set; }
		public string replyContent { get; set; }
		public bool replyLoved { get; set; }
		public string postsId { get; set; }
		public DateTime? replyDate { get; set; }
		public long lovedCount { get; set; }
        public string replyToUser { get; set; }
        public int replyType { get; set; }
        public bool isAdmin { get; set; }
    }
}
