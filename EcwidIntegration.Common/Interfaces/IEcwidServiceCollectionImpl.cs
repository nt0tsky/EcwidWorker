using System;
using System.Collections.Generic;
using System.Text;
using EcwidIntegration.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Common.Interfaces
{
    public interface IEcwidServiceCollectionImpl
    {
        void Register(ServiceCollection serviceProvider, IList<RegisterItem> items);
    }
}
