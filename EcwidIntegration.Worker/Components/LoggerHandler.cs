using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.ExtensionPoints;
using NLog;

namespace EcwidIntegration.Worker.Components
{
    [Component]
    class LoggerHandler : IEventHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Handle(string args)
        {
            logger.Info(args);
        }
    }
}
