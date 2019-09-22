using System;
using System.Collections.Generic;
using System.Text;

namespace EcwidIntegration.DAL.Models
{
    public class OrderItemOption
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
