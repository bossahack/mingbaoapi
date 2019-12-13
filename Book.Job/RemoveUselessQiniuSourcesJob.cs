using Book.Service;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    public class RemoveUselessQiniuSourcesJob
    {
        public void Execute(int retryCount = 0)
        {
            try
            {
                if (retryCount >= 3)
                {
                    return;
                }
                QiniuService.GetInstance().RemoveUnuseImg();
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
