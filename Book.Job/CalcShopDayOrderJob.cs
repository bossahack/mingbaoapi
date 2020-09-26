using Book.Dal;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    public class CalcShopDayOrderJob
    {
        public void Execute(int retryCount=0)
        {
            try
            {
                if (retryCount >= 3)
                {
                    return;
                }
                var beginDay = DateTime.Now.AddDays(-2);
                calcOrderFee(beginDay);
                calcDayOrderFee(beginDay);
                ShopDayOrderDal.GetInstance().CalcShopDayOrderQty(DateTime.Now.AddDays(-2));
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                retryCount++;
                Execute(retryCount);
            }
        }

        private void calc()
        {
            var beginDate = DateTime.Now.AddDays(-2);
            var shopDayOrders = ShopDayOrderDal.GetInstance().GetAfterDay(beginDate);
            if (shopDayOrders == null)
                return;
            foreach(var shopDayOrder in shopDayOrders)
            {

            }
        }

        private void calcOrderFee(DateTime beginDay)
        {
            OrderDal.GetInstance().CalcOrderPrice(beginDay);
        }
        private void calcDayOrderFee(DateTime beginDay)
        {
            ShopDayOrderDal.GetInstance().CalcDayOrderFee(beginDay);

        }
    }
}
