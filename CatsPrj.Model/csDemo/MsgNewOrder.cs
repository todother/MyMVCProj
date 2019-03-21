using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class MsgNewOrder
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string page { get; set; }
        public string form_id { get; set; }
        public Dictionary<string, KVpari> data { get; set; }
    }
}
