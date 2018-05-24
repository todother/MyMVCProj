using System;
using CatsDataEntity;
using CatsPrj.Model;

namespace EntityModelConverter
{
    public class UserConverter
    {
		
            public static tbl_user userModelToEntity(UserModel model)
            {
                tbl_user userEntiry = new tbl_user();
                userEntiry.openid = model.openId;
                userEntiry.avantarUrl = model.avatarUrl;
                userEntiry.city = model.city;
                userEntiry.country = model.country;
                userEntiry.gender = model.gender;
                userEntiry.lastLoginDate = DateTime.Now;
                userEntiry.nickName = model.nickName;
                userEntiry.province = model.province;
			userEntiry.language = model.language;
                return userEntiry;
            }

        public static UserModel userEntityToModel(tbl_user entity)
		{
			UserModel model = new UserModel();
			model.avatarUrl = entity.avantarUrl;
			model.city = entity.city;
			model.country = entity.country;
			model.gender = entity.gender;
			model.nickName = entity.nickName;
			model.openId = entity.openid;
			model.language = entity.language;
			model.province = entity.province;
			return model;
		}
     }
    
}
