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
    public class SystemUserWorkerService : BackgroundService
    {
        private readonly ILogger<SystemUserWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _getLatestSystemUserURL = "";
        private readonly string _syncLatestSystemUserToRecruitmentURL = "";
        private readonly string _syncLatestSystemUserToH2PayURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public SystemUserWorkerService(ILogger<SystemUserWorkerService> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestSystemUserToRecruitmentURL =
              string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("SystemUser").Value);

            _defaultDirectory = _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value;
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("SystemUser_IntervalInSeconds").Value);

            _getLatestSystemUserURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").Value,
                       "?unit=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_Value").Value);

            #region Sync all on startup
            _getLatestSystemUserURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").Value,
                       "?unit=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.Recruitment.Data.DataDuplication.SystemUser.SystemUser>(_syncLatestSystemUserToRecruitmentURL, "Recruitment").ConfigureAwait(true);

            _syncLatestSystemUserToH2PayURL =
              string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("SecurityService_API_URL").GetSection("H2Pay_SystemUser_Sync").Value,
                      "?unit=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                      "&value=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_Value_OnStartUp").Value);

            //EMSH2PayIntegration(_syncLatestSystemUserToH2PayURL, "H2Pay").ConfigureAwait(true);
            #endregion

            _getLatestSystemUserURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").Value,
                       "?unit=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_Value").Value);

            _syncLatestSystemUserToH2PayURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("H2Pay_SystemUser_Sync").Value,
                       "?unit=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("SystemUser_GetModifiedBy_Value").Value);


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.Recruitment.Data.DataDuplication.SystemUser.SystemUser>(_syncLatestSystemUserToRecruitmentURL, "Recruitment");
                //await EMSH2PayIntegration(_syncLatestSystemUserToH2PayURL, "H2Pay");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {

            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestSystemUserURL);

                List<TModel> updatedSystemUsers = APIResult;

                if (IsSuccess)
                {
                    if (updatedSystemUsers.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - SystemUser] {count} record(s) for synchronization.", updatedSystemUsers.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedSystemUsers, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - SystemUser] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_SystemUser_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - SystemUser] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - SystemUser] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - SystemUser] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - SystemUser] ERROR: {error}", ex.Message);
                //throw;
            }
        }

        private async Task EMSH2PayIntegration(string URL, string ModuleName)
        {
            try
            {
                var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(new object { }, URL);

                if (PostIsSuccess)
                {
                    if (Int32.TryParse(Message, out int result))
                    {
                        if (result > 0)
                        {
                            string logMessage =
                                            string.Concat("[" + ModuleName + " - SystemUser] SUCCESS: ",
                                            Message, " record(s) synchronized.");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_SystemUser_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);
                        }
                    }

                }
                else
                    _logger.LogInformation("[" + ModuleName + " - SystemUser] ERROR: {message}", Message);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - SystemUser] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
