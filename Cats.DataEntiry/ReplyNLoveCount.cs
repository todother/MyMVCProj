using System;
namespace CatsProj.DataEntiry
{
    public class ReplyNLoveCount
    {
        public ReplyNLoveCount()
        {
        }

		public string postsId{get;set;}
		public long replyCount{get;set;}
		public long postsLoveCount{get;set;}
		//public long replyLoveCount{get;set;}
		public string picsSimpPath { get; set; }
		public bool read { get; set; }
        public DateTime replydate { get; set; }
        public int picindex { get; set; }
        public string postsmaker { get; set; }
        public string replyMaker { get; set; }
        public DateTime lovedTime { get; set; }
        public string userId { get; set; }
        public string replyToUser { get; set; }
    }
}
