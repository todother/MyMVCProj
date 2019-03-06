using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model
{
    public class ElementGroupModel
    {
        public int groupId { get; set; }
        public string elementImage { get; set; }
        public bool selected { get; set; }
        public string elementGrpName { get; set; }
    }
}
