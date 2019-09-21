using System;
using System.Threading;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Ecwid;
using EcwidIntegration.Worker.CLI;
using EcwidIntegration.Worker.Constants;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Jobs;
using Newtonsoft.Json;
using PowerArgs;
using Quartz;
using Quartz.Impl;

namespace EcwidIntegration.Worker.Services
{
    internal class WorkerService : IWorkerService, IService, IWorker
    {
        private readonly IWriter writer = new ConsoleWriter();

        /// <inhertidoc />
        public Guid Uid => new Guid("{355B8006-8CDB-48E5-99D0-1643A6A9036C}");

        public void Start(string[] args)
        {
            Args.InvokeAction<WorkerActions>(args);
        }

        public async void Run(RunOptions options)
        {
            writer.Write(Message.Method.Run);

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<OrderWriteJob>().Build();

            var trigger = TriggerBuilder.Create()                   // создаем триггер
                .WithIdentity("trigger1", "group1")                 // идентифицируем триггер с именем и группой
                .UsingJobData(new JobDataMap
                {
                    { "Options", options }
                })
                .StartNow()                                         // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x                          // настраиваем выполнение действия
                    .WithIntervalInMinutes(options.Interval)        // через 1 минуту
                    .RepeatForever())                               // бесконечное повторение
                .Build();

            await scheduler.ScheduleJob(job, trigger);              // начинаем выполнение работы

            Thread.Sleep(Timeout.Infinite);
        }

        public void GetEcwidOrders(EcwidOptions options)
        {
            writer.Write(Message.Method.GetOrders);

            var ecwidService = new EcwidService(options.StoreId, options.EcwidAPI);
            var orders = ecwidService.GetPaidNotShippedOrdersAsync().Result;
            writer.Write(JsonConvert.SerializeObject(orders));
        }
    }
}
