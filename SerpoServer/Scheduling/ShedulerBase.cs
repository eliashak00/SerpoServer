using System;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using SerpoServer.Scheduling.Jobs;

namespace SerpoServer.Sheduling
{
    public static class ShedulerBase
    {
        private static IScheduler Scheduler;

        public static async void StopSheduler()
        {
            await Scheduler.Shutdown();
        }

        public static async void StartSheduler()
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                var props = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary"}
                };
                var factory = new StdSchedulerFactory(props);
                var scheduler = await factory.GetScheduler();

                await scheduler.Start();

                var job = JobBuilder.Create<CacheJob>()
                    .WithIdentity("fileCache", "main")
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity("hourly", "mainTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInHours(1)
                        .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                Scheduler = scheduler;
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}