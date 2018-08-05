using System;
namespace Cats.DataEntiry
{
    public class PostsPics
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
		public string openId { get; set; }

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
        public int? postsStatus { get; set; }

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

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public long? postsCollected { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public long? postsReported { get; set; }

		public string postsPics { get; set; }

		public string makerName { get; set; }

		public int picIndex { get; set; }

		public string picSimpPath { get; set; }

		public string picPath { get; set; }

		public string makerPhoto { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
        public string postsLocation { get; set; }
        public string postsType { get; set; }
        public int ifOfficial { get; set; }
    }
}
