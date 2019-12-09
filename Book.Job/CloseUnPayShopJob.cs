using Book.Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    public class CloseUnPayShopJob
    {
        public void Execute(int retryCount = 0)
        {
            try
            {
                if (retryCount >= 3)
                {
                    return;
                }

                ShopDal.GetInstance().CloseUnPayShop();
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
