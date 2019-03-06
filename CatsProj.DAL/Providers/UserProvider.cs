using System;
using System.Collections.Generic;
using CatsProj.DB;
using CatsDataEntity;
using SqlSugar;
using Cats.DataEntiry;
using System.Data;

namespace CatsProj.DAL.Providers
{
    //
    public class UserProvider
    {
        public tbl_user getUser(string userid)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user result = db.Queryable<tbl_user>().Where(o => o.openid == userid).First();
            return result;
        }

        public void newOrUpdateUser(tbl_user user,string refer)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == user.openid).First();
                if (curUser != null)
                {
                    if (curUser.avantarUrl != user.avantarUrl || curUser.nickName != user.nickName || curUser.gender != user.gender || curUser.country != user.country || curUser.city != user.city || curUser.province != user.province)
                    {
                        curUser.lastLoginDate = DateTime.Now;
                        //user.registerDate = curUser.registerDate;
                        //user.userStatus = curUser.userStatus;
                        curUser.avantarUrl = user.avantarUrl;
                        curUser.nickName = user.nickName;
                        curUser.gender = user.gender;
                        curUser.country = user.country;
                        curUser.city = user.city;
                        curUser.province = user.province;
                        db.Updateable<tbl_user>(curUser).Where(o => o.openid == user.openid).ExecuteCommand();
                    }
                    updateLastLoginDate(user.openid);
                }
                else
                {
                    user.referBy = refer;
                    user.registerDate = DateTime.Now;
                    user.lastRefreshDate = DateTime.Now;
                    user.lastRefreshFans = DateTime.Now;
                    user.userStatus = 0;//0 means the user is under active status
                    user.lastLoginDate = DateTime.Now;
                    db.Insertable<tbl_user>(user).ExecuteCommand();

                    tbl_userFollowed followedSelf = new tbl_userFollowed();
                    followedSelf.followedTime = DateTime.Now;
                    followedSelf.followedUser = user.openid;
                    followedSelf.followId = Guid.NewGuid().ToString();
                    followedSelf.userId = user.openid;
                    db.Insertable<tbl_userFollowed>(followedSelf).ExecuteCommand();

                    List<tbl_admin> admin = new List<tbl_admin>();
                    admin = db.Queryable<tbl_admin>().ToList();
                    foreach (var item in admin)
                    {
                        bool ifFollowed = false;
                        ifFollowed = db.Queryable<tbl_userFollowed>().Where(o => o.userId == user.openid && o.followedUser == item.openId).Count() > 0;
                        if (!ifFollowed)
                        {
                            tbl_userFollowed followed = new tbl_userFollowed();
                            followed.followedTime = DateTime.Now;
                            followed.followedUser = item.openId;
                            followed.followId = Guid.NewGuid().ToString();
                            followed.userId = user.openid;
                            db.Insertable<tbl_userFollowed>(followed).ExecuteCommand();
                        }
                    }

                    db.Insertable<tbl_userConfig>(new tbl_userConfig { userId = user.openid, byTime = 0, byViewed = 1, onlyLoved = 0, onlyVerify = 0, videoMuted = 1 }).ExecuteCommand();
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
        }

        public void updateUserStatus(string userId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == userId).First();
            curUser.userStatus = 1;//disable the user
            db.Updateable<tbl_user>(curUser);
        }

        public void updateLastLoginDate(string userId)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                tbl_user curUser = db.Queryable<tbl_user>().Where(o => o.openid == userId).First();
                curUser.lastLoginDate = DateTime.Now;//update lastlogin date
                db.Updateable(curUser).Where(o => o.openid == userId).UpdateColumns(arg => new { arg.lastLoginDate }).ExecuteCommand();
            }
            catch (Exception e)
            {
                int i = 1;
            }
        }

        public void updateNickName(string openid, string nickName)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user cur = db.Queryable<tbl_user>().Where(o => o.openid == openid).First();
            cur.nickName = nickName;
            db.Updateable(cur).Where(o => o.openid == openid).UpdateColumns(arg => new { arg.nickName }).ExecuteCommand();
        }

        public tbl_user getUserInfo(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            return user;
        }

        public tbl_userConfig getUserConfig(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_userConfig config = db.Queryable<tbl_userConfig>().Where(o => o.userId == openId).First();
            return config;
        }


        public void updateUserConfig(tbl_userConfig config)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Updateable<tbl_userConfig>(config).WhereColumns(o => o.userId).ExecuteCommand();
        }

        public bool ifFollowed(string postsId, string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            try
            {
                var dt = db.Ado.GetDataTable(@"select tu.* from tbl_user tu 
            left join tbl_userFollowed tuf on tu.openid=tuf.userid
            left join tbl_posts tp on tp.postsMaker=tuf.followedUser
            where tu.openId=@openId and tp.postsId=@postsId ", new List<SugarParameter>(){
                new SugarParameter("@openId",openId),
                new SugarParameter("@postsId",postsId)
                });
                int i = dt.Rows.Count;
                return i > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void addFollow(string openId, string postsId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_posts posts = db.Queryable<tbl_posts>().Where(o => o.postsID == postsId).First();
            string postsMaker = posts.postsMaker;
            tbl_userFollowed followed = new tbl_userFollowed();
            followed.followId = Guid.NewGuid().ToString();
            followed.userId = openId;
            followed.followedUser = postsMaker;
            followed.followedTime = DateTime.Now;
            tbl_userFollowed tf = new tbl_userFollowed();
            long i = db.Queryable<tbl_userFollowed>().Where(o => o.userId == openId && o.followedUser == postsMaker).Count();
            if (i == 0)
            {
                db.Insertable<tbl_userFollowed>(followed).ExecuteCommand();
            }
        }

        public void delFollow(string openId, string userId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Deleteable<tbl_userFollowed>().Where(new tbl_userFollowed { userId = openId, followedUser = userId }).ExecuteCommand();
        }

        public List<tbl_user> getFollowedUsers(string openId, int from, int count)
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                List<tbl_userFollowed> fansList = new List<tbl_userFollowed>();
                fansList = db.Queryable<tbl_userFollowed>().Where(o => o.userId == openId).OrderBy(o => o.followedTime, OrderByType.Desc).ToPageList(from / count + 1, count);
                List<tbl_user> followedList = new List<tbl_user>();
                foreach (var item in fansList)
                {
                    followedList.Add(db.Queryable<tbl_user>().Where(o => o.openid == item.followedUser).First());

                }
                return followedList;
            }
            catch (Exception e)
            {
                return new List<tbl_user>();
            }
        }

        public List<tbl_user> getFans(string openId, int from, int count)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            /*List<tbl_user> followedList = db.Queryable<tbl_user>("tu")
                                           .Where("tu.openId in (select userId from tbl_userfollowed where followedUser='" + openId + "')")
                                           .ToPageList(from/count+1,count);*/
            List<tbl_userFollowed> fansList = new List<tbl_userFollowed>();
            fansList = db.Queryable<tbl_userFollowed>().Where(o => o.followedUser == openId).OrderBy(o => o.followedTime, OrderByType.Desc).ToPageList(from / count + 1, count);
            List<tbl_user> followedList = new List<tbl_user>();
            foreach (var item in fansList)
            {
                tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == item.userId).First();
                if (user != null)
                {
                    followedList.Add(user);
                }

            }
            return followedList;
        }

        public long getFollowCount(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long count = db.Queryable<tbl_userFollowed>().Where(o => o.userId == openId).Count();
            return count;
        }


        public long getFansCount(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            long count = db.Queryable<tbl_userFollowed>().Where(o => o.followedUser == openId).Count();
            return count;
        }

        public List<tbl_user> getUserScoreCard()
        {
            try
            {
                SqlSugarClient db = SqlSugarInstance.newInstance();
                //DataTable dt = db.Ado.UseStoredProcedure().GetDataTable("proc_scorecard", new { });
                //List<string> scores = new List<string>();
                //int i = 0;
                //for (i = 0; i < 200; i++)
                //{
                //    if (i < dt.Rows.Count)
                //    {
                //        scores.Add(dt.Rows[i][0].ToString());
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                List<tbl_user> users = db.Queryable<tbl_user>().Where(o=>o.ifRobot!=1).OrderBy(o=>getRand()).ToList().GetRange(0,20);
                return users;
            }
            catch (Exception e)
            {
                return new List<tbl_user>();
            }
        }

        

        public bool ifUserFollowedByOpenId(string openId, string userId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_userFollowed followed = new tbl_userFollowed();
            followed = db.Queryable<tbl_userFollowed>().Where(o => o.userId == openId && o.followedUser == userId).First();
            if (followed != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void followUserByUserId(string openId, string userId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_userFollowed>(new tbl_userFollowed
            {
                followId = Guid.NewGuid().ToString(),
                userId = userId,
                followedUser = openId,
                followedTime = DateTime.Now
            }).ExecuteCommand();
        }

        public void delFollowedUserById(string openId, string userId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Deleteable<tbl_userFollowed>().Where(o => o.userId == userId && o.followedUser == openId).ExecuteCommand();
        }

        public bool isAdmin(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            return db.Queryable<tbl_admin>().Where(o => o.openId == openId).Count() > 0;
        }

        public DateTime getLastRefreshDate(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            DateTime lastRefresh = db.Queryable<tbl_user>().Where(o => o.openid == openId).Select(o => o.lastRefreshDate).First().GetValueOrDefault();
            return lastRefresh;
        }

        public void updateLastRefreshDate(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Updateable<tbl_user>().UpdateColumns(o => new tbl_user { lastRefreshDate = DateTime.Now }).Where(o => o.openid == openId).ExecuteCommand();
        }

        public long newFansCount(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            long count = db.Queryable<tbl_userFollowed>().
                           Where(o => o.followedUser == openId && o.followedTime >= user.lastRefreshFans.GetValueOrDefault()).Count();
            return count;
        }

        public void updateLastRefreshFans(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Updateable<tbl_user>().UpdateColumns(o => new tbl_user { lastRefreshFans = DateTime.Now }).Where(o => o.openid == openId).ExecuteCommand();
        }

        public tbl_user getCoverPageUser()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            string userId = db.Queryable<tbl_coverPage>().Where(o => o.startDate <= DateTime.Now).OrderBy(o => o.startDate, OrderByType.Desc).First().userId;
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == userId).First();
            return user;
        }

        public void userTransPage(string openId,string pageName)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_transpage transpage = new tbl_transpage();
            transpage.openId = openId;
            transpage.pageName = pageName;
            transpage.transId = Guid.NewGuid().ToString();
            transpage.transTime = DateTime.Now;
            db.Insertable<tbl_transpage>(transpage).ExecuteCommand();
        }

        public void saveFormSubmit(string openId,string formId)
        {
            tbl_welcomeform welcomeForm = new tbl_welcomeform();
            welcomeForm.formId = formId;
            welcomeForm.openId = openId;
            welcomeForm.welcomeId = Guid.NewGuid().ToString();
            welcomeForm.submitTime = DateTime.Now;

            SqlSugarClient db = SqlSugarInstance.newInstance();
            db.Insertable<tbl_welcomeform>(welcomeForm).ExecuteCommand();
        }

        public int needToShowMask(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_welcomeform form = db.Queryable<tbl_welcomeform>().Where(o => o.openId == openId).OrderBy(o => o.submitTime, OrderByType.Desc).First();
            if(form==null)
            {
                return 0;
            }
            else if (form.submitTime.AddDays(6)<=DateTime.Now)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public long getLovedCount(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            string[] tpList = db.Queryable<tbl_posts>().Where(o => o.postsMaker == openId).Select(o=>o.postsID).ToList().ToArray();
            long postsLoved = db.Queryable<tbl_userloved>().Where(o => SqlFunc.ContainsArray<string>(tpList, o.postsID)).Count();
            return postsLoved;
        }

        public void updateSelfIntro(string openId,string selfIntro)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            user.selfIntro = selfIntro;
            db.Updateable<tbl_user>(user).Where(o => o.openid == openId).ExecuteCommand();
        }

        public tbl_user getRobotUser()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.ifRobot==1).OrderBy(o=>getRand()).First();
            return user;
        }

        public int getRand()
        {
            return 0;
        }

        public List<tbl_user> getRobotUsers()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_user> robots = db.Queryable<tbl_user>().Where(o => o.ifRobot == 1).ToList();
            return robots;
        }
    }
}
