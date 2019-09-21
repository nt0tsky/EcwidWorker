using EcwidIntegration.Ecwid;
using EcwidIntegration.Ecwid.Models;
using EcwidIntegration.GoogleSheets;
using EcwidIntegration.Worker.CLI;
using EcwidIntegration.Worker.Interfaces;
using EcwidIntegration.Worker.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcwidIntegration.Worker.Jobs
{
    internal class OrderWriteJob
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

        public void Execute(RunOptions options)
        {
            writer.Write(JsonConvert.SerializeObject(options));
            writer.Write($"Точка входа в тело метода Execute");
            var ecwidService = new EcwidService(options.StoreId, options.EcwidAPI);
            writer.Write("Сервис Ecwid проинициализирован успешно!");
            var googleSheetService = new GoogleSheetsService(options.SpreadSheet);
            writer.Write("Инициализация сервисов завершена");
            try
            {
                var gsheetOrders = googleSheetService.GetOrdersNumbers(options.TabId);
                writer.Write("Получили список заказов с GoogleSheet");
                var ecwidOrders = ecwidService.GetPaidNotShippedOrdersAsyncWithCondition(o =>
                {
                    return !gsheetOrders.Contains(o.OrderNumber);
                }).Result;
                writer.Write("Получили список заказов с Ecwid");
                if (ecwidOrders.Any())
                {
                    writer.Write($"Новые заказы для записи! {ecwidOrders.Count}");
                    foreach (var order in ecwidOrders.OrderBy(o => o.CreateDate))
                    {
                        handlerService.Handle<OrderDTO>(order);
                        try
                        {
                            googleSheetService.Write(options.TabId, GetOrders(order), options.BeginColumn);
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
