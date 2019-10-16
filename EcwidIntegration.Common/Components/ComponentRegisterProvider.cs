using System;
using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.ExtensionPoints;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Components
{
    [Component]
    class ComponentRegisterProvider : IRegisterProvider
    {
        public bool IsRegister(Type type)
        {
            return type.HasAttributeTypeOf<ComponentAttribute>();
        }

        public void Register(IList<RegisterItem> items, Type type)
        {
            var extension = type.GetInterfaces().FirstOrDefault(i => i.HasAttributeTypeOf<ExtensionPointAttribute>());
            if (extension == null)
            {
                throw new ArgumentException("Класс помеченный аттрибутом ComponentAttribute " +
                                            "должен реализовывать интерфейс помеченный аттрибутом " +
                                            "ExtensionPoint для последующего расширения функционала");
            }
            else
            {
                var item = items.FirstOrDefault(i => i.Interface == extension);
                if (item != null)
                {
                    item.Implementations.Add(type);
                }
                else
                {
                    var ri = new RegisterItem
                    {
                        Interface = extension
                    };
                    ri.Implementations.Add(type);
                    items.Add(ri);
                }
            }
        }
    }
}
