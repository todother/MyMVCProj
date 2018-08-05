using System;
namespace CatsPrj.Model
{
    public class PostsModel
    {
		/// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string postsID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string postsContent { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string postsMaker { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? postsMakeDate { get; set; }

        

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? postsPicCount { get; set; }


        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public long? postsReaded { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public long? postsLoved { get; set; }

		public string postsPics { get; set; }
       
		public string makerName { get; set; }
		public string makerID { get; set; }

		public string picsSimpPath { get; set; }

		public string picsPath { get; set; }

		public string whenPosts { get; set; }

		public string makerPhoto { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
        public string postsLocation { get; set; }
        public string postsType { get; set; }
        public int postsStatus { get; set; }
        public int ifOfficial { get; set; }
    }
}
