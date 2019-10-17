using System;
using EcwidIntegration.Common.Interfaces;
using NLog;

namespace EcwidIntegration.Worker.Components
{
    internal class LoggerWriter : IWriter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Write(string message)
        {
            logger.Debug(message);
        }
    }
}
