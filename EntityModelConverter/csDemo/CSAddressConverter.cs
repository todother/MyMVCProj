using Cats.DataEntiry.csdemo;
using CatsPrj.Model.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class CSAddressConverter
    {
        public static CSAddressModel convertAddressToModel(tbl_receiveAddress entity)
        {
            CSAddressModel model = new CSAddressModel();
            model.addressId = entity.addressId;
            model.homeaddress = entity.homeaddress;
            model.openId = entity.openId;
            model.receiver = entity.receiver;
            model.selected = entity.selected;
            model.telno = entity.telno;
            return model;
        }
    }
}
