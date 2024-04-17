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
    public class EmployeeRovingWorkerService : BackgroundService
    {
        private readonly ILogger<EmployeeRovingWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestEmployeeRovingToIPMURL = "";
        private readonly string _getLatestEmployeeRovingURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public EmployeeRovingWorkerService(ILogger<EmployeeRovingWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestEmployeeRovingToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeRoving").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("EmployeeRoving_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestEmployeeRovingURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeRoving").Value,
                       "?unit=", _iconfiguration.GetSection("EmployeeRoving_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("EmployeeRoving_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.IPM.Data.DataDuplication.Employee.EmployeeRoving>(_syncLatestEmployeeRovingToIPMURL, "IPM").ConfigureAwait(true);
            #endregion

            _getLatestEmployeeRovingURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeRoving").Value,
                       "?unit=", _iconfiguration.GetSection("EmployeeRoving_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("EmployeeRoving_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.IPM.Data.DataDuplication.Employee.EmployeeRoving>(_syncLatestEmployeeRovingToIPMURL, "IPM");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestEmployeeRovingURL);

                List<TModel> updatedEmployeeRovings = APIResult;

                if (IsSuccess)
                {
                    if (updatedEmployeeRovings.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - EmployeeRoving] {count} record(s) for synchronization.", updatedEmployeeRovings.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedEmployeeRovings, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - EmployeeRoving] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_EmployeeRoving_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - EmployeeRoving] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - EmployeeRoving] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - EmployeeRoving] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - EmployeeRoving] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
