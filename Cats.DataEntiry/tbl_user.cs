using System;
using System.Linq;
using System.Text;

namespace CatsDataEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class tbl_user
    {
           public tbl_user(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string openid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string nickName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string avantarUrl {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string country {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string province {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string city {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string gender {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? registerDate {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? lastLoginDate {get;set;}

		/// <summary>
        /// Desc:use this flag to show the status for user,1 means the user is disabled
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? userStatus { get; set; }

		public string language { get; set; }

		public DateTime? lastRefreshDate { get; set; }

		public DateTime? lastRefreshFans { get; set; }

        public string selfIntro { get; set; }

        public long totalScore { get; set; }
    }
}
