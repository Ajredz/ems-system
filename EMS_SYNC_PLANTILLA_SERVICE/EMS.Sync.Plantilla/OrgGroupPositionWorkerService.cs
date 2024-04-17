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
    public class OrgGroupPositionWorkerService : BackgroundService
    {
        private readonly ILogger<OrgGroupPositionWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestOrgGroupPositionToManpowerURL = "";
        private readonly string _syncLatestOrgGroupPositionToRecruitmentURL = "";
        private readonly string _syncLatestOrgGroupPositionToIPMURL = "";
        private readonly string _getLatestOrgGroupPositionURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public OrgGroupPositionWorkerService(ILogger<OrgGroupPositionWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestOrgGroupPositionToManpowerURL =
              string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroupPosition").Value);

            _syncLatestOrgGroupPositionToRecruitmentURL =
              string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("OrgGroupPosition").Value);

            _syncLatestOrgGroupPositionToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroupPosition").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("OrgGroupPosition_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestOrgGroupPositionURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroupPosition").Value,
                       "?unit=", _iconfiguration.GetSection("OrgGroupPosition_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("OrgGroupPosition_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.Manpower.Data.DataDuplication.OrgGroup.OrgGroupPosition>(_syncLatestOrgGroupPositionToManpowerURL, "Manpower").ConfigureAwait(true);
            //DoWork<EMS.Recruitment.Data.DataDuplication.OrgGroup.OrgGroupPosition>(_syncLatestOrgGroupPositionToRecruitmentURL, "Recruitment").ConfigureAwait(true);
            DoWork<EMS.IPM.Data.DataDuplication.OrgGroup.OrgGroupPosition>(_syncLatestOrgGroupPositionToIPMURL, "IPM").ConfigureAwait(true);
            #endregion

            _getLatestOrgGroupPositionURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroupPosition").Value,
                       "?unit=", _iconfiguration.GetSection("OrgGroupPosition_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("OrgGroupPosition_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.Manpower.Data.DataDuplication.OrgGroup.OrgGroupPosition>(_syncLatestOrgGroupPositionToManpowerURL, "Manpower");
                //await DoWork<EMS.Recruitment.Data.DataDuplication.OrgGroup.OrgGroupPosition>(_syncLatestOrgGroupPositionToRecruitmentURL, "Recruitment");
                await DoWork<EMS.IPM.Data.DataDuplication.OrgGroup.OrgGroupPosition>(_syncLatestOrgGroupPositionToIPMURL, "IPM");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestOrgGroupPositionURL);

                List<TModel> updatedOrgGroupPositions = APIResult;

                if (IsSuccess)
                {
                    if (updatedOrgGroupPositions.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - OrgGroupPosition] {count} record(s) for synchronization.", updatedOrgGroupPositions.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedOrgGroupPositions, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - OrgGroupPosition] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_OrgGroupPosition_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - OrgGroupPosition] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - OrgGroupPosition] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - OrgGroupPosition] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - OrgGroupPosition] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
