using System;
using System.Collections.Generic;
using System.Text;

namespace EcwidIntegration.Common.Events
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Event { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sender"></param>
        public GenericEventArgs(T @event)
        {
            this.Event = @event;
        }
    }
}
