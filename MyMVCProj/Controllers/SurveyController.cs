using CatsPrj.Model;
using CatsProj.BLL.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCProj.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getChatContent()
        {
            List<RobotChatModel> chats = new List<RobotChatModel>();
            chats = new SurveyHandler().getChatModels();
            return Json(new { result = chats }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getQuestions()
        {
            List<QuestionModel> result = new SurveyHandler().getQustions();
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAnalysisResult(string answers,string openId)
        {
            string analysisResult = new SurveyHandler().calcResult(answers);
            List<string> descs = new SurveyHandler().getDescs(analysisResult,openId);
            return Json(new { result = analysisResult, descs=descs }, JsonRequestBehavior.AllowGet);

        }

        public void generateSurveyCode()
        {
            new SurveyHandler().getSurveyQRCode();
        }

        public JsonResult saveTCMQuestion(string dogs, string startPoint, int width, int height, string openId)
        {
            long questionId = new SurveyHandler().saveTCMQuestion(dogs, startPoint, width, height, openId);
            return Json(new { questionId = questionId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTCMInfo(long qId)
        {
            SurveyHandler handler = new SurveyHandler();
            List<List<TCMContent>> matrix = handler.generateMatrix(qId);
            return Json(new { result = matrix }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getQuestionMaker(long qId)
        {
            SurveyHandler handler = new SurveyHandler();
            if (qId == 0)
            {
                qId = handler.getRandQuestion();
            }
            UserModel user = handler.getUserByTCM(qId);
            return Json(new { result = user,qId=qId }, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult getRandQuestion()
        {
            return Json(new { qId = new SurveyHandler().getRandQuestion() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult solveQuestion(string openId,string qId)
        {
            return Json(new { count = new SurveyHandler().getSolveCount(openId, qId) }, JsonRequestBehavior.AllowGet);
        }
    }
}