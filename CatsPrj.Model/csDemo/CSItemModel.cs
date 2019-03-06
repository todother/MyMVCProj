using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class CSItemModel
    {
        public int itemId { get; set; }
        public int itemCate { get; set; }
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double itemPrice { get; set; }
        public double itemPriceDdt { get; set; }
        public string Specs { get; set; }
        public List<CSPicModel> picList { get; set; }
    }
}
