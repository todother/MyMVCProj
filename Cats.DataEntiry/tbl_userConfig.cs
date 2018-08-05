using System;
namespace Cats.DataEntiry
{
    public class tbl_userConfig
    {
        public tbl_userConfig()
        {
        }
		public string userId { get; set; }
		public int byTime { get; set; }
		public int byViewed { get; set; }
		public int onlyLoved { get; set; }

        public int onlyVerify { get; set; }
        public int videoMuted { get; set; }
    }
}
