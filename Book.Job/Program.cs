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
        static string title = "job,不可关闭";
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        static void DisableClosebtn()
        {
            //与控制台标题名一样的路径
            string fullPath = System.Environment.CurrentDirectory + "\\Book.Job.exe";
            //根据控制台标题找控制台
            var windowHandle = FindWindow(null, fullPath);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }
        static void Main(string[] args)
        {
            DisableClosebtn();
            Book.Dal.Model.ColumnMapper.SetMapper();
            FeeJobScheduler.start().GetAwaiter().GetResult();
            while (true)
            {
                Console.ReadLine();
            }
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
              .WithCronSchedule("0 05 00 * * ?")//每日0点5分执行一次
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
                Console.WriteLine("end");
            });
        }
    }

}
