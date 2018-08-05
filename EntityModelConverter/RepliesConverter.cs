using System;
using CatsDataEntity;
using CatsPrj.Model;
namespace EntityModelConverter
{
    public class RepliesConverter
    {
		public static RepliesModel repliesEntityToModel(CatsDataEntity.UserReply orig )
		{
			RepliesModel des = new RepliesModel();
			des.avantarUrl = orig.avantarUrl;
			des.nickName = orig.nickName;
			des.postsId = orig.postsId;
			des.replyId = orig.replyId;
			des.replyContent = orig.replyContent;
			des.replyLoved = orig.replyLoved;
			des.replyMaker = orig.replyMaker;
			des.lovedCount = orig.lovedCount;
            des.afterReplyDate = orig.replyDate.GetValueOrDefault();
            des.replyToUser = orig.replyToUser??"";
            des.isAdmin = orig.isAdmin;
			return des;
		}
    }
}
