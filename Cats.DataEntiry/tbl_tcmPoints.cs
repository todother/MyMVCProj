using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_tcmPoints
    {
        public string pointId { get; set; }
        public long questionId { get; set; }
        public int pType { get; set; }//1 start   2 dog
        public int pX { get; set; }
        public int pY { get; set; }
    }
}
