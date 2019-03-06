using System;
using System.Collections.Generic;
using Cats.DataEntiry;
using CatsDataEntity;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using CatsProj.DataEntiry;
using EntityModelConverter;
using CatsProj.Tools;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;

namespace CatsProj.BLL.Handlers
{
    public class JFHandler
    {
        public int userQD(string openId)
        {
            int result = new JFProvider().userQD(openId);
            return result;
        }

        public int getQDTimes(string openId)
        {
            int times = new JFProvider().getCurWeekQDRecord(openId);
            return times;
        }

        public List<DailyMissionModel> getDailyMissions(string openId)
        {
            List<DailyMission> missions = new List<DailyMission>();
            JFProvider provider = new JFProvider();
            missions = provider.getUserDailyMission(openId);
            List<DailyMissionModel> result = new List<DailyMissionModel>();
            foreach(var item in missions)
            {
                result.Add(DailyMissionConverter.convertEntityToModel(item));
            }
            return result;
        }

        public int getDailyScore(string openId)
        {
            return new JFProvider().getDailyPoints(openId);
        }

        public List<DailyMissionModel> getPrevScore(string openId)
        {
            List<DailyMission> scores = new JFProvider().getPreviousScore(openId);
            List<DailyMissionModel> result = new List<DailyMissionModel>();
            int i = 0;
            for (i = 0; i < 7; i++)
            {
                DailyMissionModel model = new DailyMissionModel();
                model.missionDate = DateTime.Now.AddDays(-i).Date;
                result.Add(model);
            }

            foreach(var item in scores)
            {
                result.Find(o => o.missionDate.Date == item.missionDate.Date).score = item.score;
            }
            return result;
        }

        public List<DailyMissionModel> getPrevScoreRefer(string openId)
        {
            List<DailyMission> scores = new JFProvider().getPrevScoreRefer(openId);
            List<DailyMissionModel> result = new List<DailyMissionModel>();
            foreach (var item in scores)
            {
                result.Add(DailyMissionConverter.convertEntityToModel(item));
            }
            return result;
        }

        public void generateThkPoster()
        {
            string token = new TokenProvider().getToken();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/wxa/getwxacode?access_token=" + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //long shareId = new PostsProvider().saveShareCode(postsId, openId);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //string json = "{\"scene\":\"id=" + shareId + "\",\"page\":\"\"}";
                var obj = new { path = "pages/thanksgiving/thanksgiving" };
                string json = JsonConvert.SerializeObject(obj);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    
            //    return result;
            //}
            Image qrcode = Image.FromStream(httpResponse.GetResponseStream());
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/thks")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/thks"));
            }
            qrcode.Save(HttpContext.Current.Server.MapPath("~/thks/ poster.jpeg"));
        }
    }
}
