using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecwid;
using EcwidIntegration.Ecwid.Interfaces;
using EcwidIntegration.Ecwid.Models;

namespace EcwidIntegration.Ecwid
{
    public class EcwidService : IEcwidService
    {
        private readonly IEcwidClient ecwidClient;
        private readonly OrderHelper orderHelper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="storeId">Id магазина</param>
        /// <param name="accessToken">API token</param>
        public EcwidService(int shopId, string accessToken)
        {
            this.ecwidClient = new EcwidClient(shopId, accessToken);
            this.orderHelper = new OrderHelper();
        }

        /// <summary>
        /// Получить список заказов
        /// </summary>
        /// <param name="limit">Лимит выгрузки</param>
        /// <returns>Список заказов</returns>
        public async Task<IEnumerable<OrderDTO>> GetOrders(double limit)
        {
            var orders = await this.ecwidClient.GetOrdersAsync(new { limit = limit });
            if (orders.Any())
            {
                return orders.Select(i => orderHelper.CreateOrderDTO(i));
            }

            return Enumerable.Empty<OrderDTO>();
        }

        /// <summary>
        /// Получить список неотправленных заказов
        /// </summary>
        /// <param name="condition">Список внесенных заказов в таблицу</param>
        /// <returns>Список заказов</returns>
        public async Task<IList<OrderDTO>> GetPaidNotShippedOrdersAsync()
        {
            try
            {
                var result = new List<OrderDTO>();
                var orders = await this.ecwidClient.GetPaidNotShippedOrdersAsync();
                result = orders.Select(i => orderHelper.CreateOrderDTO(i)).ToList();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Получить список неотправленных заказов с условием отбора
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <returns>Список заказов</returns>
        public async Task<IList<OrderDTO>> GetPaidNotShippedOrdersAsyncWithCondition(Func<OrderDTO, bool> condition)
        {
            var orders = await GetPaidNotShippedOrdersAsync();
            return orders.Where(o => condition(o)).ToList();
        }
    }
}
