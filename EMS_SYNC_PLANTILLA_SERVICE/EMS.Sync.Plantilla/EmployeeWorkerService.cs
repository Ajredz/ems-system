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
    public class EmployeeWorkerService : BackgroundService
    {
        private readonly ILogger<EmployeeWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestEmployeeToIPMURL = "";
        private readonly string _getLatestEmployeeURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public EmployeeWorkerService(ILogger<EmployeeWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestEmployeeToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("Employee").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("Employee_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestEmployeeURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").Value,
                       "?unit=", _iconfiguration.GetSection("Employee_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("Employee_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.IPM.Data.DataDuplication.Employee.Employee>(_syncLatestEmployeeToIPMURL, "IPM").ConfigureAwait(true);
            #endregion

            _getLatestEmployeeURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").Value,
                       "?unit=", _iconfiguration.GetSection("Employee_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("Employee_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.IPM.Data.DataDuplication.Employee.Employee>(_syncLatestEmployeeToIPMURL, "IPM");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestEmployeeURL);

                List<TModel> updatedEmployees = APIResult;

                if (IsSuccess)
                {
                    if (updatedEmployees.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - Employee] {count} record(s) for synchronization.", updatedEmployees.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedEmployees, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - Employee] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_Employee_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - Employee] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - Employee] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - Employee] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - Employee] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
