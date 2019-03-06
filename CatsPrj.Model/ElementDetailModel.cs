using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model
{
    public class ElementDetailModel
    {
        public int elementId { get; set; }
        public string elementImg { get; set; }
        public int elementPrice { get; set; }
        public decimal elementMoney { get; set; }
        public bool selected { get; set; }
    }
}
