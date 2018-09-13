using System;
using CatsDataEntity;
using CatsPrj.Model;
namespace EntityModelConverter
{
    public class PicsConverter
    {
        public static PicsModel picsEntityToModel(tbl_postspics entity)
		{
			PicsModel model = new PicsModel();
			model.picID = entity.picID;
			model.picIndex = entity.picIndex;
			model.picPath = entity.picPath;
			model.postsID = entity.postsID;
			model.picSimpPath = entity.picSimpPath;
            model.picsRate = entity.picsRate;
			return model;
		}
        
        public static tbl_postspics picsModeltoEntity(PicsModel model)
		{
			tbl_postspics entity = new tbl_postspics();
			entity.picID = model.picID;
			entity.picIndex = model.picIndex;
			entity.picPath = model.picPath;
			entity.postsID = model.postsID;
			entity.picSimpPath = model.picSimpPath;
            entity.picsRate = model.picsRate;
			return entity;
		}

    }
}
