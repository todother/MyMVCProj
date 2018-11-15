using System;
using System.Data;
using System.Data.OleDb;
using CatsDataEntity;
using System.IO;
using System.Web;
using CatsProj.DAL.Providers;
using System.Web.Configuration;
using Cats.DataEntiry;
using System.Collections.Generic;

namespace CatsProj.BLL.Handlers
{
    public class RobotHandler
    {
        public void addRobotUser()
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='"+HttpContext.Current.Server.MapPath("~/avatar.xlsx") +"';Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'"))
            {
                DataTable dtTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [avatar$]", conn);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds);
                    int i = ds.Tables[0].Rows.Count;
                    dtTable = ds.Tables[0];

                    for (i = 0; i < dtTable.Rows.Count; i++)
                    {
                        tbl_user user = new tbl_user();
                        user.openid = Guid.NewGuid().ToString();
                        string avatarPath = @"E:\wbavatar";
                        DirectoryInfo root = new DirectoryInfo(avatarPath);
                        FileInfo[] files = root.GetFiles();
                        if (files.Length > 0)
                        {
                            int rand = new Random().Next(files.Length);
                            string avtar = files[rand].FullName;
                            string dest=HttpContext.Current.Server.MapPath("~/robotAvtar/") + user.openid + ".jpeg";
                            string destPath = WebConfigurationManager.AppSettings["hostName"] + "/robotAvtar/" + user.openid + ".jpeg";
                            File.Move(avtar, dest);
                            user.avantarUrl = destPath;
                            user.nickName = dtTable.Rows[i][2].ToString();
                            user.userStatus = 0;
                            user.registerDate = DateTime.Now;
                            user.ifRobot = 1;
                            UserProvider provider = new UserProvider();
                            provider.newOrUpdateUser(user,"");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    dtTable = new DataTable();

                }
            }
        }

        public void addRobotContent()
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + HttpContext.Current.Server.MapPath("~/jt.xlsx") + "';Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'"))
            {

                DataTable dtTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", conn);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds);
                    int i = ds.Tables[0].Rows.Count;
                    dtTable = ds.Tables[0];

                    for (i = 0; i < dtTable.Rows.Count; i++)
                    {
                        tbl_robotContent content = new tbl_robotContent();
                        content.contentId = Guid.NewGuid().ToString();
                        content.content = dtTable.Rows[i][0].ToString();
                        content.ifUsed = 0;
                        PostsProvider provider = new PostsProvider();
                        provider.addRobotContent(content);
                    }
                }
                catch (Exception ex)
                {
                    dtTable = new DataTable();
                }
            }
        }

        public void robotPics()
        {
            try
            {
                if (new Random().Next(5) == 1&&DateTime.Now.Hour>=8 && DateTime.Now.Hour<=23)
                //if(true)
                {
                    tbl_user user = new UserProvider().getRobotUser();
                    tbl_robotContent content = new PostsProvider().getRobotContent();
                    tbl_posts posts = new tbl_posts();
                    posts.postsMakeDate = DateTime.Now;
                    posts.postsMaker = user.openid;
                    posts.postsContent = new Random().Next(4)==1?content.content:" ";
                    posts.ifOfficial = 0;
                    posts.postsStatus = 0;
                    posts.postsID = Guid.NewGuid().ToString();
                    posts.postsPicCount = 1;
                    posts.postsType = "P";
                    posts.postsReaded = 0;
                    posts.postsCollected = 0;
                    posts.postsLoved = 0;
                    posts.postsLocation = "";
                    new PostsProvider().savePosts(posts);

                    writeTxt("开始寻找随机图片");
                    string robotPicPath = @"E:\duitangImg\Imgs";

                    DirectoryInfo root = new DirectoryInfo(robotPicPath);
                    FileInfo[] files = root.GetFiles();
                    writeTxt("一共有图片:" + files.Length);
                    int rand = new Random().Next(files.Length);
                    writeTxt("随机图片：" + rand.ToString());
                    FileInfo destImg = files[rand];

                    using (FileStream stream = File.Open(destImg.FullName, FileMode.Open))
                    {
                        writeTxt("开始执行保存缩略图");
                        new PicsHandler().savePics(stream, posts.postsID, 0);
                    }
                    File.Delete(destImg.FullName);
                }
            }
            catch(Exception e)
            {
                writeTxt(e.Message.ToString());
            }
        }

        public void writeTxt(string text)
        {
            PostsProvider provider = new PostsProvider();
            provider.addLog(text);
        }

        public void ifValidRobot()
        {

        }

        public void addRobotReply()
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='C:\Users\Admin\Desktop\tbl_robotReply.xlsx';Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'"))
            {

                DataTable dtTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [tbl_robotReply$]", conn);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds);
                    int i = ds.Tables[0].Rows.Count;
                    dtTable = ds.Tables[0];
                    for(i=0;i< ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_robotReply reply = new tbl_robotReply();
                        reply.replyId = Guid.NewGuid().ToString();
                        reply.replyContent = dtTable.Rows[i][0].ToString();
                        reply.replyType = "C";
                        PostsProvider provider = new PostsProvider();
                        provider.addRobotReply(reply);
                    }
                }
                catch (Exception ex)
                {
                    dtTable = new DataTable();
                }
            }
        }

        public void robotUserReply()
        {
            PostsProvider provider = new PostsProvider();
            UserProvider userP = new UserProvider();
            List<string> posts = provider.getPostsInLast36Hr();
            foreach(var item in posts)
            {
                if (provider.ifPostedByRobot(item))
                {
                    if(new Random().Next(432) == 127)
                    //if(true)
                    {
                        tbl_user user = userP.getRobotUser();
                        tbl_reply reply = new tbl_reply();
                        string replyContent = provider.getRobotReply("").replyContent + (new Random().Next(4)==1? provider.getRobotReply("E").replyContent:"");
                        reply.postsID = item;
                        reply.replyContent = replyContent;
                        reply.replyDate = DateTime.Now;
                        reply.replyID = Guid.NewGuid().ToString();
                        reply.replyLoved = 0;
                        reply.replyMaker = user.openid;
                        reply.replyStatus = 0;
                        provider.userReply(reply);
                    }

                    if(new Random().Next(144) == 68)
                    //if(true)
                    {
                        tbl_user user = userP.getRobotUser();
                        tbl_userloved loved = new tbl_userloved();
                        loved.lovedID = Guid.NewGuid().ToString();
                        loved.lovedTime = DateTime.Now;
                        loved.loveStatus = 1;
                        loved.postsID = item;
                        loved.userID = user.openid;
                        provider.lovePosts(loved);
                    }
                }
                else
                {
                    if (new Random().Next(216) == 127)
                    //if (true)
                    {
                        tbl_user user = userP.getRobotUser();
                        tbl_reply reply = new tbl_reply();
                        string replyContent = provider.getRobotReply("").replyContent + (new Random().Next(4) == 1 ? provider.getRobotReply("E").replyContent : "");
                        reply.postsID = item;
                        reply.replyContent = replyContent;
                        reply.replyDate = DateTime.Now;
                        reply.replyID = Guid.NewGuid().ToString();
                        reply.replyLoved = 0;
                        reply.replyMaker = user.openid;
                        reply.replyStatus = 0;
                        provider.userReply(reply);
                    }

                    if (new Random().Next(40) == 12)
                    //if(true)
                    {
                        tbl_user user = userP.getRobotUser();
                        tbl_userloved loved = new tbl_userloved();
                        loved.lovedID = Guid.NewGuid().ToString();
                        loved.lovedTime = DateTime.Now;
                        loved.loveStatus = 1;
                        loved.postsID = item;
                        loved.userID = user.openid;
                        provider.lovePosts(loved);
                    }
                }
            }
        }
    }
}
