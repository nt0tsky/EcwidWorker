using EcwidIntegration.Worker.CLI;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Services;
using PowerArgs;

namespace EcwidIntegration.Worker
{
    /// <summary>
    /// Опции запуска
    /// </summary>
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class WorkerActions : IWorkerActions
    {
        private readonly IWorkerService workerService = new WorkerService();

        [HelpHook, ArgShortcut("--?"), ArgDescription("Отобразить справку")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgShortcut("r"), ArgDescription("Запустить анализ заказов и выгрузку в GoogleSheet")]
        public void Run(RunOptions options)
        {
            workerService.Run(options);
        }

        [ArgActionMethod, ArgShortcut("eo"), ArgDescription("Получить список новых заказов из системы Ecwid")]
        public void GetEcwidOrders(EcwidOptions options)
        {
            workerService.GetEcwidOrders(options);
        }
    }
}
