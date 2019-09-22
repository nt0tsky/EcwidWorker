using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Services
{
    public class AssemblyService
    {
        private readonly IWriter writer;
        private readonly AssemblyListener assemblyListener;
        private IList<RegisterItem> assemblyItems;
        private bool initialized;

        /// <summary>
        /// Ctor
        /// </summary>
        public AssemblyService()
        {
            initialized = false;
            this.writer = new ConsoleWriter();
            this.assemblyListener = new AssemblyListener
            {
                EnableWatcher = true,
                OnChange = (fileName) =>
                {
                    writer.Write($"Обновление файла директории {fileName}");
                    initialized = false;
                }
            };

            this.assemblyListener.Start();
        }

        /// <summary>
        /// Общий тип
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Успех</returns>
        protected bool IsCommonType(Type type)
        {
            if (!type.IsInterface)
            {
                return type.GetInterfaces().Any(i => i.IsCommon());
            }

            return false;
        }

        /// <summary>
        /// Добавить элементы
        /// </summary>
        /// <param name="items">Элементы</param>
        /// <param name="assembly"Сборка</param>
        protected void AppendItems(IList<RegisterItem> items, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(t => IsCommonType(t)))
            {
                var commonIfaces = type.GetInterfaces().Where(i => i.IsCommon());
                foreach (var ci in commonIfaces)
                {
                    var ri = items.FirstOrDefault(i => ci == i.Interface);
                    if (ri != null)
                    {
                        ri.Implementations.Add(type);
                    }
                    else
                    {
                        var item = new RegisterItem
                        {
                            Interface = ci
                        };
                        item.Implementations.Add(type);
                        items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузить сборки
        /// </summary>
        /// <returns>Список элементов для регистрации в <see cref="DI"/></returns>
        public IList<RegisterItem> LoadCommon()
        {
            if (!initialized)
            {
                assemblyItems = new List<RegisterItem>();

                foreach (var assembly in assemblyListener.GetAssemblies())
                {
                    AppendItems(assemblyItems, assembly);
                }
            }

            return assemblyItems;
        }
    }
}
