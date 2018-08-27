using System;
using System.IO;
using System.Web;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using EntityModelConverter;
using CatsDataEntity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Text;
using CatsProj.Tools;
using Newtonsoft.Json;

namespace CatsProj.BLL.Handlers
{
    public class PicsHandler
    {
		public bool savePics(HttpPostedFileWrapper name,  string postsId,int idx)
		{
			try
			{

				Guid id = Guid.NewGuid();
				if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images")))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images"));
				}
				if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images/original")))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images/original"));
				}
				if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images/simple")))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images/simple"));
				}
                tbl_posts posts = new tbl_posts();
                posts = new PostsProvider().getPost(postsId);
                string openId = posts.postsMaker;
                tbl_user user = new UserProvider().getUserInfo(openId);
				string fiePath = "/images/original/" + id.ToString() + ".jpg";
                string waterMark = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("微信小程序 果Jio @" + user.nickName));
                var simpleImage = Image.FromStream(name.InputStream);
                int width = simpleImage.Width;
                int height = simpleImage.Height;
                int fontsize = height / 50 <= 10 ? 10 : height / 50;
                Image origImage = addWaterMark(name.InputStream, waterMark, 9, 0, fontsize);
                string origFilePath = HttpContext.Current.Server.MapPath("~/images/original/" + id.ToString() + ".jpg");
                origImage.Save(origFilePath);

				string simplePic = "/images/simple/" + id.ToString() + ".jpg";
				decimal rate;
                if(width>height)
				{
					height = 200;
					rate = Convert.ToDecimal(width) / Convert.ToDecimal(simpleImage.Height);
					width = Convert.ToInt32(height * rate);
				}
				else
				{
					width = 200;
					rate = Convert.ToDecimal(height) / Convert.ToDecimal(simpleImage.Width);
					height=Convert.ToInt32(width * rate);
				}

                
				Bitmap bmp = ResizeImage(simpleImage, width, height);
                string smpFilePath = HttpContext.Current.Server.MapPath("~/images/simple/" + id.ToString() + ".jpg");
                bmp.Save(smpFilePath);

                string returnValue = RunProcess.RunCMD(generateCMD(smpFilePath));
                ReturnValue value = JsonConvert.DeserializeObject<ReturnValue>(returnValue);
                if(value.errcode==87014)
                {
                    delPics(postsId,1,openId);
                    return false;
                }

                PicsProvider provider = new PicsProvider();
				PicsModel model = new PicsModel();
				model.picID = id.ToString();
				model.picIndex = idx;
				model.picPath = fiePath;
				model.postsID = postsId;
				model.picSimpPath = simplePic;
				provider.savePics(PicsConverter.picsModeltoEntity(model));
                return true;
			}
            catch(Exception e)
			{
				int i = 0;
                return false;
			}
		}

        public void saveVideoPics(string postsId,string openId,string videoPath,string simpPicPath,string videoId)
        {
            tbl_postspics pics = new tbl_postspics();
            pics.picID = videoId;
            pics.picIndex = 0;
            pics.picPath = videoPath;
            pics.picSimpPath = simpPicPath;
            pics.postsID = postsId;
            PicsProvider provider = new PicsProvider();
            provider.savePics(pics);
        }

        public string generateCMD(string filePath)
        {
            string token = new TokenProvider().getToken();
            string cmd =string.Format( "-F media=@{0} \"https://api.weixin.qq.com/wxa/img_sec_check?access_token={1}\"",filePath,token);
            return cmd;
        }

        private class ReturnValue
        {
            public long errcode { get; set; }
            public string errMsg { get; set; }
        }

		public static Bitmap ResizeImage(Image image, int width, int height)
        {
			try
			{
				var destRect = new Rectangle(0, 0, width, height);
				var destImage = new Bitmap(width, height);

				//destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

				using (var graphics = Graphics.FromImage(destImage))
				{
					graphics.CompositingMode = CompositingMode.SourceCopy;
					graphics.CompositingQuality = CompositingQuality.Default;
					graphics.InterpolationMode = InterpolationMode.Default;
					graphics.SmoothingMode = SmoothingMode.Default;
					graphics.PixelOffsetMode = PixelOffsetMode.Default;

					using (var wrapMode = new ImageAttributes())
					{
						wrapMode.SetWrapMode(WrapMode.TileFlipXY);
						graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
					}
				}

				return destImage;
			}
            catch(Exception e)
			{
				return new Bitmap(0,0);
			}
        }

		public void delPics(string postsId,int delType,string openId)
		{
			PostsProvider provider = new PostsProvider();
			List<tbl_postspics> postspics = new List<tbl_postspics>();
			postspics = provider.delPosts(postsId,delType,openId);

			foreach (var item in postspics)
			{
				if (File.Exists(HttpContext.Current.Server.MapPath(item.picPath)))
				{
					File.Delete(HttpContext.Current.Server.MapPath(item.picPath));
				}
				if (File.Exists(HttpContext.Current.Server.MapPath(item.picSimpPath)))
				{
					File.Delete(HttpContext.Current.Server.MapPath(item.picSimpPath));
				}
			}
		}


		/// <summary>
        /// 文字水印
        /// </summary>
        
        public static Image addWaterMark(Stream imgStream, string watermarkText, int watermarkStatus, int quality, int fontsize, string fontname = "微软雅黑")
        {
            
			Image img = Image.FromStream(imgStream);
            

            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (float)img.Width * (float).01;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
			return img;
        }

        public string drawContent(string origPicPath,string content,string madeBy)
        {
            Bitmap bmp = new Bitmap(origPicPath);
            int mode = 0;//0是横向排版，1是纵向排版
            int width = bmp.Width;
            float rpx = 0;
            rpx = bmp.Width / 750;
            int height = bmp.Height;
            if (width >= height)
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }
            int fontSize = 400 / 9;
            int border = 5;
            int curX = 0;int curY = 0;
            int curRow = 0;
            Bitmap contentPic = new Bitmap(750, 400);
            int i = 0;int j = 0;
            for(i=0;i<750;i++)
            {
                for(j=0;j<400;j++)
                {
                    contentPic.SetPixel(i, j, Color.Transparent);
                }
            }
            Graphics g = Graphics.FromImage(contentPic);
            Font drawFont = new Font("方正清刻本悦宋简体", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            content = "生活不止眼前的苟且，还有诗和远方的田野。";
            for (i = 0; i < content.Length; i++)
            {
                curY = curY + border;
                g.DrawString(content[i].ToString(), drawFont, new SolidBrush(Color.Black), curRow * (fontSize + border), curY+border);
                curY = curY + fontSize;
                if(curY>400-fontSize-border)
                {
                    curY = 0;
                    curRow = curRow + 1;
                }
            }
            contentPic.Save(@"E:/text.png", ImageFormat.Png);
            return "1";
        }
    }
}
