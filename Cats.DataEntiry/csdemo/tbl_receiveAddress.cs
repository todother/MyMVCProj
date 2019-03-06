using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_receiveAddress
    {
        public string addressId { get; set; }
        public string receiver { get; set; }
        public string telno { get; set; }
        public string homeaddress { get; set; }
        public int selected { get; set; }//0未选，1已选
        public string openId { get; set; }
    }
}
