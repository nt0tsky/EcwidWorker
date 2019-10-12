using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Common.Services
{
    [Service]
    public class EcwidServiceCollection : IEcwidServiceCollection
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        private static ServiceProvider Container { get; set; }

        /// <summary>
        /// Сервис работы со сборками
        /// </summary>
        private readonly AssemblyService assemblyService = new AssemblyService();

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly IWriter writer = new ConsoleWriter();

        /// <summary>
        /// Ctor
        /// </summary>
        public EcwidServiceCollection()
        {
            InitContainer();
        }

        /// <summary>
        /// Инициализация контейнера
        /// </summary>
        private void InitContainer()
        {
            if (Container != null)
            {
                return;
            }

            writer.Write("Инициализация контейнера");
            var items = assemblyService.LoadCommon();
            writer.Write($"Количество элементов для регистрации {items.Count}");
            var serviceProvider = new ServiceCollection();
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
            Container = serviceProvider.BuildServiceProvider();
            var onBuildClasses = Container.GetService<IEnumerable<IOnBuild>>();
            if (onBuildClasses.Any())
            {
                foreach(var item in onBuildClasses)
                {
                    item.OnBuild(Container);
                }
            }
            writer.Write("Регистрация окончена");
        }

        /// <summary>
        /// Получить сервис\компонент
        /// </summary>
        /// <typeparam name="T">Тип компонента контейнера</typeparam>
        /// <returns>Инстанс объекта</returns>
        public T GetService<T>()
        {
            return Container.GetService<T>(); ;
        }
    }
}
