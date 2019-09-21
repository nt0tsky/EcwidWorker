using EcwidIntegration.Common.Services;
using EcwidIntegration.Worker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PowerArgs;

namespace EcwidIntegration.Worker.Services
{
    internal static class DI
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
        private static void InitContainer()
        {
            writer.Write("Инициализация контейнера");
            var items = assemblyService.LoadCommon();
            writer.Write($"Количество элементов для регистрации ${items.Count}");
            var serviceProvider = new ServiceCollection();
            foreach (var item in items)
            {
                item.Implementations.ForEach(i =>
                {
                    serviceProvider.AddTransient(item.Interface, i);
                });
            }
            container = serviceProvider.BuildServiceProvider();

            writer.Write("Регистрация окончена");
        }

        /// <summary>
        /// Контейнер
        /// </summary>
        public static ServiceProvider Container
        {
            get
            {
                if (container == null)
                {
                    InitContainer();
                }

                return container;
            }
        }
    }
}
