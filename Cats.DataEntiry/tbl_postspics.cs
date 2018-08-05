using System;
using System.Linq;
using System.Text;

namespace CatsDataEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class tbl_postspics
    {
           public tbl_postspics(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string picID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string postsID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string picPath {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int picIndex {get;set;}

		/// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string picSimpPath { get; set; }

    }
}
