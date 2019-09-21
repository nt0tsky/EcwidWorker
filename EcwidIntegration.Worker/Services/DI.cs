using EcwidIntegration.Common.Services;
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
        /// Инстанс контейнера
        /// </summary>
        private static ServiceProvider container;

        /// <summary>
        /// Инициализация контейнера
        /// </summary>
        private static void InitContainer()
        {
            var items = assemblyService.LoadCommon();
            var serviceProvider = new ServiceCollection();
            foreach (var item in items)
            {
                item.Implementations.ForEach(i =>
                {
                    serviceProvider.AddTransient(item.Interface, i);
                });
            }
            container = serviceProvider.BuildServiceProvider();
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
