using System;
using System.Collections.Generic;
using System.Text;

namespace EcwidIntegration.Worker.Models
{
    public class KeyValue<T, E>
    {
        public T Key { get; set; }

        public E Value { get; set; }
    }
}
