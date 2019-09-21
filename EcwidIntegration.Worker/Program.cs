using System.Threading;
using System.Threading.Tasks;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Services;

namespace EcwidIntegration.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                IWorker worker = new WorkerService();
                worker.Start(args);
            });

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
