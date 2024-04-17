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
    public class PositionLevelWorkerService : BackgroundService
    {
        private readonly ILogger<PositionLevelWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestPositionLevelToManpowerURL = "";
        private readonly string _syncLatestPositionLevelToRecruitmentURL = "";
        private readonly string _getLatestPositionLevelURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public PositionLevelWorkerService(ILogger<PositionLevelWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestPositionLevelToManpowerURL =
              string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("PositionLevel").Value);

            _syncLatestPositionLevelToRecruitmentURL =
              string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("PositionLevel").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("PositionLevel_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestPositionLevelURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").Value,
                       "?unit=", _iconfiguration.GetSection("PositionLevel_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("PositionLevel_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.Manpower.Data.DataDuplication.PositionLevel.PositionLevel>(_syncLatestPositionLevelToManpowerURL, "Manpower").ConfigureAwait(true);
            DoWork<EMS.Recruitment.Data.DataDuplication.PositionLevel.PositionLevel>(_syncLatestPositionLevelToRecruitmentURL, "Recruitment").ConfigureAwait(true);
            #endregion

            _getLatestPositionLevelURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").Value,
                       "?unit=", _iconfiguration.GetSection("PositionLevel_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("PositionLevel_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.Manpower.Data.DataDuplication.PositionLevel.PositionLevel>(_syncLatestPositionLevelToManpowerURL, "Manpower");
                await DoWork<EMS.Recruitment.Data.DataDuplication.PositionLevel.PositionLevel>(_syncLatestPositionLevelToRecruitmentURL, "Recruitment");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestPositionLevelURL);

                List<TModel> updatedPositionLevels = APIResult;

                if (IsSuccess)
                {
                    if (updatedPositionLevels.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - PositionLevel] {count} record(s) for synchronization.", updatedPositionLevels.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedPositionLevels, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - PositionLevel] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_PositionLevel_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - PositionLevel] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - PositionLevel] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - PositionLevel] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - PositionLevel] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
