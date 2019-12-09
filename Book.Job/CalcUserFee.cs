using Book.Dal;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    public class CalcUserFee
    {
        public void Execute(int retryCount = 0)
        {
            try
            {
                if (retryCount >= 3)
                {
                    return;
                }
                var dt = DateTime.Now.AddDays(-1);
                var bills = ShopMonthOrderDal.GetInstance().GetPayedList(dt);
                if (bills == null || bills.Count == 0)
                    return;

                foreach(var bill in bills)
                {
                    var shop = ShopDal.GetInstance().Get(bill.ShopId);
                    if (shop.Recommender <= 0)
                        continue;

                    TransactionHelper.Run(()=> {
                        UserFeeService.GetInstance().ShopPay(bill, shop.Recommender);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Trace.WriteLine(ex.ToString());
                retryCount++;
                Execute(retryCount);
            }
        }
    }
}
