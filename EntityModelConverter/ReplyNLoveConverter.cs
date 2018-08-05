using System;
using CatsPrj.Model;
using CatsProj.DataEntiry;

namespace EntityModelConverter
{
    public class ReplyNLoveConverter
    {
        public ReplyNLoveConverter()
        {
        }
		public static ReplyNLoveModel entityToModel(ReplyNLoveCount entity )
		{
			ReplyNLoveModel model = new ReplyNLoveModel();
			model.picsSimpPath = entity.picsSimpPath;
			model.postsId = entity.postsId;
			model.postsLoveCount = entity.postsLoveCount;
			model.replyCount = entity.replyCount;
			model.read = false;
			return model;
		}
    }
}
