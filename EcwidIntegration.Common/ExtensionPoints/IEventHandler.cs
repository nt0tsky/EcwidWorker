using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Common.ExtensionPoints
{
    [ExtensionPoint]
    public interface IEventHandler
    {
        void Handle(string data);
    }
}
