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
    public class PositionWorkerService : BackgroundService
    {
        private readonly ILogger<PositionWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestPositionToManpowerURL = "";
        private readonly string _syncLatestPositionToRecruitmentURL = "";
        private readonly string _syncLatestPositionToIPMURL = "";
        private readonly string _syncLatestPositionToH2PayURL = "";
        private readonly string _getLatestPositionURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public PositionWorkerService(ILogger<PositionWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestPositionToManpowerURL =
              string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").Value);

            _syncLatestPositionToRecruitmentURL =
              string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Position").Value);

            _syncLatestPositionToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("Position").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("Position_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestPositionURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Position").Value,
                       "?unit=", _iconfiguration.GetSection("Position_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                       "&value=", _iconfiguration.GetSection("Position_GetModifiedBy_Value_OnStartUp").Value);
            DoWork<EMS.Manpower.Data.DataDuplication.Position.Position>(_syncLatestPositionToManpowerURL, "Manpower").ConfigureAwait(true);
            DoWork<EMS.Recruitment.Data.DataDuplication.Position.Position>(_syncLatestPositionToRecruitmentURL, "Recruitment").ConfigureAwait(true);
            DoWork<EMS.IPM.Data.DataDuplication.Position.Position>(_syncLatestPositionToIPMURL, "IPM").ConfigureAwait(true);

            _syncLatestPositionToH2PayURL =
              string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("H2Pay_Position_Sync").Value,
                      "?unit=", _iconfiguration.GetSection("Position_GetModifiedBy_UnitOfTime_OnStartUp").Value,
                      "&value=", _iconfiguration.GetSection("Position_GetModifiedBy_Value_OnStartUp").Value);

            //EMSH2PayIntegration(_syncLatestPositionToH2PayURL, "H2Pay").ConfigureAwait(true);
            #endregion

            _getLatestPositionURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Position").Value,
                       "?unit=", _iconfiguration.GetSection("Position_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("Position_GetModifiedBy_Value").Value);

            _syncLatestPositionToH2PayURL =
              string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("H2Pay_Position_Sync").Value,
                      "?unit=", _iconfiguration.GetSection("Position_GetModifiedBy_UnitOfTime").Value,
                      "&value=", _iconfiguration.GetSection("Position_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.Manpower.Data.DataDuplication.Position.Position>(_syncLatestPositionToManpowerURL, "Manpower");
                await DoWork<EMS.Recruitment.Data.DataDuplication.Position.Position>(_syncLatestPositionToRecruitmentURL, "Recruitment");
                await DoWork<EMS.IPM.Data.DataDuplication.Position.Position>(_syncLatestPositionToIPMURL, "IPM");
                //await EMSH2PayIntegration(_syncLatestPositionToH2PayURL, "H2Pay");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestPositionURL);

                List<TModel> updatedPositions = APIResult;

                if (IsSuccess)
                {
                    if (updatedPositions.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - Position] {count} record(s) for synchronization.", updatedPositions.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedPositions, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - Position] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_Position_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - Position] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - Position] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - Position] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - Position] ERROR: {error}", ex.Message);
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
                                            string.Concat("[" + ModuleName + " - Position] SUCCESS: ",
                                            Message, " record(s) synchronized.");

                            //SharedUtilities.CreateWorkerServiceLog(
                            //    _defaultDirectory,
                            //    "EMS_" + ModuleName + "_Position_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                            //    logMessage);
                            _logger.LogInformation(logMessage);
                        }
                    }

                }
                else
                    _logger.LogInformation("[" + ModuleName + " - Position] ERROR: {message}", Message);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - Position] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
