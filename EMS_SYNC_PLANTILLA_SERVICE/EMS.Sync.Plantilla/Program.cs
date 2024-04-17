using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlantillaToRecruitment.Worker;
using Microsoft.Extensions.Logging;

namespace EMS.PlantillaToRecruitment.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging(loggerFactory => loggerFactory.AddEventLog())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<PositionWorkerService>();
                    services.AddHostedService<PositionLevelWorkerService>();
                    services.AddHostedService<OrgGroupWorkerService>();
                    services.AddHostedService<OrgGroupPositionWorkerService>();
                    services.AddHostedService<EmployeeWorkerService>();
                    //services.AddHostedService<PSGCCityWorkerService>();
                    //services.AddHostedService<PSGCRegionWorkerService>();
                    services.AddHostedService<EmployeeMovementWorkerService>();
                    services.AddHostedService<EmployeeRovingWorkerService>();
                    services.AddHostedService<EmployeeMovementSyncWorkerService>();
                });
    }
}
