using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utilities.API;

namespace PlantillaToRecruitment.Worker
{
    public class EmployeeMovementWorkerService : BackgroundService
    {
        private readonly ILogger<EmployeeMovementWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _executeSyncURL = "";
        private readonly string _defaultDirectory = "";

        public EmployeeMovementWorkerService(ILogger<EmployeeMovementWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _executeSyncURL =
              string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            
            DoWork<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement>().ConfigureAwait(true); ;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Execute every end of day
                int hourSpan = 24 - DateTime.Now.Hour;
                int numberOfHours = hourSpan;
                if (hourSpan == 24)
                {
                    await DoWork<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement>();
                    numberOfHours = 24;
                }
                await Task.Delay(TimeSpan.FromSeconds(numberOfHours), stoppingToken);
            }
        }

        private async Task DoWork<TModel>()
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _executeSyncURL);

                List<TModel> updatedEmployeeMovements = APIResult;

                if (IsSuccess)
                {
                    if (updatedEmployeeMovements.Count > 0)
                    {
                        _logger.LogInformation("[Plantilla - EmployeeMovement] {count} record(s) for synchronization.", updatedEmployeeMovements.Count);
                        
                        string logMessage =
                            string.Concat("[Plantilla - EmployeeMovement] SUCCESS: ",
                            APIResult.Count, " record(s) synchronized.", " | "
                            , DateTime.Now, Environment.NewLine,
                            "IDs=[", string.Join(",", APIResult.Select(x =>
                            x.GetType().GetProperty("ID").GetValue(x)
                            ).ToArray()), "]");

                        //SharedUtilities.CreateWorkerServiceLog(
                        //    _defaultDirectory,
                        //    "EMS_Plantilla_EmployeeMovement_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                        //    logMessage);
                        _logger.LogInformation(logMessage);

                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[Plantilla - EmployeeMovement] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[Plantilla - EmployeeMovement] ERROR: {error}", ex.Message);
            }
        }
    }
}
