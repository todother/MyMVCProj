using System;
using CatsDataEntity;
using CatsPrj.Model;
namespace EntityModelConverter
{
    public class PicsConverter
    {
        public static PicsModel picsEntityToModel(tbl_postsPics entity)
		{
			PicsModel model = new PicsModel();
			model.picID = entity.picID;
			model.picIndex = entity.picIndex;
			model.picPath = entity.picPath;
			model.postsID = entity.postsID;
			return model;
		}
        
        public static tbl_postsPics picsModeltoEntity(PicsModel model)
		{
			tbl_postsPics entity = new tbl_postsPics();
			entity.picID = model.picID;
			entity.picIndex = model.picIndex;
			entity.picPath = model.picPath;
			entity.postsID = model.postsID;
			return entity;
		}

    }
}
