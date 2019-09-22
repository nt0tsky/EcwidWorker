using EcwidIntegration.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Common.Interfaces
{
    [Common]
    public interface IOnBuild
    {
        void OnBuild(ServiceProvider container);
    }
}
