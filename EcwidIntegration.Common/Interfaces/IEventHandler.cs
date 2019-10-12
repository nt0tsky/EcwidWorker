using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Events;

namespace EcwidIntegration.Common.Interfaces
{
    [ExtensionPoint]
    public interface IEventHandler
    {
        void Handle(string data);
    }
}
