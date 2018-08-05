using System;
namespace CatsPrj.Model
{
    public class UserViewedModel
    {
        public UserViewedModel()
        {
        }
		public string viewedId { get; set; }
        public string userId { get; set; }
        public string postsId { get; set; }
        public DateTime viewedDate { get; set; }
    }
}
