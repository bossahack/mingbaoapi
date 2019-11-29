using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Model.Enums;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class ShopMonthOrderService
    {
        private static ShopMonthOrderService _Instance;
        public static ShopMonthOrderService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopMonthOrderService();
            return _Instance;
        }

        public bool hasUnPay()
        {
            var current = UserUtil.CurrentUser();
            var unpays=ShopMonthOrderDal.GetInstance().GetByStatus(current.ShopId, (int)BillStatus.UnPay);
            if (unpays != null && unpays.Count > 0)
                return true;
            return false;
        }
        public List<BillModel> GetLast()
        {
            var current = UserUtil.CurrentUser();
            var list = ShopMonthOrderDal.GetInstance().GetTopList(current.ShopId, 12);
            if (list == null || list.Count == 0)
                return null;
            var result = new List<BillModel>();
            list.ForEach((item) =>
            {
                result.Add(new BillModel()
                {
                    Id = item.Id,
                    EffectQty = item.EffectQty,
                    Month = item.Month,
                    Qty = item.Qty,
                    ShouldPay = item.ShouldPay,
                    Status = item.Status,
                    Year = item.Year
                });
            });
            return result;

        }

        public void Finish(int id,decimal fee)
        {
            if (id == 0)
                return;
            var current = UserUtil.CurrentUser();
            var bill = ShopMonthOrderDal.GetInstance().Get(id);
            if (bill.Status == (int)BillStatus.Init)
            {
                throw new Exception("存在初始状态的订单，操作失败");
            }
            bill.Status = (int)BillStatus.Payed;
            ShopFeeRecord feeRecord = new ShopFeeRecord() {
                CreateTime=DateTime.Now,
                Fee=fee,
                ShopId=current.ShopId
            };
            var shop = ShopDal.GetInstance().Get(bill.ShopId);
            TransactionHelper.Run(()=> {
                ShopMonthOrderDal.GetInstance().Update(bill);
                ShopFeeRecordDal.GetInstance().Create(feeRecord);
                if (shop.Recommender > 0)
                {
                    UserFeeService.GetInstance().ShopPay(bill, shop.Recommender);
                }
            });
        }

        public void ZeroPay(int id)
        {
            var current = UserUtil.CurrentUser();
            var bill = ShopMonthOrderDal.GetInstance().Get(id);
            if (bill == null)
                throw new Exception("未查到该订账单");
            if (bill.ShopId != current.ShopId)
                throw new Exception("您无权限操作");
            if (bill.ShouldPay > 0)
                throw new Exception("您无权操作");
            if (bill.Status != (int)BillStatus.UnPay)
                throw new Exception("账单不是未付款状态，不可操作");
            bill.Status = (int)BillStatus.Payed;
            ShopMonthOrderDal.GetInstance().Update(bill);
        }
    }
}
