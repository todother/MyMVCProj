using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class DailyMissionConverter
    {
        public static DailyMissionModel convertEntityToModel(DailyMission mission)
        {
            DailyMissionModel model = new DailyMissionModel();
            model.dailyId = mission.dailyId;
            model.detailDesc = mission.detailDesc;
            model.finished = mission.finished;
            model.missionDate = mission.missionDate;
            model.missionDesc = mission.missionDesc;
            model.missionId = mission.missionId;
            model.openId = mission.openId;
            model.score = mission.score;
            model.times = mission.times;
            return model;
        }
    }
}
