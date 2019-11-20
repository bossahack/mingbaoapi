using Book.Dal;
using Quartz;
using System;
using System.Collections.Generic;
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
                ShopDayOrderDal.GetInstance().CalcShopDayOrder(DateTime.Now.AddDays(-2));
            }
            catch(Exception ex)
            {
                retryCount++;
                Execute(retryCount);
            }
        }
    }
}
