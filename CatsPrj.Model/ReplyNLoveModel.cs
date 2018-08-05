using System;
namespace CatsPrj.Model
{
    public class ReplyNLoveModel
    {
        public ReplyNLoveModel()
        {
        }
		public string postsId { get; set; }
        public long replyCount { get; set; }
        public long postsLoveCount { get; set; }
        //public long replyLoveCount{get;set;}
        public string picsSimpPath { get; set; }
		public bool read { get; set; }
    }
}
