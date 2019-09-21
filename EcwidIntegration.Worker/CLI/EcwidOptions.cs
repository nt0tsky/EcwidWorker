using PowerArgs;

namespace EcwidIntegration.Worker.CLI
{
    internal class EcwidOptions
    {
        [ArgShortcut("-s"), ArgRequired, ArgDescription("Уникальный номер магазина в системе Ecwid")]
        public int StoreId { get; set; }

        [ArgShortcut("-ea"), ArgRequired, ArgDescription("API-ключ в системе Ecwid")]
        public string EcwidAPI { get; set; }
    }
}
