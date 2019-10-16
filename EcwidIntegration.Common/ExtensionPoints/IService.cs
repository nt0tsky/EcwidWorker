using System;
using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Common.ExtensionPoints
{
    /// <summary>
    /// Интерфейс расширяемых сервисов
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
