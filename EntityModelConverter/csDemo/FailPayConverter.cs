using Cats.DataEntiry.csdemo;
using CatsPrj.Model.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class FailPayConverter
    {
        public static FailPayModel convertFailyPayToModel(tbl_failpay failpay)
        {
            FailPayModel model = new FailPayModel();
            model.nonceStr = failpay.nonceStr;
            model.orderId = failpay.orderId;
            model.packageId = failpay.packageId;
            model.paySign = failpay.paySign;
            model.timeStamp = failpay.timeStamp;
            return model;
        }
    }
}
