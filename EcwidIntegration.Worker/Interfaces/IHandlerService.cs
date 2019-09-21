using System;
using System.Collections.Generic;
using System.Text;

namespace EcwidIntegration.Worker.Interfaces
{
    internal interface IHandlerService
    {
        void Handle<T>(T order);
    }
}
