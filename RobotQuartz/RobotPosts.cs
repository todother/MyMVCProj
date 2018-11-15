
using CatsProj.BLL.Handlers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotQuartz
{
    public class RobotPosts:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            RobotHandler handler = new RobotHandler();
            handler.robotPics();
            handler.robotUserReply();
        }
    }
}
