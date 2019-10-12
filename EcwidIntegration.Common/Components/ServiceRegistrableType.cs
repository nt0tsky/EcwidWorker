using System;
using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Components
{
    [Component]
    class ServiceRegistrableType : IRegistrableType
    {
        public bool IsRegister(Type type)
        {
            return type.HasAttributeTypeOf<ServiceAttribute>();
        }

        public void Register(IList<RegisterItem> items, Type type)
        {
            var contracts = type.GetInterfaces();
            if (contracts.Any())
            {
                foreach (var contract in contracts)
                {
                    var item = items.FirstOrDefault(i => i.Interface == contract);
                    if (item != null)
                    {
                        item.Implementations.Add(type);
                    }
                    else
                    {
                        var ri = new RegisterItem
                        {
                            Interface = contract
                        };
                        ri.Implementations.Add(type);
                        items.Add(ri);
                    }
                }
            }
        }
    }
}
