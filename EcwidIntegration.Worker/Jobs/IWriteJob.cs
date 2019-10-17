using System.Threading.Tasks;
using EcwidIntegration.Worker.CLI;

namespace EcwidIntegration.Worker.Jobs
{
    internal interface IWriteJob
    {
        Task Execute(RunOptions options);
    }
}