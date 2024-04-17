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
    public class IntegrateSystemUserWorkerService : BackgroundService
    {
        private readonly ILogger<SystemUserWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly string _IntegrateWithPortalGlobalURL = "";
        private readonly string _defaultDirectory = "";
        private readonly int _IntervalInSeconds = 0;

        public IntegrateSystemUserWorkerService(ILogger<SystemUserWorkerService> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _defaultDirectory = _iconfiguration.GetSection("SynchronizationLogs").GetSection("DefaultPath").Value;
            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("IntegrateWithPortalGlobal_IntervalInSeconds").Value);

            #region Sync all on startup
            _IntegrateWithPortalGlobalURL =
               string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("IntegrateWithPortalGlobal").Value);

            DoWork().ConfigureAwait(true);
            #endregion

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork();
                await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
            }
        }

        private async Task DoWork()
        {

            try
            {
                var (IsSuccess, Message) =
                               await SharedUtilities.PostFromAPI(new object(), _IntegrateWithPortalGlobalURL);

                if (IsSuccess)
                {
                    string logMessage =
                        string.Concat("[Integrate - SystemUser] SUCCESS: ", Message);

                    //SharedUtilities.CreateWorkerServiceLog(
                    //    _defaultDirectory,
                    //    "EMS_Integrate_SystemUser_" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                    //    logMessage);
                    _logger.LogInformation(logMessage);
                }
                else
                {

                    if (!string.IsNullOrEmpty(Message))
                        _logger.LogInformation("[Integrate - SystemUser] ERROR: {error}", Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[Integrate - SystemUser] ERROR: {error}", ex.Message);
                //throw;
            }
        }


    }
}
