using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsPrj.Model.csDemo
{
    public class CSCateModel
    {
        public int cateId { get; set; }
        public string cateName { get; set; }
        public int cateIdx { get; set; }
        public string catePicName { get; set; }
        public List<CSItemModel> items { get; set; }
        public bool selected { get; set; }
        public int stock { get; set; }
    }
}
