using EcwidIntegration.Ecwid.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcwidIntegration.Ecwid.Interfaces
{
    public interface IEcwidService
    {
        /// <summary>
        /// Получить список новых заказов
        /// </summary>
        /// <returns></returns>
        Task<IList<OrderDTO>> GetPaidNotShippedOrdersAsync();

        /// <summary>
        /// Получить список новых заказов с условием отбора
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<OrderDTO>> GetPaidNotShippedOrdersAsyncWithCondition(Func<OrderDTO, bool> condition);
    }
}
