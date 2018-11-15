using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class RobotChatConverter
    {
        public static RobotChatModel convertEntityToModel(tbl_robot robot)
        {
            RobotChatModel model = new RobotChatModel();
            model.chatContent = robot.chatContent;
            model.chatId = robot.chatId;
            model.chatIdx = robot.chatIdx;
            model.robotName = robot.robotName;
            model.robotType = robot.robotType;
            model.timeout = robot.timeout;
            model.waitUserResponse = robot.waitUserResponse;
            return model;
        }
    }
}
