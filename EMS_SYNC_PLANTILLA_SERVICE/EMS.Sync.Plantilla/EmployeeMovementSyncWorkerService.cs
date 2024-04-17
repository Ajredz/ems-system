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
    public class EmployeeMovementSyncWorkerService : BackgroundService
    {
        private readonly ILogger<EmployeeMovementSyncWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestEmployeeMovementToIPMURL = "";
        private readonly string _getLatestEmployeeMovementURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public EmployeeMovementSyncWorkerService(ILogger<EmployeeMovementSyncWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestEmployeeMovementToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeMovement").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("EmployeeMovement_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestEmployeeMovementURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovementSync").Value,
                       "?unit=", _iconfiguration.GetSection("EmployeeMovement_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("EmployeeMovement_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.IPM.Data.DataDuplication.EmployeeMovement.EmployeeMovement>(_syncLatestEmployeeMovementToIPMURL, "IPM").ConfigureAwait(true);
            #endregion

            _getLatestEmployeeMovementURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovementSync").Value,
                       "?unit=", _iconfiguration.GetSection("EmployeeMovement_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("EmployeeMovement_GetModifiedBy_Value").Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.IPM.Data.DataDuplication.EmployeeMovement.EmployeeMovement>(_syncLatestEmployeeMovementToIPMURL, "IPM");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestEmployeeMovementURL);

                List<TModel> updatedEmployeeMovements = APIResult;

                if (IsSuccess)
                {
                    if (updatedEmployeeMovements.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - EmployeeMovement] {count} record(s) for synchronization.", updatedEmployeeMovements.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedEmployeeMovements, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - EmployeeMovement] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_EmployeeMovement_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - EmployeeMovement] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - EmployeeMovement] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - EmployeeMovement] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - EmployeeMovement] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
