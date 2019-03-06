using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_elementdetail
    {
        public int elementId { get; set; }
        public string elementImg { get; set; }
        public int elementGroup { get; set; }
        public int elementPrice { get; set; }
        public int elementIdx { get; set; }
        public decimal elementMoney { get; set; }
    }
}
