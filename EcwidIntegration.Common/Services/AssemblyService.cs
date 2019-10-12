using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Services
{
    public class AssemblyService
    {
        private IList<IRegistrableType> RegistrableTypes { get; set; }

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
        protected bool IsComponentType(Type type)
        {
            if (!type.IsInterface)
            {
                return type.GetInterfaces().Any(i => i.HasAttributeTypeOf<ComponentAttribute>());
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
            foreach (var type in assembly.GetTypes().Where(t => RegistrableTypes.Any(i => i.IsRegister(t))))
            {
                var regType = RegistrableTypes.FirstOrDefault(i => i.IsRegister(type));
                regType.Register(items, type);
            }
        }

        protected void BeforeAppendItems(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(i =>
            {
                return i.GetTypes().Where(t =>
                {
                    return t.IsClass && t.HasAttributeTypeOf<ComponentAttribute>();
                });
            }).Where(t =>
            {
                return t.GetInterfaces().Any(i => i == typeof(IRegistrableType));
            });

            RegistrableTypes = types.Select(i => Activator.CreateInstance(i) as IRegistrableType).ToList();
        }

        protected void AfterAppendItems(IEnumerable<Assembly> assemblies)
        {
            //todo
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
                var assemblies = assemblyListener.GetAssemblies();
                BeforeAppendItems(assemblies);
                foreach (var assembly in assemblies)
                {
                    AppendItems(assemblyItems, assembly);
                }
                AfterAppendItems(assemblies);
            }

            return assemblyItems;
        }
    }
}
