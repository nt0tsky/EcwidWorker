using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.ExtensionPoints;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.Common.Services;
using EcwidIntegration.Ecwid;
using EcwidIntegration.Ecwid.Models;
using EcwidIntegration.GoogleSheets;
using EcwidIntegration.GoogleSheets.Models;
using EcwidIntegration.Worker.CLI;
using EcwidIntegration.Worker.Components;
using EcwidIntegration.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcwidIntegration.Worker.Jobs
{
    [Service]
    internal class OrderWriteJob : IWriteJob, IService
    {
        private EcwidService ecwidService;
        private SheetService sheetService;
        private readonly IWriter writer;
        private readonly IHandlerService handlerService;

        public Guid Uid => Guid.NewGuid();

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

        private EcwidService GetEcwidService(RunOptions options)
        {
            if (this.ecwidService == null)
            {
                this.ecwidService = new EcwidService(options.StoreId, options.EcwidAPI);
            }

            return this.ecwidService;
        }

        private SheetService GetSheetService(RunOptions options)
        {
            if (this.sheetService == null)
            {
                this.sheetService = new SheetService(new SheetsParams
                {
                    SheetId = options.SpreadSheet,
                    ApplicationName = "GoogleSheetsWriter_FCAA338E-426A-44CB-8474-200048847DBD",
                    CredentialsName = "credentials.json"
                });
            }

            return this.sheetService;
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
        public OrderWriteJob(IHandlerService handlerService)
        {
            writer = new LoggerWriter();
            this.handlerService = handlerService;
        }

        public async Task Execute(RunOptions options)
        {
            var ecwidService = GetEcwidService(options);
            var sheetService = GetSheetService(options);
            try
            {
                var exists = sheetService.GetOrdersNumbers(options.TabId);
                var ecwidOrders = await ecwidService.GetPaidNotShippedOrdersAsyncWithExclude(exists);
                if (ecwidOrders.Any())
                {
                    foreach (var order in ecwidOrders.OrderBy(o => o.CreateDate))
                    {
                        handlerService.Handle<OrderDTO>(order);
                        try
                        {
                            sheetService.Write(GetOrders(order), options.TabId, options.BeginColumn);
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
