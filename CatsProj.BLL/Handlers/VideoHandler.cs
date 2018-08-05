using System;
using System.IO;
using System.Web;
using CatsProj.Tools;
using CatsProj.DAL.Providers;
using CatsDataEntity;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;

namespace CatsProj.BLL.Handlers
{
    public class VideoHandler
    {

        public string generateVideo(double startTime, double endTime, HttpPostedFileWrapper video, string openId, int height, int width)
        {

            string extension = new FileInfo(video.FileName).Extension;
            string videoId = Guid.NewGuid().ToString();
            string tempId = Guid.NewGuid().ToString();
            string sthTemp = Guid.NewGuid().ToString();
            string filePathfull = HttpContext.Current.Server.MapPath("~/video/" + videoId + extension);
            string tempPathfull = HttpContext.Current.Server.MapPath("~/video/" + tempId + extension);
            string scdPathFull = HttpContext.Current.Server.MapPath("~/video/" + sthTemp + extension);
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/video")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/video"));
            }
            using (RunProcess process = new RunProcess())
            {
                video.SaveAs(tempPathfull);
                UserProvider provider = new UserProvider();
                tbl_user user = provider.getUserInfo(openId);
                string cmd = "-ss " + string.Format("{0:F}", startTime<=3?0: startTime) + " -t " + string.Format("{0:F}", (endTime - startTime)>=10?10: (endTime - startTime)) + " -i " + tempPathfull + " -vcodec copy -acodec copy " + filePathfull + " -y";
                process.RunCMDVedio(cmd);
            }
            generateSimpPic(filePathfull, height, width, sthTemp);
            int waterMarkHeight = Math.Max(height, width) / 9;

            addWaterMarkImage(filePathfull, scdPathFull, waterMarkHeight, waterMarkHeight);
            /*using (RunProcess process = new RunProcess())
            {
                string logoFile = HttpContext.Current.Server.MapPath("~/Logo/LogoPNG.png");
                string cmd = "-i " + filePathfull + " -i " + logoFile + " -filter_complex \"overlay = 10:10\" -acodec copy -y " + scdPathFull;
                string result = process.RunCMDBKVedio(cmd);
                
            }*/
            File.Delete(tempPathfull);
            File.Delete(filePathfull);
            return sthTemp;
        }

        public void addWaterMarkImage(string oriPath, string desPath, int width, int height)
        {
            using (RunProcess process = new RunProcess())
            {
                string logoFile = HttpContext.Current.Server.MapPath("~/Logo/logoGIF.gif");

                string resizeLogo = logoFile.Replace("logoGIF", Guid.NewGuid().ToString());
                File.Copy(logoFile, resizeLogo);
                using (var collection = new MagickImageCollection(new FileInfo(resizeLogo)))
                {
                    collection.Coalesce();
                    foreach (var image in collection)
                    {
                        image.Resize(width, height);
                    }
                    collection.Write(resizeLogo);
                }

                //PicsHandler.ResizeImage(Image.FromFile(logoFile),
                string cmd = "-i " + oriPath + " -i " + resizeLogo + " -filter_complex \"overlay = 10:10\" -acodec copy -y " + desPath;
                process.RunCMDBKVedio(cmd);
                File.Delete(resizeLogo);
                //Thread.Sleep(3000);
            }
        }

        public void saveFullVideo(string openId, HttpPostedFileWrapper video, int height, int width, double latitude, double longitude, string location, string postsContent,int ifOfficial)
        {
            string extension = new FileInfo(video.FileName).Extension;
            string videoId = Guid.NewGuid().ToString();

            string sthTemp = Guid.NewGuid().ToString();
            string filePathfull = HttpContext.Current.Server.MapPath("~/video/" + videoId + extension);
            string tempPathfull = HttpContext.Current.Server.MapPath("~/video/" + sthTemp + extension);
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/video")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/video"));
            }
            video.SaveAs(filePathfull);
            string simPicPath = "";

            RunProcess process = new RunProcess();
            string logoFile = HttpContext.Current.Server.MapPath("~/Logo/LogoPNG.png");
            //string cmd = "-i " + filePathfull + " -i " + logoFile + " -filter_complex \"overlay = 10:10\" -acodec copy -y " + tempPathfull;
            //string result = process.RunCMDBKVedio(cmd);


            simPicPath = generateSimpPic(filePathfull, height, width, sthTemp);
            int waterMarkHeight = Math.Max(height, width) / 9;

            addWaterMarkImage(filePathfull, tempPathfull, waterMarkHeight, waterMarkHeight);
            string postsType = "V";
            PostsHandler handler = new PostsHandler();
            string postsId = Guid.NewGuid().ToString();
            handler.savePosts(openId, postsContent, 1, postsId, latitude, longitude, location, postsType,ifOfficial);
            PicsHandler picsHandler = new PicsHandler();
            picsHandler.saveVideoPics(postsId, openId, "/video/" + sthTemp + extension, "/images/simple/" + sthTemp + ".jpg", sthTemp);
            File.Delete(filePathfull);
        }

        public void saveProcessedVideo(string openId, string videoId, double latitude, double longitude, string location, string postsContent, string extension,int ifOfficial)
        {
            PostsHandler handler = new PostsHandler();
            string postsId = Guid.NewGuid().ToString();
            handler.savePosts(openId, postsContent, 1, postsId, latitude, longitude, location, "V",ifOfficial);
            string filePathfull = HttpContext.Current.Server.MapPath("~/video/" + videoId + extension);
            string simPicPath = HttpContext.Current.Server.MapPath("~/images/simple/" + videoId + ".jpg");
            PicsHandler picsHandler = new PicsHandler();
            picsHandler.saveVideoPics(postsId, openId, "/video/" + videoId + extension, "/images/simple/" + videoId + ".jpg", videoId);
        }

        public string generateSimpPic(string videoPath, int height, int width, string videoId = "")
        {
            using (RunProcess process = new RunProcess())
            {
                FileInfo info = new FileInfo(videoPath);
                string[] fileNameSplit = info.FullName.Split('\\');

                string fileName = fileNameSplit[fileNameSplit.Length - 1].Replace(info.Extension, "");
                if (videoId != "")
                {
                    fileName = videoId;
                }
                string simPicPath = HttpContext.Current.Server.MapPath("~/images/simple/" + fileName + ".jpg");
                string cmd = "-i " + videoPath + " -f image2 -ss 0 -vframes 1 -s " + width.ToString() + "x" + height.ToString() + " " + simPicPath + " -y";
                process.RunCMDVedio(cmd);
                return simPicPath;
            }
        }


    }
}
