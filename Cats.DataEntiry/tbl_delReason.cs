using System;
namespace Cats.DataEntiry
{
    public class tbl_delReason
    {
        public tbl_delReason()
        {
        }

		public string delId { get; set; }
		public string delContent { get; set; }
		public string delType { get; set; }
		public DateTime delTime { get; set; }
		public string delUser { get; set; }
		public string delOpenId { get; set; }
    }
}
