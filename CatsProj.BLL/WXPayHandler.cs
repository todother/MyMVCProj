using Cats.DataEntiry;
using CatsProj.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CatsProj.BLL
{
    public class WXPayHandler
    {
        public string RaiseWXPay(string DBPath,string payBody,string wxopenId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            WXPayProvider provider = new WXPayProvider();
            tbl_wxpay shopInfo = provider.getShopInfo(DBPath);
            string appid = shopInfo.appid;
            string mch_id = shopInfo.mch_id;
            string nonce_str = Guid.NewGuid().ToString("N");
            string key = shopInfo.wxkey;
            string body = payBody;
            string out_trade_no = Guid.NewGuid().ToString("N");
            string total_fee = "1";
            string spbill_create_ip = "223.64.148.206";
            string notify_url = "https://www.guojio.com";
            string trade_type = "JSAPI";
            string openId = wxopenId;
            param.Add("appid", appid);
            param.Add("mch_id", mch_id);
            param.Add("nonce_str", nonce_str);
            //param.Add("key", key);
            param.Add("body", body);
            param.Add("out_trade_no", out_trade_no);
            param.Add("total_fee", total_fee);
            param.Add("spbill_create_ip", spbill_create_ip);
            param.Add("notify_url", notify_url);
            param.Add("trade_type", trade_type);
            param.Add("openid", openId);


            var vDic = (from objDic in param orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            string str1 = string.Empty;
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
                str1 = str.ToString();
            }

            str.Append("key=" + key);
            string result = str.ToString();
            string sign = GetMD5(result,"utf-8"); // MD5.Create(result).ToString().ToUpper();
            string xmlText = str1 + "sign=" + sign;

            XElement el = new XElement("root", param.Select(kv => new XElement(kv.Key, kv.Value)));
            el.Add(new XElement("sign", sign));
            int i = 0;

            string postback= PostXML(el).ToString();
            XmlDocument document = new XmlDocument();
            document.LoadXml(postback);
            string prepay_id = document.SelectSingleNode("/xml/prepay_id").InnerText;
            return prepay_id;

        }

        public Dictionary<string,string> generateMD5forPay(string prepayid,string DBPath)
        {
            WXPayProvider provider = new WXPayProvider();
            tbl_wxpay shopInfo = provider.getShopInfo(DBPath);
            string appId= shopInfo.appid;
            string timeStamp = ConvertDateTimeToInt(DateTime.Now).ToString();
            string nonceStr = Guid.NewGuid().ToString("N");
            string package = "prepay_id=" + prepayid;
            string signType = "MD5";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appId", appId);
            param.Add("timeStamp", timeStamp);
            param.Add("nonceStr", nonceStr);
            param.Add("package", package);
            param.Add("signType", signType);
            string key = shopInfo.wxkey;
            var vDic = (from objDic in param orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            string str1 = string.Empty;
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
                str1 = str.ToString();
            }

            str.Append("key=" + key);
            string result = str.ToString();
            string sign = GetMD5(result, "utf-8");
            Dictionary<string, string> paraList= new Dictionary<string, string>();
            paraList.Add("timeStamp", timeStamp);
            paraList.Add("nonceStr", nonceStr);
            paraList.Add("sign", sign);
            return paraList;
        }

        public long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }


        public  string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public string PostXML(XElement xml)
        {
            string WebUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";//换成接收方的URL
            string result= RequestUrl(WebUrl, xml.ToString());
            return result;
        }

        public string RequestUrl(string url, string data)//发送方法
        {

            var request = WebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "text/xml";
            request.Headers.Add("charset:utf-8");
            var encoding = Encoding.GetEncoding("utf-8");

            if (data != null)
            {
                byte[] buffer = encoding.GetBytes(data);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }
            else
            {
                //request.ContentLength = 0;
            }

            using (HttpWebResponse wr = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(wr.GetResponseStream(), encoding))
                {
                    string sth= reader.ReadToEnd();
                    return sth;
                }
            }
        }
    }
}
