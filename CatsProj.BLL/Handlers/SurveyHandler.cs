
using System;
using System.Collections.Generic;
using Cats.DataEntiry;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using EntityModelConverter;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Web;
using System.Web.Script.Serialization;
using CatsDataEntity;

namespace CatsProj.BLL.Handlers
{
    public class SurveyHandler
    {
        public List<RobotChatModel> getChatModels()
        {
            List<tbl_robot> chats = new SurveyProvider().getChatContent();
            List<RobotChatModel> result = new List<RobotChatModel>();
            foreach(var item in chats)
            {
                result.Add(RobotChatConverter.convertEntityToModel(item));
            }
            return result;
        }

        public List<QuestionModel> getQustions()
        {
            List<tbl_surveyQuestion> questions = new SurveyProvider().getQustions();
            List<QuestionModel> result = new List<QuestionModel>();
            foreach(var item in questions)
            {
                result.Add(QuestionConverter.convertEntityToModel(item));
            }
            return result;
        }

        public string calcResult(string result)
        {
            int length = result.Length;
            result = result.Substring(0, result.Length - 1);
            string[] ansList = result.Split(',');
            int i = 0;
            //List<tbl_surveyanalysis> analysis = new SurveyProvider().getSurveyAnalysis();
            int HD = 0;int CM = 0;int YG = 0;int NR = 0;int LJ = 0;
            for(i=0;i< ansList.Length; i++)
            {
                tbl_surveyanalysis currAnalysis = new SurveyProvider().getSurveyAnalysis(i+1,Convert.ToInt32(ansList[i]));
                HD += currAnalysis.HD;
                CM += currAnalysis.CM;
                YG += currAnalysis.YG;
                NR += currAnalysis.NR;
                LJ += currAnalysis.LJ;
            }
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("HD", HD);
            dict.Add("CM", CM);
            dict.Add("YG", YG);
            dict.Add("NR", NR);
            dict.Add("LJ", LJ);
            //dict = BubbleSort(dict);

            var dicSort = from objDic in dict orderby objDic.Value descending select objDic;

            List<string> prop = new List<string>();
            i = 0;
            foreach(var item in dicSort)
            {
                prop.Add(item.Key);
                i = i + 1;
                if (i == 2)
                {
                    break;
                }
            }
            return prop[0] + ";" + prop[1];
        }

        public void getSurveyQRCode()
        {

            string token = new TokenProvider().getToken();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/wxa/getwxacode?access_token=" + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //long shareId = new PostsProvider().saveShareCode(postsId, openId);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //string json = "{\"scene\":\"id=" + shareId + "\",\"page\":\"\"}";
                var obj = new { path = "pages/serveyForm/serveyForm", width=430 };
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
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/surveyCode")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/surveyCode"));
            }
            qrcode.Save(HttpContext.Current.Server.MapPath("~/surveyCode/" + "survey" + ".jpeg"));
            
        }

        public Dictionary<string, int> BubbleSort(Dictionary<string, int> data)
        {
            List<string> names = new List<string>();
            names.Add("HD");
            names.Add("CM");
            names.Add("YG");
            names.Add("NR");
            names.Add("LJ");
            for (int i = 0; i < data.Count - 1; i++)
            {
                for (int j = data.Count - 1; j > i; j--)
                {
                    if (data[names[j]] > data[names[j - 1]])
                    {
                        data[names[j]] = data[names[j]] + data[names[j - 1]];
                        data[names[j - 1]] = data[names[j]] - data[names[j - 1]];
                        data[names[j]] = data[names[j]] - data[names[j - 1]];
                    }
                }
            }
            return data;
        }

        public List<string> getDescs(string property,string openId)
        {
            List<string> properties = property.Split(';').ToList();
            List<tbl_commondesc> descs = new List<tbl_commondesc>();
            SurveyProvider provider = new SurveyProvider();
            int i = 0;
            for (i = 0; i < properties.Count; i++)
            {
                descs = provider.getDesc(descs, 1, properties[i],openId);
            }
            descs = provider.getDesc(descs, 2, "TY",openId);
            List<string> result = new List<string>();
            foreach(var item in descs)
            {
                result.Add(item.description);
            }
            return result;
        }

        public long saveTCMQuestion(string dogs, string startPoint, int width, int height, string openId)
        {
            List<List<string>> dogPoints = new JavaScriptSerializer().Deserialize<List<List<string>>>(dogs);
            List<string> start = new JavaScriptSerializer().Deserialize<List<string>>(startPoint);
            long questionId = new SurveyProvider().saveTCMQuestion(dogPoints, start, width, height, openId);

            string token = new TokenProvider().getToken();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //long shareId = new PostsProvider().saveShareCode(postsId, openId);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //string json = "{\"scene\":\"id=" + shareId + "\",\"page\":\"\"}";
                var obj = new { page = "pages/solveQuestion/solveQuestion", scene="qId="+questionId, width = 430 };
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
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/tcmQuestion")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/tcmQuestion"));
            }
            qrcode.Save(HttpContext.Current.Server.MapPath("~/tcmQuestion/" + questionId + ".jpeg"));

            return questionId;
        }

        public long getRandQuestion()
        {
            return new SurveyProvider().getRandTCM();
        }

        public List<List<TCMContent>> generateMatrix(long qId)
        {
            SurveyProvider provider = new SurveyProvider();
            tbl_tcmQuestion question = provider.getTCMQuestion(qId);
            int x = question.height;
            int y = question.width;

            List<List<TCMContent>> matrix = new List<List<TCMContent>>();
            int i = 0;int j = 0;
            for (i = 0; i < x + 2; i++)
            {
                var sth = new List<TCMContent>();
                matrix.Add(sth);
                for (j = 0; j < y + 2; j++)
                {
                    if(i==0 || j==0 || i==x+1 || j == y + 1)
                    {
                        matrix[i].Add(new TCMContent { dog = false, start = false,value=0 });
                    }
                    else
                    {
                        matrix[i].Add(new TCMContent { dog = false, start = false,value=1 });
                    }
                    
                }
            }

            List<tbl_tcmPoints> points = provider.getTCMPoints(qId);
            foreach(var item in points)
            {
                if (item.pType == 1)
                {
                    matrix[item.pX][item.pY].start = true;
                    matrix[item.pX][item.pY].value = 0;
                }
                if (item.pType == 2)
                {
                    matrix[item.pX][item.pY].dog = true;
                    matrix[item.pX][item.pY].value = 0;
                }
            }
            return matrix;
        }

        public UserModel getUserByTCM(long qId)
        {
            SurveyProvider provider = new SurveyProvider();
            tbl_user user = provider.getUserByTCM(qId);
            return UserConverter.userEntityToModel(user);
        }

        public int getSolveCount(string openId,string qId)
        {
            return new SurveyProvider().getSuccessCount(openId, qId);
        }
    }
}
