using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_surveyanalysis
    {
        public int analysisId { get; set; }
        public int questionId { get; set; }
        public int answerId { get; set; }
        public int HD { get; set; }
        public int CM { get; set; }
        public int YG { get; set; }
        public int NR { get; set; }
        public int LJ { get; set; }
    }
}
