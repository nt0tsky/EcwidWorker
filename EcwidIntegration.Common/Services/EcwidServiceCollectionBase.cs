using System.Collections.Generic;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Common.Services
{
    public class EcwidServiceCollectionBase : IEcwidServiceCollectionImpl
    {
        public void Register(ServiceCollection serviceProvider, IList<RegisterItem> items)
        {
            foreach (var item in items)
            {
                foreach (var impl in item.Implementations)
                {
                    if (impl.HasAttributeTypeOf<ServiceAttribute>())
                    {
                        serviceProvider.AddSingleton(item.Interface, impl);
                    }
                    else
                    {
                        serviceProvider.AddTransient(item.Interface, impl);
                    }
                }
            }
        }
    }
}
