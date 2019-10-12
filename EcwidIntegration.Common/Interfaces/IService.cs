using System;
using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Common.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса
    /// </summary>
    [ExtensionPoint]
    public interface IService
    {
        /// <summary>
        /// Уникальный идентификатор сервиса
        /// </summary>
        Guid Uid { get; }
    }
}
