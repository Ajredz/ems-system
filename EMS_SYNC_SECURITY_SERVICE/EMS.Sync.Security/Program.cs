using EMS.Sync.Security.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMS.Sync.Security.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SystemUserWorkerService>();
                    services.AddHostedService<SystemRoleWorkerService>();
                    //services.AddHostedService<IntegrateSystemUserWorkerService>();
                });
    }
}