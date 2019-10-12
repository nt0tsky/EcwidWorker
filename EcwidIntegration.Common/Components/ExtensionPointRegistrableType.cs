using System;
using System.Collections.Generic;
using System.Text;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Components
{
    [Component]
    class ExtensionPointRegistrableType : IRegistrableType
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
