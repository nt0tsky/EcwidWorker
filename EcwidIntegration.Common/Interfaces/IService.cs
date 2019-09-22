using System;
using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Common.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса
    /// </summary>
    [Common]
    public interface IService
    {
        /// <summary>
        /// Уникальный идентификатор сервиса
        /// </summary>
        Guid Uid { get; }
    }
}
