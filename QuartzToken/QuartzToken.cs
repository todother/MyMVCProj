using CatsProj.DAL.Providers;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuartzToken
{
    public class QuartzToken : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string appid = "wx279f067da507d202";
            string secret = "12f53ae598e2dedea19849baa602f6cd";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.weixin.qq.com/cgi-bin/token?appid=" + appid + "&secret=" + secret + "&grant_type=client_credential");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            TokenTemplate template = JsonConvert.DeserializeObject<TokenTemplate>(retXml);
            string token = template.access_token;
            TokenProvider provider = new TokenProvider();
            provider.updateToken(token);
        }

        private class TokenTemplate
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
        }
    }

   
}
