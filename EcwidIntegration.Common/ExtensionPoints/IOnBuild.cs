using EcwidIntegration.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.Common.ExtensionPoints
{
    [ExtensionPoint]
    public interface IOnBuild
    {
        void OnBuild(ServiceProvider container);
    }
}
