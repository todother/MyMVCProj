using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MyMVCProj.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        public string checkWXValid(string signature,string timestamp,string nonce,string echostr)
        {
            string token = "guojiowxapp";
            signature = HttpContext.Request.Form["signature"];
            timestamp = HttpContext.Request.Form["timestamp"];
            nonce = HttpContext.Request.Form["nonce"];
            echostr = HttpContext.Request.Form["echostr"];
            Dictionary<string, string> fromWX = new Dictionary<string, string>();
            fromWX.Add("token", token);
            fromWX.Add("timestamp", timestamp);
            fromWX.Add("nonce", nonce);
            var item = fromWX.OrderBy(o => o.Value, StringComparer.Ordinal);
            string result = string.Empty;
            foreach(var i in item)
            {
                result += i.Value;
            }
            string sha1 = SHA1(result, Encoding.UTF8);
            if (sha1 == signature)
            {
                return echostr;
            }
            else
            {
                return "signature:" + signature + "   timestamp:" + timestamp + "   nonce:" + nonce + "   echostr:" + echostr + "   sha1" + sha1;
            }
        }

        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
    }
}
