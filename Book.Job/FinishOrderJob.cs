using Book.Dal;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    public class FinishOrderJob
    {
        public void Execute(int retryCount=0)
        {
            try
            {
                if (retryCount >= 3)
                {
                    return;
                }

                OrderDal.GetInstance().FinishOrder(DateTime.Now);
            }
            catch (Exception ex)
            {
                retryCount++;
                Execute(retryCount);
            }
        }       
    }

}
