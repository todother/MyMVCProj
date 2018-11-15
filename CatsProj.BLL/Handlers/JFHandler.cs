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
    }
}
