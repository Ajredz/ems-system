using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utilities.API;

namespace EMS.Sync.Security.Worker
{
    public class SystemRoleWorkerService : BackgroundService
    {
        private readonly ILogger<SystemRoleWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _getLatestSystemRoleURL = "";
        private readonly string _syncLatestSystemRoleToManpowerURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public SystemRoleWorkerService(ILogger<SystemRoleWorkerService> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestSystemRoleToManpowerURL =
              string.Concat(_iconfiguration.GetSection("ManpowerService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("ManpowerService_API_URL").GetSection("SystemRole").Value);

            _defaultDirectory = _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value;
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("SystemRole_IntervalInSeconds").Value);

            _getLatestSystemRoleURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").Value,
                       "?unit=", _iconfiguration.GetSection("SystemRole_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("SystemRole_GetModifiedBy_Value").Value);

            #region Sync all on startup
            _getLatestSystemRoleURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").Value,
                       "?unit=", _iconfiguration.GetSection("SystemRole_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("SystemRole_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.Manpower.Data.DataDuplication.SystemRole.SystemRole>(_syncLatestSystemRoleToManpowerURL, "Manpower").ConfigureAwait(true); 
            #endregion

            _getLatestSystemRoleURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").Value,
                       "?unit=", _iconfiguration.GetSection("SystemRole_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("SystemRole_GetModifiedBy_Value").Value);


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.Manpower.Data.DataDuplication.SystemRole.SystemRole>(_syncLatestSystemRoleToManpowerURL, "Manpower");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
                
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestSystemRoleURL);

                List<TModel> updatedSystemRoles = APIResult;

                if (IsSuccess)
                {
                    if (updatedSystemRoles.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - SystemRole] {count} record(s) for synchronization.", updatedSystemRoles.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedSystemRoles, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - SystemRole] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_SystemRole_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - SystemRole] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - SystemRole] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - SystemRole] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - SystemRole] ERROR: {error}", ex.Message);
                //throw;
            }
        }


    }
}
