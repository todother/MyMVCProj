using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class CSAddressModel
    {
        public string addressId { get; set; }
        public string receiver { get; set; }
        public string telno { get; set; }
        public string homeaddress { get; set; }
        public int selected { get; set; }//0未选，1已选
        public string openId { get; set; }
    }
}
