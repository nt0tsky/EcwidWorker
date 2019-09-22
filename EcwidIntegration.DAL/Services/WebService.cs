using System.Threading;
using System.Threading.Tasks;
using EcwidIntegration.Common.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace EcwidIntegration.DAL.Services
{
    internal class WebWorker : IOnBuild
    {
        private static Thread thread;

        public void OnBuild(ServiceProvider container)
        {
            if (thread == null)
            {
                thread = new Thread(() =>
                {
                    WebHost.CreateDefaultBuilder()
                            .UseKestrel(op =>
                            {
                                op.Limits.MaxConcurrentConnections = 100;
                            })
                            .UseStartup<Startup>()
                            .Build().Run();
                });

                thread.Start();
            }
        }
    }
}
