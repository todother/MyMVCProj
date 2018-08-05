using System;
using System.Linq;
using System.Text;

namespace CatsDataEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class tbl_userloved
    {
           public tbl_userloved(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string lovedID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string userID {get;set;}

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
           public DateTime? lovedTime {get;set;}

        public int loveStatus { get; set; }
    }
}
