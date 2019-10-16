using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Extensions;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.ExtensionPoints;
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
        /// Указатель на реализацию
        /// </summary>
        private readonly IEcwidServiceCollectionImpl ecwidServiceCollectionImpl;

        /// <summary>
        /// Ctor
        /// </summary>
        public EcwidServiceCollection(IEcwidServiceCollectionImpl ecwidServiceCollectionImpl)
        {
            this.ecwidServiceCollectionImpl = ecwidServiceCollectionImpl;
        }

        /// <summary>
        /// Инициализация контейнера
        /// </summary>
        public void Init()
        {
            var serviceProvider = new ServiceCollection();
            var items = assemblyService.LoadCommon();

            ecwidServiceCollectionImpl.Register(serviceProvider, items);

            Container = serviceProvider.BuildServiceProvider();

            var onBuild = Container.GetService<IEnumerable<IOnBuild>>();
            if (onBuild.Any())
            {
                foreach(var item in onBuild)
                {
                    item.OnBuild(Container);
                }
            }
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
