using EcwidIntegration.Ecwid;
using EcwidIntegration.Ecwid.Models;
using EcwidIntegration.GoogleSheets;
using EcwidIntegration.Worker.CLI;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcwidIntegration.Worker.Jobs
{
    internal class OrderWriteJob : IJob
    {
        private readonly IWriter writer;
        private readonly IHandlerService handlerService;

        /// <summary>
        /// Получить информацию для записи
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <returns>Информация в виде списка</returns>
        private IList<object> GetOrders(OrderDTO order)
        {
            var list = new List<object>()
            {
                order.OrderNumber,
                order.ShippingMethod,
                order.CreateDate.ToString("dd.MM.yyyy"),
                order.ShippingPerson,
                order.OrderComments
            };
            var orderItems = order.Items.Select(i => GetDescription(i));
            foreach (var oiList in orderItems)
            {
                list.AddRange(oiList);
            }

            return list;
        }

        /// <summary>
        /// Получить описание заказа
        /// </summary>
        /// <param name="orderItem">Позиция заказа</param>
        /// <returns>Описание в виде списка</returns>
        private IList<string> GetDescription(OrderItemDTO orderItem)
        {
            var items = new List<string>();

            for (int i = 0; i < orderItem.Quantity; i++)
            {
                var builder = new StringBuilder();
                builder.AppendLine(orderItem.Name);
                foreach (var option in orderItem.Options)
                {
                    builder.AppendLine($"{option.Name} - {option.Value}");
                }
                items.Add(builder.ToString());
            }

            return items;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderWriteJob()
        {
            writer = new ConsoleWriter();
            handlerService = new HandlerService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var data = context.MergedJobDataMap;
            object options;
            if (data.TryGetValue("Options", out options))
            {
                var dataOptions = options as RunOptions;
                var ecwidService = new EcwidService(dataOptions.StoreId, dataOptions.EcwidAPI);
                var googleSheetService = new GoogleSheetsService(dataOptions.SpreadSheet);

                try
                {
                    var gsheetOrders = googleSheetService.GetOrdersNumbers(dataOptions.TabId);
                    var ecwidOrders = await ecwidService.GetPaidNotShippedOrdersAsyncWithCondition(o =>
                    {
                        return !gsheetOrders.Contains(o.OrderNumber);
                    });
                    if (ecwidOrders.Any())
                    {
                        foreach (var order in ecwidOrders.OrderBy(o => o.CreateDate))
                        {
                            handlerService.Handle<OrderDTO>(order);
                            try
                            {
                                googleSheetService.Write(dataOptions.TabId, GetOrders(order), dataOptions.BeginColumn);
                            }
                            catch (Exception e)
                            {
                                writer.Write(e.Message);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    writer.Write(e.Message);
                }
            }
        }
    }
}
