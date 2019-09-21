using Ecwid.Models;
using EcwidIntegration.Ecwid.Models;
using System.Linq;

namespace EcwidIntegration.Ecwid
{
    /// <summary>
    /// Класс методов-хелперов
    /// </summary>
    internal class OrderHelper
    {
        /// <summary>
        /// Создать OrderDTO из модели заказа Ecwid
        /// </summary>
        /// <param name="order">Модель заказа</param>
        /// <returns>OrderDTO</returns>
        public OrderDTO CreateOrderDTO(OrderEntry order)
        {
            return new OrderDTO()
            {
                ShippingMethod = order.ShippingOptionInfo?.ShippingMethodName,
                ShippingPerson = order.ShippingPerson?.Name,
                Total = order.Total,
                OrderNumber = order.OrderNumber,
                CreateDate = order.CreateDate,
                Items = order.Items.Select(i => CreateOrderItemDTO(i)).ToList()
            };
        }

        /// <summary>
        /// Создать OrderItemDTO из модели OrderItem
        /// </summary>
        /// <param name="orderItem">Элемент заказа</param>
        /// <returns>DTO</returns>
        public OrderItemDTO CreateOrderItemDTO(OrderItem orderItem)
        {
            return new OrderItemDTO
            {
                Name = orderItem.Name,
                Quantity = orderItem.Quantity,
                Options = orderItem.SelectedOptions.Select(op => new OrderItemOptionDTO
                {
                    Name = op.Name,
                    Value = op.Value
                }).ToList()
            };
        }
    }
}
