using System;
using CatsProj.DB;
using CatsDataEntity;
using MySql.Data;
using SqlSugar;
using System.Collections.Generic;
using Cats.DataEntiry;
using System.Data;
using CatsProj.DataEntiry;

namespace CatsProj.DAL.Providers
{
    public class SurveyProvider
    {
        public List<tbl_robot> getChatContent()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_robot> chats = db.Queryable<tbl_robot>().OrderBy(o => o.chatIdx).ToList();
            return chats;
        }

        public List<tbl_surveyQuestion> getQustions()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_surveyQuestion> questions = db.Queryable<tbl_surveyQuestion>().OrderBy(o => o.questionId).ToList();
            return questions;
        }

        public tbl_surveyanalysis getSurveyAnalysis(int questionId,int answerId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_surveyanalysis analysis = db.Queryable<tbl_surveyanalysis>().Where(o=>o.questionId==questionId && o.answerId==answerId).First();
            return analysis;
        }

        public List<tbl_commondesc> getDesc(List<tbl_commondesc> currList,int loopTime,string type,string openId)
        {
            int i = 0;
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            int gender = Convert.ToInt32( user.gender);
            List<tbl_commondesc> listShort = new List<tbl_commondesc>();
            List<tbl_commondesc> listLong = new List<tbl_commondesc>();
            listShort = db.Queryable<tbl_commondesc>().Where(o => o.property == type &&  o.len == 1 && (o.gender==0||o.gender==gender)).OrderBy(o => getRand()).ToList();
            listLong= db.Queryable<tbl_commondesc>().Where(o => o.property == type && o.len == 2 && (o.gender == 0 || o.gender == gender)).OrderBy(o => getRand()  ).ToList();

            for (i = 0; i < loopTime; i++)
            {
                currList.Add(listLong[i]);
                currList.Add(listShort[i]);
            }
            return currList;
        }

        public int getRand()
        {
            return 0;
        }

        public long getRandTCM()
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();

            tbl_tcmQuestion question = new tbl_tcmQuestion();
            question = db.Queryable<tbl_tcmQuestion>().OrderBy(o => getRand()).First();
            return question.questionId;
        }

        public long saveTCMQuestion(List<List<string>> dogs,List<string> startPoint,int width,int height,string openId)
        {

            SqlSugarClient db = SqlSugarInstance.newInstance();

            tbl_tcmQuestion question = new tbl_tcmQuestion();
            
            question.questionMaker = openId;
            question.questionMakeDate = DateTime.Now;
            question.width = height;
            question.height = width;

            question.questionId= db.Insertable<tbl_tcmQuestion>(question).ExecuteReturnBigIdentity();

            tbl_tcmPoints start = new tbl_tcmPoints();
            start.pointId = Guid.NewGuid().ToString();
            start.questionId = question.questionId;
            start.pType = 1;
            start.pX =Convert.ToInt32( startPoint[0]);
            start.pY = Convert.ToInt32(startPoint[1]);
            db.Insertable<tbl_tcmPoints>(start).ExecuteCommand();

            foreach(var item in dogs)
            {
                tbl_tcmPoints dog = new tbl_tcmPoints();
                dog.pointId = Guid.NewGuid().ToString();
                dog.questionId = question.questionId;
                dog.pType = 2;
                dog.pX = Convert.ToInt32(item[0]);
                dog.pY = Convert.ToInt32(item[1]);
                db.Insertable<tbl_tcmPoints>(dog).ExecuteCommand();
            }

            return question.questionId;
        }

        public tbl_user getUserByTCM(long qId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_tcmQuestion question = db.Queryable<tbl_tcmQuestion>().Where(o => o.questionId == qId).First();
            string openId = question.questionMaker;
            tbl_user user = db.Queryable<tbl_user>().Where(o => o.openid == openId).First();
            return user;
        }

        public List<tbl_tcmPoints> getTCMPoints(long qId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            List<tbl_tcmPoints> points = db.Queryable<tbl_tcmPoints>().Where(o => o.questionId == qId).ToList();
            return points;
        }

        public tbl_tcmQuestion getTCMQuestion(long qId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_tcmQuestion tcm = db.Queryable<tbl_tcmQuestion>().Where(o => o.questionId == qId).First();
            return tcm;
        }

        public int getSuccessCount(string openId,string qId)
        {
            SqlSugarClient db = SqlSugarInstance.newInstance();
            tbl_solveQuestion solve = new tbl_solveQuestion();
            solve.questionId = qId;
            solve.openId = openId;
            solve.solveId = Guid.NewGuid().ToString();
            solve.solveTime = DateTime.Now;

            db.Insertable<tbl_solveQuestion>(solve).ExecuteCommand();

            int count = db.Queryable<tbl_solveQuestion>().Where(o => o.openId == openId).Count();
            return count;
        }
    }
}
