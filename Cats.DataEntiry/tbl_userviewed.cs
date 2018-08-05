using System;
using System.Linq;
using System.Text;

namespace CatsDataEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class tbl_userviewed
    {
        public tbl_userviewed()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string viewId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string userID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string postsID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? viewTime { get; set; }

    }
}