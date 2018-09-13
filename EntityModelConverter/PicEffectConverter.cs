using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class PicEffectConverter
    {
        public static PicEffectModel entityToModel(tbl_picEffect entity)
        {
            string folderPath = "/filter/";
            PicEffectModel model = new PicEffectModel();
            model.effectId = entity.effectId;
            model.effectName = entity.effectName;
            model.R = entity.R;
            model.G = entity.G;
            model.B = entity.B;
            model.Rrate = entity.Rrate;
            model.Grate = entity.Grate;
            model.Brate = entity.Brate;
            model.effectIndex = entity.effectIndex;
            model.effectsThumbNail = folderPath + entity.effectName + ".jpg";
            return model;
        }
    }
}
