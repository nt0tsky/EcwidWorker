using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Services;

namespace EcwidIntegration.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            IWorker worker = new WorkerService();
            worker.Start(args);
        }
    }
}
