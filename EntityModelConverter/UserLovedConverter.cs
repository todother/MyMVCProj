using System;
using CatsDataEntity;
using CatsPrj.Model;

namespace EntityModelConverter
{
    public class UserLovedConverter
    {
        public UserLovedConverter()
        {
        }

        public static tbl_userloved userlovedModelToEntity(UserLovedModel model)
		{
			tbl_userloved entity = new tbl_userloved();
			entity.lovedID = model.lovedId;
			entity.lovedTime = model.lovedDate;
			entity.postsID = model.postsId;
			entity.userID = model.userId;
            entity.loveStatus = model.loveStatus;
			return entity;
		}

        public static UserLovedModel userLovedEntityToModel(tbl_userloved entity)
		{
			UserLovedModel model = new UserLovedModel();
			model.lovedId = entity.lovedID;
			model.lovedDate = entity.lovedTime.GetValueOrDefault();
			model.postsId = entity.postsID;
			model.userId = entity.userID;
            model.loveStatus = entity.loveStatus;
			return model;
		}


    }
}
