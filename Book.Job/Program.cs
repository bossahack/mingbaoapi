using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Book.Job
{
    class Program
    {        
        static void Main(string[] args)
        {
            Book.Dal.Model.ColumnMapper.SetMapper();
            //ManualExe();
            FeeJobScheduler.start().GetAwaiter().GetResult();
            while (true)
            {
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 手动执行
        /// </summary>
        static void ManualExe()
        {
            Console.WriteLine("begin");
            new FinishOrderJob().Execute();
            new CalcShopDayOrderJob().Execute();
            new CalcShopMonthOrderJob().Execute();
            new CloseUnPayShopJob().Execute();
            new CalcUserFee().Execute();
            new RemoveUselessQiniuSourcesJob().Execute();
            Console.WriteLine("end");
        }
    }


    public class FeeJobScheduler
    {
        public static async Task start()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job1 = JobBuilder.Create<FeeJob>()
               .WithIdentity("FeeJob")
               .Build();
            ITrigger trigger1 = TriggerBuilder.Create()
              .WithIdentity("FeeJobTrigger")
              .StartNow()
              .WithCronSchedule("0 5 0 * * ?")//每日0点5分执行一次
              .Build();
            await scheduler.ScheduleJob(job1, trigger1);


            //IJobDetail job2 = JobBuilder.Create<CalcShopDayOrder>()
            //   .WithIdentity("CalcShopDayOrderJob")
            //   .Build();
            //ITrigger trigger2 = TriggerBuilder.Create()
            //  .WithIdentity("CalcShopDayOrderTrigger")
            //  .StartNow()
            //  .WithCronSchedule("0 56 11 * * ?")//每日0点执行一次
            //  .Build();
            //await scheduler.ScheduleJob(job2, trigger2);
        }

    }

    public class FeeJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("begin");
                new FinishOrderJob().Execute();
                new CalcShopDayOrderJob().Execute();
                new CalcShopMonthOrderJob().Execute();
                new CloseUnPayShopJob().Execute();
                new CalcUserFee().Execute();
                new CalcUserShopOrder().Execute();
                new RemoveUselessQiniuSourcesJob().Execute();
                Console.WriteLine("end");
            });
        }
    }

   

}
