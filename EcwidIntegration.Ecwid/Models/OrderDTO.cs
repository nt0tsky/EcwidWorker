using System;
using System.Collections.Generic;

namespace EcwidIntegration.Ecwid.Models
{
    public class OrderDTO
    {
        /// <summary>
        /// Метод отправки
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public string ShippingPerson { get; set; }

        /// <summary>
        /// Комментарии к заказу
        /// </summary>
        public string OrderComments { get; set; }

        /// <summary>
        /// Сумма заказа
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Номер заказа
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Элементы заказа
        /// </summary>
        public IList<OrderItemDTO> Items { get; set; }
    }
}
