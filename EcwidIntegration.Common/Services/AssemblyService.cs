using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.ExtensionPoints;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.Services
{
    public class AssemblyService
    {
        private IList<IRegisterProvider> RegisterProviders { get; set; }

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
        /// Добавить элементы
        /// </summary>
        /// <param name="items">Элементы</param>
        /// <param name="assembly"Сборка</param>
        protected void AppendItems(IList<RegisterItem> items, Type type)
        {
            var provider = RegisterProviders.FirstOrDefault(i => i.IsRegister(type));
            if (provider != null)
            {
                provider.Register(items, type);
            }
        }

        protected void BeforeAppendItems(IEnumerable<Type> assemblies)
        {
            RegisterProviders = assemblies.Where(i => i.HasAttributeTypeOf<ComponentAttribute>()
                                    && typeof(IRegisterProvider).IsAssignableFrom(i)).Select(type =>
                                    {
                                        return Activator.CreateInstance(type) as IRegisterProvider;
                                    }).ToList();
        }

        protected void AfterAppendItems(IEnumerable<Assembly> assemblies)
        {
            //todo
        }

        /// <summary>
        /// Получить список типов для регистрации
        /// </summary>
        /// <param name="assemblies">Сборки</param>
        /// <returns>Список сборок</returns>
        protected IEnumerable<Type> GetCommonTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(i =>
            {
                return i.GetTypes().Where(t =>
                {
                    return t.HasAttributeTypeOf<ComponentAttribute>()
                           || t.HasAttributeTypeOf<ExtensionPointAttribute>()
                           || t.HasAttributeTypeOf<ServiceAttribute>();
                });
            });
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
                var types = GetCommonTypes(assemblies);
                BeforeAppendItems(types);
                foreach (var type in types)
                {
                    AppendItems(assemblyItems, type);
                }
                AfterAppendItems(assemblies);
            }

            return assemblyItems.Where(i => i.Implementations.Any()).ToList();
        }
    }
}
