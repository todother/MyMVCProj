using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzToken
{
    public class TokenTrigger
    {
        public static void Start()
        {
            ISchedulerFactory schedulerFact = new StdSchedulerFactory();
            IScheduler scheduler =  schedulerFact.GetScheduler();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<QuartzToken>().Build();
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("accessToken", "tokenGroup")
              .WithSimpleSchedule(t =>
                t.WithIntervalInMinutes(115)
                 .RepeatForever())
                 .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
