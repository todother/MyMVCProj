using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class ShareCodeConverter
    {
        public ShareCode entityToModel(tbl_sharecode entity)
        {
            ShareCode model = new ShareCode();
            model.openId = entity.openId;
            model.postsId = entity.postsId;
            model.shareId = entity.shareId;
            model.shareTime = entity.shareTime;
            return model;
        }
    }
}
