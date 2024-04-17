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
    public class PSGCCityWorkerService : BackgroundService
    {
        private readonly ILogger<PSGCCityWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _syncLatestPSGCCityToIPMURL = "";
        private readonly string _getLatestPSGCCityURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public PSGCCityWorkerService(ILogger<PSGCCityWorkerService> logger, IConfiguration iconfiguration, IHostEnvironment env)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _syncLatestPSGCCityToIPMURL =
              string.Concat(_iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("IPMService_API_URL").GetSection("PSGCCity").Value);

            _defaultDirectory = string.Concat(env.ContentRootPath, _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value);
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("PSGCCity_IntervalInSeconds").Value);

            #region Sync all on startup
            _getLatestPSGCCityURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGCCity").Value,
                       "?unit=", 0 /*Get All Records for syncing*/,
                       "&value=", _iconfiguration.GetSection("PSGCCity_GetModifiedBy_Value").Value);
            DoWork<EMS.IPM.Data.DataDuplication.PSGCCity.PSGCCity>(_syncLatestPSGCCityToIPMURL, "IPM").ConfigureAwait(true);
            #endregion

            _getLatestPSGCCityURL =
               string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGCCity").Value,
                       "?unit=", _iconfiguration.GetSection("PSGCCity_GetModifiedBy_UnitOfTime").Value,
                       "&value=", _iconfiguration.GetSection("PSGCCity_GetModifiedBy_Value").Value);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork<EMS.IPM.Data.DataDuplication.PSGCCity.PSGCCity>(_syncLatestPSGCCityToIPMURL, "IPM");
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork<TModel>(string URL, string ModuleName)
        {
            try
            {
                var (APIResult, IsSuccess, ErrorMessage) =
                               await SharedUtilities.GetFromAPI(new List<TModel>(), _getLatestPSGCCityURL);

                List<TModel> updatedPSGCCitys = APIResult;

                if (IsSuccess)
                {
                    if (updatedPSGCCitys.Count > 0)
                    {
                        _logger.LogInformation("[" + ModuleName + " - PSGCCity] {count} record(s) for synchronization.", updatedPSGCCitys.Count);
                        var (PostIsSuccess, Message) = await SharedUtilities.PostFromAPI(updatedPSGCCitys, URL);

                        if (PostIsSuccess)
                        {
                            string logMessage =
                                string.Concat("[" + ModuleName + " - PSGCCity] SUCCESS: ",
                                APIResult.Count, " record(s) synchronized.", " | "
                                , DateTime.Now, Environment.NewLine,
                                "IDs=[", string.Join(",", APIResult.Select(x =>
                                x.GetType().GetProperty("ID").GetValue(x)
                                ).ToArray()), "]");

                            SharedUtilities.CreateWorkerServiceLog(
                                _defaultDirectory,
                                "EMS_" + ModuleName + "_PSGCCity_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                                logMessage);
                            _logger.LogInformation(logMessage);

                        }
                        else
                            _logger.LogInformation("[" + ModuleName + " - PSGCCity] ERROR: {message}", Message);
                    }
                    //else
                    //    _logger.LogInformation("[" + ModuleName + " - PSGCCity] No record(s) for synchronization.");
                }
                else
                {

                    if (!string.IsNullOrEmpty(ErrorMessage))
                        _logger.LogInformation("[" + ModuleName + " - PSGCCity] ERROR: {error}", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[" + ModuleName + " - PSGCCity] ERROR: {error}", ex.Message);
                //throw;
            }
        }
    }
}
