using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Worker.Interfaces
{
    [ExtensionPoint]
    internal interface IHandlerService
    {
        void Handle<T>(T order);
    }
}
