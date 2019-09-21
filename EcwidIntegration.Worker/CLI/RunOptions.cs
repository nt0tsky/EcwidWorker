using PowerArgs;

namespace EcwidIntegration.Worker.CLI
{
    internal class RunOptions : EcwidOptions
    {
        [ArgShortcut("ss"), ArgRequired, ArgDescription("Код документа в GoogleSheet")]
        public string SpreadSheet { get; set; }

        [ArgShortcut("ti"), ArgRequired, ArgDescription("Наименование вкладки в таблице GoogleSheet")]
        public string TabId { get; set; }

        [ArgShortcut("bc"), ArgRequired, ArgDescription("Стартовая колонка по оси OX")]
        public string BeginColumn { get; set; }

        [ArgShortcut("i"), ArgDescription("Интервал опроса сервиса Ecwid")]
        public int Interval { get; set; }
    }
}
