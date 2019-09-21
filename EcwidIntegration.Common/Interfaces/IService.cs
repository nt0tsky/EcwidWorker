using System;

namespace EcwidIntegration.Common.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Уникальный идентификатор сервиса
        /// </summary>
        Guid Uid { get; }
    }
}
