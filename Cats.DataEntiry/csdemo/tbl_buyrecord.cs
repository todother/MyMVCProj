using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry.csdemo
{
    public class tbl_buyrecord
    {
        public string cartId { get; set; }
        public string orderId { get; set; }
        public DateTime subTime { get; set; }

        public string addressId { get; set; }
    }
}
