using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Worker.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Worker.Services
{
    internal class HandlerService : IHandlerService
    {
        public void Handle<T>(T order)
        {
            var handlers = DI.Container.GetService<IEnumerable<IEventHandler>>();
            if (handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    handler.Handle(order);
                }
            }
        }
    }
}
