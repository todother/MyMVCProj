using System;
using System.Collections.Generic;
using Cats.DataEntiry;
using CatsDataEntity;
using CatsPrj.Model;
namespace EntityModelConverter
{
    public class UserReplyConverter
    {
		public static UserReplyModel replyEntityToModel(tbl_reply entity)
		{
			UserReplyModel model = new UserReplyModel();
			model.postsId = entity.postsID;
			model.replyId = entity.replyID;
			model.replyContent = entity.replyContent;
			model.replyDate = entity.replyDate.GetValueOrDefault();
			model.userId = entity.replyMaker;
			model.replyStatus = entity.replyStatus??0;
			return model;
		}

        public static tbl_reply replyModelToEntity(UserReplyModel model)
		{
			tbl_reply entity = new tbl_reply();
			entity.postsID = model.postsId;
			entity.replyContent = model.replyContent;
			entity.replyDate = model.replyDate;
			entity.replyID = model.replyId;
			entity.replyLoved = 0;
			entity.replyMaker = model.userId;
			entity.replyStatus = model.replyStatus;
			return entity;
		}

        public static List<tbl_replyAfterReply> afterReplyModelToEntity(List<ReplyAfterReplyModel> ori)
        {
            List<tbl_replyAfterReply> des = new List<tbl_replyAfterReply>();
            foreach (var item in ori)
            {
                tbl_replyAfterReply reply = new tbl_replyAfterReply();
                reply.afterReplyId = item.afterReplyId;
                reply.replyContent = item.replyContent;
                reply.replyDate = item.replyDate;
                reply.replyId = item.replyId;
                reply.replyMaker = item.replyMaker;
                des.Add(reply);
            }
            return des;
        }

        public static List<ReplyAfterReplyModel> afterReplyEntityToModel(List<tbl_replyAfterReply> ori)
        {
            List<ReplyAfterReplyModel> des = new List<ReplyAfterReplyModel>();
            foreach (var item in ori)
            {
                ReplyAfterReplyModel reply = new ReplyAfterReplyModel();
                reply.afterReplyId = item.afterReplyId;
                reply.replyContent = item.replyContent;
                reply.replyDate = item.replyDate;
                reply.replyId = item.replyId;
                reply.replyMaker = item.replyMaker;
                des.Add(reply);
            }
            return des;
        }
    }
}
