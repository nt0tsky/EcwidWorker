using EcwidIntegration.Worker.CLI;

namespace EcwidIntegration.Worker.Jobs
{
    internal interface IWriteJob
    {
        void Execute(RunOptions options);
    }
}