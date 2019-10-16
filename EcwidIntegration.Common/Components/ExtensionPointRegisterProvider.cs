using System;
using System.Collections.Generic;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.ExtensionPoints;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Components
{
    [Component]
    class ExtensionPointRegisterProvider : IRegisterProvider
    {
        public bool IsRegister(Type type)
        {
            return type.HasAttributeTypeOf<ExtensionPointAttribute>();
        }

        public void Register(IList<RegisterItem> items, Type type)
        {
            var item = new RegisterItem
            {
                Interface = type
            };
            items.Add(item);
        }
    }
}
