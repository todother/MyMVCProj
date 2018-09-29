using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class PosterConverter
    {
        public static PosterModel convertEntityToModel(tbl_posterlayout entity)
        {
            PosterModel model = new PosterModel();
            model.posterCttPositionX = entity.posterCttPositionX;
            model.posterCttPositionY = entity.posterCttPositionY;
            model.posterId = entity.posterId;
            model.posterIdx = entity.posterIdx;
            model.posterImgPositionX = entity.posterImgPositionX;
            model.posterImgPositionY = entity.posterImgPositionY;
            model.posterPath = entity.posterPath;
            model.posterQRCodePositionX = entity.posterQRCodePositionX;
            model.posterQRCodePositionY = entity.posterQRCodePositionY;

            return model;
        }

        public static PosterContentModel converContentToModel(tbl_posterContent entity)
        {
            PosterContentModel model = new PosterContentModel();
            model.content1 = entity.content1;
            model.content2 = entity.content2;
            model.posterContentId = entity.posterContentId;
            return model;
        }
    }
}
