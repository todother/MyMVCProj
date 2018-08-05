using System;
namespace CatsPrj.Model
{
    public class ConfigModel
    {
        public ConfigModel()
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
