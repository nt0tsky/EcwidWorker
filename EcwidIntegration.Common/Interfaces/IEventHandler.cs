using EcwidIntegration.Common.Attributes;
using EcwidIntegration.Common.Events;

namespace EcwidIntegration.Common.Interfaces
{
    [Common]
    public interface IEventHandler
    {
        void Handle(string data);
    }
}
