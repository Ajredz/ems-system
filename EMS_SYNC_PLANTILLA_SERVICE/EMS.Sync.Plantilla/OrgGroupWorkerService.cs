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
    public class OrgGroupWorkerService : BackgroundService
    {
        private readonly ILogger<OrgGroupWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestOrgGroupToManpowerURL = "";
        private readonly string _syncLatestOrgGroupToRecruitmentURL = "";
        private readonly string _syncLatestOrgGroupToIPMURL = "";
        private readonly string _syncLatestOrgGroupToH2PayURL = "";
        private readonly string _getLatestOrgGroupURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public OrgGroupWorkerService(ILogger<OrgGroupWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestOrgGroupToManpowerURL =
              string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroup").Value);

            _syncLatestOrgGroupToRecruitmentURL =
              string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("OrgGroup").Value);

            _syncLatestOrgGroupToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("OrgGroup_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestOrgGroupURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").Value,
                       "?unit=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.Manpower.Data.DataDuplication.OrgGroup.OrgGroup>(_syncLatestOrgGroupToManpowerURL, "Manpower").ConfigureAwait(true);
            DoWork<EMS.Recruitment.Data.DataDuplication.OrgGroup.OrgGroup>(_syncLatestOrgGroupToRecruitmentURL, "Recruitment").ConfigureAwait(true);
            DoWork<EMS.IPM.Data.DataDuplication.OrgGroup.OrgGroup>(_syncLatestOrgGroupToIPMURL, "IPM").ConfigureAwait(true);

            _syncLatestOrgGroupToH2PayURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("H2Pay_OrgGroup_Sync").Value,
                       "?unit=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_Value_OnStartUp").Value);

            //EMSH2PayIntegration(_syncLatestOrgGroupToH2PayURL, "H2Pay").ConfigureAwait(true);

            #endregion

            _getLatestOrgGroupURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").Value,
                       "?unit=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_Value").Value);

            _syncLatestOrgGroupToH2PayURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("H2Pay_OrgGroup_Sync").Value,
                       "?unit=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("OrgGroup_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.Manpower.Data.DataDuplication.OrgGroup.OrgGroup>(_syncLatestOrgGroupToManpowerURL, "Manpower");
                await DoWork<EMS.Recruitment.Data.DataDuplication.OrgGroup.OrgGroup>(_syncLatestOrgGroupToRecruitmentURL, "Recruitment");
                await DoWork<EMS.IPM.Data.DataDuplication.OrgGroup.OrgGroup>(_syncLatestOrgGroupToIPMURL, "IPM");
                //await EMSH2PayIntegration(_syncLatestOrgGroupToH2PayURL, "H2Pay");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestOrgGroupURL);

                List<TModel> updatedOrgGroups = APIResult;

                if (IsSuccess)
                {
                    if (updatedOrgGroups.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - OrgGroup] {count} record(s) for synchronization.", updatedOrgGroups.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedOrgGroups, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - OrgGroup] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_OrgGroup_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - OrgGroup] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - OrgGroup] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - OrgGroup] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - OrgGroup] ERROR: {error}", ex.Message);
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
                                            string.Concat("[" + ModuleName + " - OrgGroup] SUCCESS: ",
                                            Message, " record(s) synchronized.");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_OrgGroup_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);  
                        }
                    }

                }
                else
                    _logger.LogInformation("[" + ModuleName + " - OrgGroup] ERROR: {message}", Message);
              
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - OrgGroup] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
