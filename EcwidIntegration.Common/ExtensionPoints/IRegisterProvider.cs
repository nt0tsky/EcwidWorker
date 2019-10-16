using System;
using System.Collections.Generic;
using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Models;

namespace EcwidIntegration.Common.ExtensionPoints
{
    /// <summary>
    /// Точка расширения для добавления типов регистрации
    /// </summary>
    [ExtensionPoint]
    public interface IRegisterProvider
    {
        /// <summary>
        /// Можно ли зарегать в контейнере
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Успех</returns>
        bool IsRegister(Type type);

        /// <summary>
        /// Зарегистрировать тип и вернуть коллекцию
        /// </summary>
        /// <param name="items">Список элементов регистрации</param>
        /// <param name="type">Итерируемый объект в очереди на регистрацию</param>
        void Register(IList<RegisterItem> items, Type type);
    }
}
