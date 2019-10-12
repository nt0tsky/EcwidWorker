using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Services;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace EcwidIntegration.Worker
{
    class Program
    {
        /// <summary>
        /// Инициализация контейнера
        /// </summary>
        /// <returns>контейнер зависимостей</returns>
        static IEcwidServiceCollection GetServiceCollection()
        {
            return new EcwidServiceCollection();
        }

        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                var eService = GetServiceCollection();
                var worker = eService.GetService<IWorker>();
                
                worker.Start(args);
            });

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
