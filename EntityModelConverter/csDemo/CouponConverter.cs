using Cats.DataEntiry.csdemo;
using CatsPrj.Model.csDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter.csDemo
{
    public class CouponConverter
    {
        public static UserApplyCouponModel convertApplyEntityToModel(UserApplyCoupon entity)
        {
            UserApplyCouponModel model = new UserApplyCouponModel();
            model.applyTimes = entity.applyTimes;
            model.baseline = entity.baseline;
            model.canApplyTimes = entity.canApplyTimes;
            model.couponAmt = entity.couponAmt;
            model.couponId = entity.couponId;
            model.period = entity.period;
            model.remainCount = entity.remainCount;
            return model;
        }

        public static UserCouponModel convertCouponToModel(tbl_userpacket entity)
        {
            UserCouponModel model = new UserCouponModel();
            model.baseline = entity.baseline;
            model.deadline = entity.deadline.ToString("yy-MM-dd");
            model.openId = entity.openId;
            model.orderId = entity.orderId;
            model.packetAmt = entity.packetAmt;
            model.packetId = entity.packetId;
            model.status = entity.status;
            model.userpacketId = entity.userpacketId;
            model.useTime = entity.useTime;
            return model;
        }

        public static CouponCanUseModel converCanuseToModel(CouponCanUse entity)
        {
            CouponCanUseModel model = new CouponCanUseModel();
            model.baseline = entity.baseline;
            model.packetAmt = entity.couponAmt;
            model.couponId = entity.couponId;
            model.ifCanUse = entity.ifCanUse;
            model.deadline = entity.deadline.ToString("yy-MM-dd");
            return model;
        }
    }
}
