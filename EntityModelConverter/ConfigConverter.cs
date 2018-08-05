using System;
using Cats.DataEntiry;
using CatsPrj.Model;
namespace EntityModelConverter
{
    public class ConfigConverter
    {
        public ConfigConverter()
        {
        }

		public static ConfigModel configEntityToModel(tbl_userConfig ori)
		{
			ConfigModel des = new ConfigModel();
			des.byTime = ori.byTime;
			des.byViewed = ori.byViewed;
			des.onlyLoved = ori.onlyLoved;
			des.userId = ori.userId;
            des.onlyVerify = ori.onlyVerify;
            des.videoMuted = ori.videoMuted;
			return des;
		}

		public static tbl_userConfig configModelToEntity(ConfigModel ori)
        {
			tbl_userConfig des = new tbl_userConfig();
            des.byTime = ori.byTime;
            des.byViewed = ori.byViewed;
            des.onlyLoved = ori.onlyLoved;
            des.userId = ori.userId;
            des.onlyVerify = ori.onlyVerify;
            des.videoMuted = ori.videoMuted;
            return des;
        }
    }
}
