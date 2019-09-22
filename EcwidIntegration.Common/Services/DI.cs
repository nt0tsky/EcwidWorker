using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Common.Services
{
    public static class DI
    {
        /// <summary>
        /// Сервис работы со сборками
        /// </summary>
        private static readonly AssemblyService assemblyService = new AssemblyService();

        /// <summary>
        /// Логгер
        /// </summary>
        private static readonly IWriter writer = new ConsoleWriter();

        /// <summary>
        /// Инстанс контейнера
        /// </summary>
        private static ServiceProvider container;

        /// <summary>
        /// Инициализация контейнера
        /// </summary>
        public static void InitContainer()
        {
            writer.Write("Инициализация контейнера");
            var items = assemblyService.LoadCommon();
            writer.Write($"Количество элементов для регистрации {items.Count}");
            var serviceProvider = new ServiceCollection();
            foreach (var item in items)
            {
                foreach (var impl in item.Implementations)
                {
                    serviceProvider.AddTransient(item.Interface, impl);
                }
            }
            container = serviceProvider.BuildServiceProvider();
            var onBuildClasses = container.GetService<IEnumerable<IOnBuild>>();
            if (onBuildClasses.Any())
            {
                foreach(var item in onBuildClasses)
                {
                    item.OnBuild(container);
                }
            }
            writer.Write("Регистрация окончена");
        }

        /// <summary>
        /// Контейнер
        /// </summary>
        public static ServiceProvider Container { get { return container; } }
    }
}
