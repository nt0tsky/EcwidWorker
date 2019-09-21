using System.Collections.Generic;

namespace EcwidIntegration.Ecwid.Models
{
    public class OrderItemDTO
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public IList<OrderItemOptionDTO> Options { get; set; }
    }
}
