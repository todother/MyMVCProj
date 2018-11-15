using System;
using CatsProj.DB;
using CatsDataEntity;
using MySql.Data;
using SqlSugar;
using System.Collections.Generic;
using Cats.DataEntiry;
namespace CatsProj.DAL.Providers
{
    public class JFProvider
    {
        public int getCurWeekQDRecord(string openId)
        {
            DateTime date = DateTime.Now;
            int today = (int)date.DayOfWeek==0?7: (int)date.DayOfWeek;
            DateTime fromDate = date.AddDays(-today + 1);
            SqlSugarClient db = SqlSugarInstance.newInstance();
            int times = db.Queryable<tbl_qdRecord>().Where(o => o.qdTime >= fromDate.Date && o.openId==openId).Count();
            return times;
        }

        public int userQD(string openId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            int ifQDToday = db.Queryable<tbl_qdRecord>().Where(o => o.qdTime.Date == DateTime.Now.Date).Count();
            if (ifQDToday > 0)
            {
                return -1;
            }
            else
            {
                int times = getCurWeekQDRecord(openId);
                int basePoint = 10;
                if (times == 3 || times == 6)
                {
                    basePoint = 15;
                }
                double extraPoint = basePoint + 10 - Math.Floor(Math.Sqrt(new Random().Next(100)));
                int result = Convert.ToInt32(extraPoint);

                tbl_qdRecord record = new tbl_qdRecord();
                record.qdId = Guid.NewGuid().ToString();
                record.openId = openId;
                record.qdTime = DateTime.Now;
                record.qdScore = result;
                db.Insertable<tbl_qdRecord>(record).ExecuteCommand();

                return result;
            }
        }

        public List<DailyMission> getUserDailyMission(string openId)
        {
            DailyMission mission = new DailyMission();
            SqlSugarClient db = SqlSugarInstance.newInstance();
            var missions = db.Queryable<tbl_missionConfig, tbl_dailyMission>((tm, td) => new object[] {
                JoinType.Left,tm.missionId==td.missionId
            }).Select((tm, td) => new DailyMission
            {
                dailyId = td.dailyId,
                detailDesc = tm.detailDesc,
                finished = (SqlFunc.IIF(td.finished==1,true,false)),
                missionDesc = tm.missionDesc,
                openId=td.openId,
                score=tm.score,
                times=tm.times,
                missionId=td.missionId,
                missionDate=td.missionDate,
                received=td.received
            }).Where(td=>td.missionDate==DateTime.Now.Date && td.openId==openId);
            if (missions.Count() > 0)
            {
                var list= missions.ToList();
                foreach(var item in list)
                {
                    switch (item.missionId)
                    {
                        case 1:
                            if (item.finished == false)
                            {
                                item.finished = ifInvite(openId, item.times);
                                if (item.finished)
                                {
                                    updateMissionStatus(item.dailyId);
                                }
                            }
                            break;
                        case 2:
                            if (item.finished == false)
                            {
                                item.finished = ifGoodPosts(openId, 30, 100);
                                if (item.finished)
                                {
                                    updateMissionStatus(item.dailyId);
                                }
                            }
                            break;
                        case 3:
                            if (item.finished == false)
                            {
                                item.finished = ifSufReply(openId, item.times);
                                if (item.finished)
                                {
                                    updateMissionStatus(item.dailyId);
                                }
                            }
                            
                            break;
                        case 4:
                            if (item.finished == false)
                            {
                                item.finished = false;
                                if (item.finished)
                                {
                                    updateMissionStatus(item.dailyId);
                                }
                            }
                            break;
                        case 5:
                            if (item.finished == false)
                            {
                                item.finished = ifViewPosts(openId, item.times);
                                if (item.finished)
                                {
                                    updateMissionStatus(item.dailyId);
                                }
                            }
                            
                            break;
                        case 6:
                            if (item.finished == false)
                            {
                                item.finished = ifShare(item.times, openId);
                                if (item.finished)
                                {
                                    updateMissionStatus(item.dailyId);
                                }
                            }
                            
                            break;
                        default:
                            break;


                    }
                }
                return list;
            }
            else
            {
                List<tbl_missionConfig> allConfig = new List<tbl_missionConfig>();
                allConfig = getAllMission();
                List<tbl_dailyMission> dailyMissions = new List<tbl_dailyMission>();
                foreach(var item in allConfig)
                {
                    tbl_dailyMission daily = new tbl_dailyMission();
                    daily.finished = 0;
                    daily.dailyId = Guid.NewGuid().ToString();
                    daily.openId = openId;
                    daily.missionDate = DateTime.Now.Date;
                    daily.missionId = item.missionId;
                    dailyMissions.Add(daily);
                }
                addDailyMission(dailyMissions);
                return getUserDailyMission(openId);
            }
        }

        public void updateMissionStatus(string dailyId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_dailyMission mission = db.Queryable<tbl_dailyMission>().Where(o => o.dailyId == dailyId).First();
            mission.finished = 1;
            db.Updateable<tbl_dailyMission>(mission).Where(o => o.dailyId == dailyId).ExecuteCommand();
        }

        public int getDailyPoints(string openId)
        {
            int result = 0;
            DailyMission mission = new DailyMission();
            SqlSugarClient db = SqlSugarInstance.newInstance();
            var missions = db.Queryable<tbl_missionConfig, tbl_dailyMission>((tm, td) => new object[] {
                JoinType.Left,tm.missionId==td.missionId
            }).Select((tm, td) => new DailyMission
            {
                dailyId = td.dailyId,
                detailDesc = tm.detailDesc,
                finished = (SqlFunc.IIF(td.finished == 1, true, false)),
                missionDesc = tm.missionDesc,
                openId = td.openId,
                score = tm.score,
                times = tm.times,
                missionId = td.missionId,
                missionDate = td.missionDate,
                received = td.received
            }).Where(td => td.missionDate == DateTime.Now.Date && td.openId == openId).ToList();
            foreach(var item in missions)
            {
                if(item.finished==true && item.received == 0)
                {
                    result += item.score;
                    var temp = db.Queryable<tbl_dailyMission>().Where(o => o.dailyId == item.dailyId).First();
                    temp.received = 1;
                    db.Updateable<tbl_dailyMission>(temp).Where(o => o.dailyId == temp.dailyId).ExecuteCommand();
                }
            }
            return result;
        }

        public List<tbl_missionConfig> getAllMission()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            var result = db.Queryable<tbl_missionConfig>().ToList();
            return result;
        }

        public void addDailyMission(List<tbl_dailyMission> list)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            foreach(var item in list)
            {
                db.Insertable<tbl_dailyMission>(item).ExecuteCommand();
            }
        }

        public bool ifShare(int times,string openId)//检查用户是否转发了
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            bool ifFinish = db.Queryable<tbl_userShare>().Where(o => o.userId == openId && o.shareTime.Date==DateTime.Now.Date).Count()>=times;
            return ifFinish;
        }

        public bool ifSufReply(string openId,int times)//检查用户的回复是否可作为精华
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_reply> replies = db.Queryable<tbl_reply>().Where(o => o.replyDate.Value.Date == DateTime.Now.Date && o.replyMaker == openId).ToList();
            //bool ifSucc = false;
            foreach(var item in replies)
            {
                int likeCount = db.Queryable<tbl_userReplyLoved>().Where(o => o.replyId == item.replyID).Count();
                if (likeCount >= times)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ifViewPosts(string openId,int times)//检查用户当天阅读了几篇
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            int viewCount = db.Queryable<tbl_userviewed>().Where(o => o.userID == openId && o.viewTime.Value.Date == DateTime.Now.Date).GroupBy(o=>new { o.postsID}).Select(o => o.postsID).Count();
            return viewCount >= times;
        }

        public bool ifGoodPosts(string openId,int lovetimes,int viewTimes)//检查是否是个优质帖子
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_posts> posts = db.Queryable<tbl_posts>().Where(o => o.postsMaker == openId && o.postsMakeDate.Value.Date == DateTime.Now.Date).ToList();
            foreach(var item in posts)
            {
                bool ifGood = db.Queryable<tbl_userloved>().Where(o => o.postsID == item.postsID && o.loveStatus == 1).Count()>=lovetimes;
                bool ifViewed= db.Queryable<tbl_userviewed>().Where(o => o.postsID == item.postsID ).Count() >= viewTimes;
                if(ifGood||ifViewed)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ifInvite(string openId,int times)//检查用户是否邀请新用户
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            int invites = db.Queryable<tbl_user>().Where(o => o.referBy == openId && o.registerDate.Value.Date == DateTime.Now.Date).Count();
            return invites >= times;
        }
    }
}
