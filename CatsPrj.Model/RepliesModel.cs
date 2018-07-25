using System;
namespace CatsPrj.Model
{
    public class RepliedModel
    {
		public string replyMaker { get; set; }
        public string nickName { get; set; }
        public string avantarUrl { get; set; }
        public string replyId { get; set; }
        public string replyContent { get; set; }
        public bool replyLoved { get; set; }
        public string postsId { get; set; }
    }
}
