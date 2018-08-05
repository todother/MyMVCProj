using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CatsProj.Tools
{
    public class RunProcess : IDisposable
    {
        public static string RunCMD(string cmd)
        {
            using (Process myPro = new Process())
            {
                //指定绝对路径  
                // myPro.StartInfo.FileName = @"I:\AppSolution\curl_pkg\curl-7.57.0-win64-mingw\bin\curl.exe";  
                //使用环境变量路径  
                //string enPath = Environment.GetEnvironmentVariable("CURL_HOME");

                myPro.StartInfo.FileName = WebConfigurationManager.AppSettings["CurlPath"];
                myPro.StartInfo.UseShellExecute = false;
                myPro.StartInfo.RedirectStandardInput = true;
                myPro.StartInfo.RedirectStandardOutput = true;
                myPro.StartInfo.RedirectStandardError = true;
                myPro.StartInfo.CreateNoWindow = true;
                myPro.StartInfo.Arguments = cmd; //指定参数  
                myPro.Start();
                myPro.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息  
                string output = myPro.StandardOutput.ReadToEnd();

                myPro.WaitForExit();
                myPro.Close();

                return output;
            }
        }
        void IDisposable.Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        public void RunCMDVedio(string cmd)
        {
            string output = string.Empty;

            //指定绝对路径  
            // myPro.StartInfo.FileName = @"I:\AppSolution\curl_pkg\curl-7.57.0-win64-mingw\bin\curl.exe";  
            //使用环境变量路径  
            //string enPath = Environment.GetEnvironmentVariable("CURL_HOME");

            //myPro.StartInfo.FileName = WebConfigurationManager.AppSettings["CurlPath"];
            //using (Process myPro = new Process())
            //{
            //    myPro.StartInfo.FileName = WebConfigurationManager.AppSettings["FFMPEGPath"];
            //    myPro.StartInfo.UseShellExecute = false;
            //    myPro.StartInfo.CreateNoWindow = true;
            //    myPro.StartInfo.Arguments = cmd;
            //    myPro.Start();
            //    myPro.StandardInput.AutoFlush = true;
            //    myPro.WaitForExit();
            //}
            Process.Start(WebConfigurationManager.AppSettings["FFMPEGPath"], cmd).WaitForExit();

        }

        public void RunCMDBKVedio(string cmd)
        {
            string output = string.Empty;

            //指定绝对路径  
            // myPro.StartInfo.FileName = @"I:\AppSolution\curl_pkg\curl-7.57.0-win64-mingw\bin\curl.exe";  
            //使用环境变量路径  
            //string enPath = Environment.GetEnvironmentVariable("CURL_HOME");

            ////myPro.StartInfo.FileName = WebConfigurationManager.AppSettings["CurlPath"];

            
            //    Process myPro = new Process();
            //    myPro.StartInfo.FileName = WebConfigurationManager.AppSettings["FFMPEGPathBK"];
            //    myPro.StartInfo.UseShellExecute = false;
            //    myPro.StartInfo.RedirectStandardInput = true;
            //    myPro.StartInfo.RedirectStandardOutput = true;
            //    myPro.StartInfo.RedirectStandardError = true;
            //    myPro.StartInfo.CreateNoWindow = true;
            //    myPro.StartInfo.Arguments = cmd;
            //    myPro.Start();
            //    myPro.StandardInput.AutoFlush = true;
            //    Thread.Sleep(5000);
            //    myPro.Dispose();
            //    myPro.Close();
            //    //myPro.WaitForExit();
            //    return output;
            Process.Start(WebConfigurationManager.AppSettings["FFMPEGPathBK"], cmd).WaitForExit();


        }

    }
}
