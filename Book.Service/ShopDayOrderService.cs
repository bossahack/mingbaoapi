using Book.Dal;
using Book.Dal.Model;
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

    }
}
