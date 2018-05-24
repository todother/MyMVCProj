using System;
using System.IO;
using System.Web;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using EntityModelConverter;
using CatsDataEntity;

namespace CatsProj.BLL.Handlers
{
    public class PicsHandler
    {
		public void savePics(HttpPostedFileWrapper name,  string postsId,int idx)
		{
			Guid id = Guid.NewGuid();
            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }
			string fiePath = "images/" + id.ToString() + ".jpg";
			name.SaveAs(fiePath);

			PicsProvider provider = new PicsProvider();
			PicsModel model = new PicsModel();
			model.picID = id.ToString();
			model.picIndex = idx;
			model.picPath = fiePath;
			model.postsID = postsId;
			provider.savePics(PicsConverter.picsModeltoEntity(model));

		}
    }
}
