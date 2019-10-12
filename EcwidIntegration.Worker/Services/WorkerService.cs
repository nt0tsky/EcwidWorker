using System;
using System.Threading;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Services;
using EcwidIntegration.Ecwid;
using EcwidIntegration.Worker.CLI;
using EcwidIntegration.Worker.Constants;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Jobs;
using Newtonsoft.Json;
using PowerArgs;

namespace EcwidIntegration.Worker.Services
{
    [Service]
    internal class WorkerService : IWorkerService, IService, IWorker
    {
        private readonly IWriter writer = new ConsoleWriter();
        private readonly IWriteJob writeJob;

        /// <inhertidoc />
        public Guid Uid => new Guid("{355B8006-8CDB-48E5-99D0-1643A6A9036C}");

        public WorkerService(IWriteJob writeJob)
        {
            this.writeJob = writeJob;
        }

        public void Start(string[] args)
        {
            Args.InvokeAction<WorkerActions>(args);
        }

        public void Run(RunOptions options)
        {
            writer.Write(Message.Method.Run);
            writer.Write("Инициализация job'a завершена");
            while(true)
            {
                writer.Write("Итерация получения данных..");
                writeJob.Execute(options);

                Thread.Sleep(options.Interval * 60000);
            }
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
