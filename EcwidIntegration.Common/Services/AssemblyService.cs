using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Services
{
    public class AssemblyService
    {
        /// <summary>
        /// Текущая директория
        /// </summary>
        private readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        public IList<RegisterItem> LoadCommon()
        {
            var items = new List<RegisterItem>();
            var paths = Directory.GetFiles(CurrentDirectory, "*.dll");

            foreach(var path in paths)
            {
                var assembly = Assembly.LoadFrom(path);

                foreach(var type in assembly.GetTypes())
                {
                    if (type.IsInterface)
                    {
                        if (type.IsCommon())
                        {
                            items.Add(new RegisterItem
                            {
                                Interface = type
                            });
                        }
                    }
                    else
                    {
                        var interfaces = type.GetInterfaces();
                        foreach(var iface in interfaces)
                        {
                            if (iface.IsCommon())
                            {
                                var item = items.FirstOrDefault(i => i.Interface.GetType() == iface.GetType());
                                if (item != null)
                                {
                                    item.Implementations.Add(type);
                                }
                                else
                                {
                                    item = new RegisterItem
                                    {
                                        Interface = iface
                                    };
                                    item.Implementations.Add(type);
                                    items.Add(item);
                                }
                            }
                        }
                    }
                }
            }

            return items;
        }
    }
}
