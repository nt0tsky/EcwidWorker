using System.Threading.Tasks;
using EcwidIntegration.Worker.CLI;

namespace EcwidIntegration.Worker.Interfaces
{
    interface IWorkerService
    {
        Task Run(RunOptions options);

        void GetEcwidOrders(EcwidOptions options);
    }
}
