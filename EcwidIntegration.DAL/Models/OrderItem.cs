using System;
using System.Collections.Generic;

namespace EcwidIntegration.DAL.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public IList<OrderItemOption> Options { get; set; }
    }
}
