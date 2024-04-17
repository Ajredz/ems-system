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
    public class PSGCRegionWorkerService : BackgroundService
    {
        private readonly ILogger<PSGCRegionWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestPSGCRegionToIPMURL = "";
        private readonly string _getLatestPSGCRegionURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public PSGCRegionWorkerService(ILogger<PSGCRegionWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestPSGCRegionToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("PSGCRegion").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("PSGCRegion_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestPSGCRegionURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGCRegion").Value,
                       "?unit=", 0 /*Get All Records for syncing*/,
                       "&value=", _iconfiguration.GetSection("PSGCRegion_GetModifiedBy_Value").Value);
            DoWork<EMS.IPM.Data.DataDuplication.PSGCRegion.PSGCRegion>(_syncLatestPSGCRegionToIPMURL, "IPM").ConfigureAwait(true);
            #endregion

            _getLatestPSGCRegionURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGCRegion").Value,
                       "?unit=", _iconfiguration.GetSection("PSGCRegion_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("PSGCRegion_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.IPM.Data.DataDuplication.PSGCRegion.PSGCRegion>(_syncLatestPSGCRegionToIPMURL, "IPM");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestPSGCRegionURL);

                List<TModel> updatedPSGCRegions = APIResult;

                if (IsSuccess)
                {
                    if (updatedPSGCRegions.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - PSGCRegion] {count} record(s) for synchronization.", updatedPSGCRegions.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedPSGCRegions, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - PSGCRegion] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            SharedUtilities.CreateWorkerServiceLog(
                                _defaultDirectory,
                                "EMS_" + ModuleName + "_PSGCRegion_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                                logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - PSGCRegion] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - PSGCRegion] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - PSGCRegion] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - PSGCRegion] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
