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
    /**
    * 28 29 30
    * 28
    * 28 29 30 31
    **/
    public class CalcShopMonthOrderJob
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void Execute(int retryCount=0)
        {
            try
            {
                if (retryCount >= 3)
                {
                    return;
                }
                initOrderMonth();
                calcMonth();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                retryCount++;
                Execute(retryCount);
            }
        }

        private void initOrderMonth()
        {
            var now = DateTime.Now;
            var todayOfMonth = now.Day;//今天
            var lastDayOfMonth = now.AddDays(1 - todayOfMonth).AddMonths(1).AddDays(-1).Day;//这个月的最后一天

            var lastDayOfBeforeMonth = now.AddDays(1 - todayOfMonth).AddDays(-1).Day;//上个月的最后一天
            var dateOfBeforeMonth = now.AddMonths(-1);

            if (todayOfMonth == lastDayOfMonth && lastDayOfMonth <= lastDayOfBeforeMonth)//上个月今天的到月底的都要初始化
            {
                ShopMonthOrderDal.GetInstance().Init(dateOfBeforeMonth, now.AddDays(1 - todayOfMonth), dateOfBeforeMonth.Year, dateOfBeforeMonth.Month);
            }
            else//初始化上个月当天的
            {
                if (!(todayOfMonth > lastDayOfBeforeMonth))
                {
                    ShopMonthOrderDal.GetInstance().Init(dateOfBeforeMonth, dateOfBeforeMonth.AddDays(1), dateOfBeforeMonth.Year, dateOfBeforeMonth.Month);
                }
            }
        }

        private void calcMonth()
        {
            var now = DateTime.Now.AddDays(-2);
            var todayOfMonth = now.Day;//今天
            var lastDayOfMonth = now.AddDays(1 - todayOfMonth).AddMonths(1).AddDays(-1).Day;//这个月的最后一天

            var lastDayOfBeforeMonth = now.AddDays(1 - todayOfMonth).AddDays(-1).Day;//上个月的最后一天
            var dateOfBeforeMonth = now.AddMonths(-1);

            if (todayOfMonth == lastDayOfMonth && lastDayOfMonth <= lastDayOfBeforeMonth)//上个月今天的到月底的都要
            {
                ShopMonthOrderDal.GetInstance().Calc(dateOfBeforeMonth, now.AddDays(1 - todayOfMonth),dateOfBeforeMonth,now, dateOfBeforeMonth.Year, dateOfBeforeMonth.Month);
            }
            else//上个月当天的
            {
                if (!(todayOfMonth > lastDayOfBeforeMonth))
                {
                    ShopMonthOrderDal.GetInstance().Calc(dateOfBeforeMonth, dateOfBeforeMonth.AddDays(1),dateOfBeforeMonth,now, dateOfBeforeMonth.Year, dateOfBeforeMonth.Month);
                }
            }
        }
    }
}
