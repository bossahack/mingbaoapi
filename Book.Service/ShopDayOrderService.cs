using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class ShopDayOrderService
    {
        private static ShopDayOrderService _Instance;
        public static ShopDayOrderService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopDayOrderService();
            return _Instance;
        }

        public void AddQty(int shopId, int qty)
        {
            var now = DateTime.Now;
            var result = ShopDayOrderDal.GetInstance().AddQty(shopId, qty, now);
            if (result > 0)
                return;
            ShopDayOrder dayorder = new ShopDayOrder()
            {
                ShopId = shopId,
                Date = now,
                Qty = qty
            };
            ShopDayOrderDal.GetInstance().Create(dayorder);

        }

        public Page<ShopDayOrderSearchModel> Search(ShopDayOrderSearchParam para)
        {
            var db = ShopDayOrderDal.GetInstance().Search(para);
            if (db.Total == 0)
                return new Page<ShopDayOrderSearchModel>()
                {
                    Total = 0
                };
            var shops = ShopDal.GetInstance().GetList(db.Items.Select(c => c.ShopId).ToList());
            return new Page<ShopDayOrderSearchModel>()
            {
                Total = db.Total,
                Items = db.Items.Select(c => new ShopDayOrderSearchModel()
                {
                    Id = c.Id,
                    Date=c.Date,
                    EffectQty=c.EffectQty,
                    Qty=c.Qty,
                    ShopId=c.ShopId,
                    ShopName= shops.FirstOrDefault(s=>s.Id==c.ShopId)?.Name
                }).ToList()
            };
        }

    }
}
