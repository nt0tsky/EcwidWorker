using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.ExtensionPoints;
using EcwidIntegration.Worker.Interfaces;
using Newtonsoft.Json;

namespace EcwidIntegration.Worker.Services
{
    [Component]
    internal class HandlerService : IHandlerService
    {
        private readonly IEnumerable<IEventHandler> handlers;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="handlers">Перехватчики</param>
        public HandlerService(IEnumerable<IEventHandler> handlers)
        {
            this.handlers = handlers;
        }

        public void Handle<T>(T order)
        {
            if (handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    handler.Handle(JsonConvert.SerializeObject(order));
                }
            }
        }
    }
}
