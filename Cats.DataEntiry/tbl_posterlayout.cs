using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.DataEntiry
{
    public class tbl_posterlayout
    {
        public string posterId { get; set; }
        public string posterPath { get; set; }
        public int posterImgPositionX { get; set; }
        public int posterImgPositionY { get; set; }
        public int posterCttPositionX { get; set; }
        public int posterCttPositionY { get; set; }
        public int posterQRCodePositionX { get; set; }
        public int posterQRCodePositionY { get; set; }
        public long posterIdx { get; set; }
    }
}
