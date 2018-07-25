using System;
namespace CatsProj.DB
{
    public class ReplyNLoveCount
    {
        public ReplyNLoveCount()
        {
        }

		public string postsId{get;set;}
		public long replyCount{get;set;}
		public long postsLoveCount{get;set;}
		public long replyLoveCount{get;set;}
    }
}
