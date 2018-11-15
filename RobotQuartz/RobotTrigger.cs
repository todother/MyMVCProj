using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotQuartz
{
    public class RobotTrigger
    {
        public static void Start()
        {
            ISchedulerFactory schedulerFact = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFact.GetScheduler();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<RobotPosts>().Build();
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("robotTrigger", "robot")
              .WithSimpleSchedule(t =>
                t.WithIntervalInMinutes(1)
                 .RepeatForever())
                 .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
