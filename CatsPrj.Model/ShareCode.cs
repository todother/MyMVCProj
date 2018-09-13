using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model
{
    public class ShareCode
    {
        public long shareId { get; set; }
        public string openId { get; set; }
        public string postsId { get; set; }
        public DateTime shareTime { get; set; }
    }
}
