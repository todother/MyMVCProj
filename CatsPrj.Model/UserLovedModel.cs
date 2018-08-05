using System;
namespace CatsPrj.Model
{
    public class UserLovedModel
    {
        public UserLovedModel()
        {
        }
		public string lovedId { get; set; }
		public string userId { get; set; }
		public string postsId { get; set; }
		public DateTime lovedDate { get; set; }
        public int loveStatus { get; set; }
    }
}
