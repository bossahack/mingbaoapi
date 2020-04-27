using Book.Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    public class CalcUserShopOrder
    {
        /// <summary>
        /// 同一天内不可多次执行
        /// </summary>
        /// <param name="retryCount"></param>
        public void Execute(int retryCount = 0)
        {
            try
            {
                if (retryCount >= 1)
                {
                    return;
                }
                UserShopDal.GetInstance().CalcUserShopOrder(DateTime.Now.AddDays(-1));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                retryCount++;
                Execute(retryCount);
            }
        }
    }
}
