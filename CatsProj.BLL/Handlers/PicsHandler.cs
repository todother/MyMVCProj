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
using Cats.DataEntiry;

namespace CatsProj.BLL.Handlers
{
    public class PicsHandler
    {
        public string savePics(HttpPostedFileWrapper name, string postsId, int idx)
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
                double ratio = origImage.Width / origImage.Height;
                string origFilePath = HttpContext.Current.Server.MapPath("~/images/original/" + id.ToString() + ".jpg");
                origImage.Save(origFilePath);
                //CompressImage(origFilePath, origFilePath);
                string simplePic = "/images/simple/" + id.ToString() + ".jpg";
                decimal rate;
                if (width > height)
                {
                    height = 200;
                    rate = Convert.ToDecimal(width) / Convert.ToDecimal(simpleImage.Height);
                    width = Convert.ToInt32(height * rate);
                }
                else
                {
                    width = 200;
                    rate = Convert.ToDecimal(height) / Convert.ToDecimal(simpleImage.Width);
                    height = Convert.ToInt32(width * rate);
                }


                Bitmap bmp = ResizeImage(simpleImage, width, height);
                string smpFilePath = HttpContext.Current.Server.MapPath("~/images/simple/" + id.ToString() + ".jpg");
                bmp.Save(smpFilePath);

                string returnValue = RunProcess.RunCMD(generateCMD(smpFilePath));
                ReturnValue value = JsonConvert.DeserializeObject<ReturnValue>(returnValue);
                if (value.errcode == 87014)
                {
                    delPics(postsId, 1, openId);
                    return "false";
                }

                PicsProvider provider = new PicsProvider();
                PicsModel model = new PicsModel();
                model.picID = id.ToString();
                model.picIndex = idx;
                model.picPath = fiePath;
                model.postsID = postsId;
                model.picSimpPath = simplePic;
                if (idx == 0)
                {
                    model.picsRate = Convert.ToDecimal(height) / Convert.ToDecimal(width);
                }
                provider.savePics(PicsConverter.picsModeltoEntity(model));
                return "true";
            }
            catch (Exception e)
            {
                int i = 0;
                return e.Message;
            }
        }

        public void saveVideoPics(string postsId, string openId, string videoPath, string simpPicPath, string videoId)
        {
            tbl_postspics pics = new tbl_postspics();
            pics.picID = videoId;
            pics.picIndex = 0;
            pics.picPath = videoPath;
            pics.picSimpPath = simpPicPath;
            decimal rate = 0;
            if (File.Exists(HttpContext.Current.Server.MapPath(simpPicPath)))
            {
                Image img = Bitmap.FromFile(HttpContext.Current.Server.MapPath(simpPicPath));
                rate = Convert.ToDecimal(img.Height) / Convert.ToDecimal(img.Width);
            }
            pics.postsID = postsId;
            pics.picsRate = rate;
            PicsProvider provider = new PicsProvider();
            provider.savePics(pics);
        }

        public string generateCMD(string filePath)
        {
            string token = new TokenProvider().getToken();
            string cmd = string.Format("-F media=@{0} \"https://api.weixin.qq.com/wxa/img_sec_check?access_token={1}\"", filePath, token);
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
            catch (Exception e)
            {
                return new Bitmap(0, 0);
            }
        }

        public void delPics(string postsId, int delType, string openId)
        {
            PostsProvider provider = new PostsProvider();
            List<tbl_postspics> postspics = new List<tbl_postspics>();
            postspics = provider.delPosts(postsId, delType, openId);

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

        public string drawContent(string origPicPath, string content, string madeBy)
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
            int curX = 0; int curY = 0;
            int curRow = 0;
            Bitmap contentPic = new Bitmap(750, 400);
            int i = 0; int j = 0;
            for (i = 0; i < 750; i++)
            {
                for (j = 0; j < 400; j++)
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
                g.DrawString(content[i].ToString(), drawFont, new SolidBrush(Color.Black), curRow * (fontSize + border), curY + border);
                curY = curY + fontSize;
                if (curY > 400 - fontSize - border)
                {
                    curY = 0;
                    curRow = curRow + 1;
                }
            }
            contentPic.Save(@"E:/text.png", ImageFormat.Png);
            return "1";
        }

        public void calcRate(string urlPath)
        {
            List<tbl_postspics> pics = new PicsProvider().getPics();
            foreach (var item in pics)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~" + item.picSimpPath)))
                {
                    Image img = Bitmap.FromFile(HttpContext.Current.Server.MapPath("~" + item.picSimpPath));
                    int width = img.Width;
                    int height = img.Height;
                    decimal rate = Convert.ToDecimal(height) / Convert.ToDecimal(width);
                    new PicsProvider().updateRate(item.picID, rate);
                }
            }
        }

        public List<PicEffectModel> getEffects()
        {
            PicsProvider provider = new PicsProvider();
            List<tbl_picEffect> effects = provider.getEffects();
            List<PicEffectModel> effectsModel = new List<PicEffectModel>();
            foreach (var item in effects)
            {
                effectsModel.Add(PicEffectConverter.entityToModel(item));
            }
            return effectsModel;
        }

        public void generateFilters()
        {
           List< PicEffectModel >filters = getEffects();
            string tempEffects = HttpContext.Current.Server.MapPath("~/filter/");
            Image origImg = Bitmap.FromFile(tempEffects+"orig.jpg");
            Bitmap origBmp = new Bitmap(origImg);
            int width = origImg.Width;
            int height = origImg.Height;

            decimal origRRate = 0;
            decimal origGRate = 0;
            decimal origBRate = 0;
            int currR = 0;
            int currG = 0;
            int currB = 0;
            
            string filePath = "";

            Bitmap destImg = new Bitmap(width, height);

            int i = 0; int j = 0;
            foreach (var filter in filters)
            {

                destImg = new Bitmap(width, height);
                    for (i = 0; i < width; i++)
                    {
                        for (j = 0; j < height; j++)
                        {
                            Color curColor = origBmp.GetPixel(i, j);
                            currR = Convert.ToInt32(Convert.ToDouble(curColor.R * Convert.ToDouble(1 - Convert.ToDouble(filter.Rrate) / 100) + filter.R * Convert.ToDouble(filter.Rrate) / 100));
                            currG = Convert.ToInt32(Convert.ToDouble(curColor.G * Convert.ToDouble(1 - Convert.ToDouble(filter.Grate) / 100) + filter.G * Convert.ToDouble(filter.Grate) / 100));
                            currB = Convert.ToInt32(Convert.ToDouble(curColor.B * Convert.ToDouble(1 - Convert.ToDouble(filter.Brate) / 100) + filter.B * Convert.ToDouble(filter.Brate) / 100));
                            destImg.SetPixel(i, j, Color.FromArgb(currR, currG, currB));
                        }
                    }
                destImg.Save(tempEffects + filter.effectName + ".jpg");
                destImg.Dispose();
            }
            //fileId = Guid.NewGuid().ToString();
            
            if (!Directory.Exists(tempEffects))
            {
                Directory.CreateDirectory(tempEffects);
            }
            //destImg.Save(filePath, ImageFormat.Jpeg);
            
            
            origImg.Dispose();
            origBmp.Dispose();
        }

        //public string generatePicEffects(HttpPostedFileWrapper file,int filterIdx,decimal hrate,decimal srate,decimal lrate)
        //{
        //    string fileName = Guid.NewGuid().ToString();
        //    string result= generateFilters(file, filterIdx, fileName);
        //    result = processEffects(file,hrate, srate, lrate, fileName,filterIdx);
        //    return result;
        //}

        //public string processEffects(HttpPostedFileWrapper file,decimal hrate, decimal srate, decimal lrate,string fileId,int filterIdx)
        //{

        //    string tempEffects = HttpContext.Current.Server.MapPath("~/tempEffects");
        //    string filePath = tempEffects + "/" + fileId + ".jpeg";
        //    Image source;
        //    if (filterIdx != 999)
        //    {
        //        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //        // 读取文件的 byte[]  
        //        byte[] bytes = new byte[fileStream.Length];
        //        fileStream.Read(bytes, 0, bytes.Length);
        //        fileStream.Close();
        //        // 把 byte[] 转换成 Stream  
        //        Stream stream = new MemoryStream(bytes);

        //        stream.Read(bytes, 0, bytes.Length);
        //        // 设置当前流的位置为流的开始  
        //        stream.Seek(0, SeekOrigin.Begin);

        //        MemoryStream mstream = null;
        //        mstream = new MemoryStream(bytes);
        //        source = Bitmap.FromStream(mstream);
        //        stream.Dispose();
        //        stream.Close();



        //    }
        //    else
        //    {
        //        source = Bitmap.FromStream(file.InputStream);
        //    }
        //    Bitmap sBmp = new Bitmap(source);
        //    int x = 0; int y = 0;
        //    int width = sBmp.Width;


        //    int height = sBmp.Height;
        //    Bitmap result = new Bitmap(width, height);
        //    for (x = 0; x < width; x++)
        //    {
        //        for (y = 0; y < height; y++)
        //        {
        //            Color curColor = sBmp.GetPixel(x, y);
        //            List<int> calcRes = new List<int>();
        //            int r = curColor.R;
        //            int g = curColor.G;
        //            int b = curColor.B;
        //            calcRes = changeDBD(r, g, b, srate);
        //            r = calcRes[0];
        //            g = calcRes[1];
        //            b = calcRes[2];
        //            calcRes = changeLight(r, g, b, lrate);
        //            r = calcRes[0];
        //            g = calcRes[1];
        //            b = calcRes[2];
        //            calcRes = changeBHD(r, g, b, hrate);
        //            r = calcRes[0];
        //            g = calcRes[1];
        //            b = calcRes[2];
        //            result.SetPixel(x, y, Color.FromArgb(r, g, b));

        //        }
        //    }

        //    if (!Directory.Exists(tempEffects))
        //    {
        //        Directory.CreateDirectory(tempEffects);
        //    }
        //    result.Save(filePath, ImageFormat.Jpeg);
        //    source.Dispose();

        //    sBmp.Dispose();
        //    result.Dispose();

        //    return fileId;
        //}



        //public void delTempPic(string fileId)
        //{
        //    string tempEffects = HttpContext.Current.Server.MapPath("~/tempEffects");
        //    string filePath = tempEffects + "/" + fileId + ".jpeg";
        //    File.Delete(filePath);
        //}

        //public List<int> changeDBD(int r, int g, int b, decimal percent)
        //{
        //    var avg = 128;
        //    List<int> result = new List<int>();
        //    var cr = 0; var cg = 0; var cb = 0;
        //    if (percent > 0)
        //    {
        //        cr = avg + Convert.ToInt32((r - avg) * 1 / (1 - percent));
        //        cg = avg + Convert.ToInt32((g - avg) * 1 / (1 - percent));
        //        cb = avg + Convert.ToInt32((b - avg) * 1 / (1 - percent));
        //    }
        //    else
        //    {
        //        cr = avg + Convert.ToInt32((r - avg) * (1 + percent));
        //        cg = avg + Convert.ToInt32((g - avg) * (1 + percent));
        //        cb = avg + Convert.ToInt32((b - avg) * (1 + percent));
        //    }
        //    result.Add(cr >= 255 ? 255 : cr <= 0 ? 0 : cr);
        //    result.Add(cg >= 255 ? 255 : cg <= 0 ? 0 : cg);
        //    result.Add(cb >= 255 ? 255 : cb <= 0 ? 0 : cb);
        //    return result;
        //}

        //public List<int> changeLight(int r, int g, int b, decimal percent)
        //{
        //    var cr = 0;
        //    var cg = 0;
        //    var cb = 0;
        //    if (percent > 0)
        //    {
        //        cr = Convert.ToInt32(Convert.ToDecimal(r * (1 - percent)) + Convert.ToDecimal(255 * percent));
        //        cg = Convert.ToInt32(Convert.ToDecimal(g * (1 - percent)) + Convert.ToDecimal(255 * percent));
        //        cb = Convert.ToInt32(Convert.ToDecimal(b * (1 - percent)) + Convert.ToDecimal(255 * percent));
        //    }
        //    else
        //    {
        //        cr = r + Convert.ToInt32(r * percent);
        //        cg = g + Convert.ToInt32(g * percent);
        //        cb = b + Convert.ToInt32(b * percent);
        //    }
        //    List<int> result = new List<int>();
        //    result.Add(cr >= 255 ? 255 : cr <= 0 ? 0 : cr);
        //    result.Add(cg >= 255 ? 255 : cg <= 0 ? 0 : cg);
        //    result.Add(cb >= 255 ? 255 : cb <= 0 ? 0 : cb);
        //    return result;
        //}

        //public List<int> changeBHD(int r, int g, int b, decimal percent)
        //{
        //    var max = Math.Max(r, Math.Max(g, b));
        //    var min = Math.Min(r, Math.Min(g, b));
        //    decimal d = Convert.ToDecimal( (max - min)) / 255M;
        //    var cr = r;
        //    var cg = g;
        //    var cb = b;
        //    if (d > 0)
        //    {
        //        decimal value = Convert.ToDecimal( (max + min)) / 255M;
        //        decimal L = Convert.ToDecimal( value) / 2M;
        //        decimal S = 0;
        //        if (L < 0.5M)
        //        {
        //            S = Convert.ToDecimal( d )/ value;
        //        }
        //        else
        //        {
        //            S =Convert.ToDecimal( d / (2 - value));
        //        }
        //        decimal alpha = 0;
        //        if (percent >= 0)
        //        {
        //            if (percent + S >= 1)
        //            {
        //                alpha = S;
        //            }
        //            else
        //            {
        //                alpha = 1 - percent;
        //            }
        //            alpha = Convert.ToDecimal( 1 / alpha - 1M);
        //            cr = r + Convert.ToInt32((r - L * 255) * alpha);
        //            cg = g + Convert.ToInt32((g - L * 255) * alpha);
        //            cb = b + Convert.ToInt32((b - L * 255) * alpha);
        //        }
        //        else
        //        {
        //            cr = Convert.ToInt32(L * 255) + Convert.ToInt32((r - L * 255) * (1 + alpha));
        //            cg = Convert.ToInt32(L * 255) + Convert.ToInt32((g - L * 255) * (1 + alpha));
        //            cb = Convert.ToInt32(L * 255) + Convert.ToInt32((b - L * 255) * (1 + alpha));
        //        }
        //    }
        //    List<int> result = new List<int>();
        //    result.Add(cr >= 255 ? 255 : cr <= 0 ? 0 : cr);
        //    result.Add(cg >= 255 ? 255 : cg <= 0 ? 0 : cg);
        //    result.Add(cb >= 255 ? 255 : cb <= 0 ? 0 : cb);
        //    return result;
        //}

        public static bool CompressImage(string sFile, string dFile, int flag = 90, int size = 300, bool sfsc = true)
        {
            FileStream fileStream = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]  
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream  
            Stream stream = new MemoryStream(bytes);

            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始  
            stream.Seek(0, SeekOrigin.Begin);

            MemoryStream mstream = null;
            mstream = new MemoryStream(bytes);
            Image iSource = Bitmap.FromStream(mstream);
            stream.Dispose();
            stream.Close();

            
            ImageFormat tFormat = iSource.RawFormat;
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                return true;
            }

            int dHeight = iSource.Height / 2;
            int dWidth = iSource.Width / 2;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        flag = flag - 10;
                        CompressImage(sFile, dFile, flag, size, false);
                    }
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
    }
}
