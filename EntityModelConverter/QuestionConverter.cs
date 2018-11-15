using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class QuestionConverter
    {
        public static QuestionModel convertEntityToModel(tbl_surveyQuestion entity)
        {
            QuestionModel model = new QuestionModel();
            model.questionId = entity.questionId;
            model.questionContent = entity.questionContent;
            model.A = entity.A;
            model.B = entity.B;
            model.C = entity.C;
            model.D = entity.D;
            return model;
        }
    }
}
