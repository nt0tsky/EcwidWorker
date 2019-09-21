using EcwidIntegration.Worker.CLI;

namespace EcwidIntegration.Worker.Interfaces
{
    interface IWorkerService
    {
        void Run(RunOptions options);

        void GetEcwidOrders(EcwidOptions options);
    }
}
