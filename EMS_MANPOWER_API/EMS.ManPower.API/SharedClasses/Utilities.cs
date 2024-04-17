using Utilities.API;
using EMS_ManPowerService.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EMS_ManPowerService.SharedClasses
{
    public class Utilities : ErrorLog
    {
        public readonly IConfiguration _iconfiguration;
        public readonly ManpowerRequisitionContext _dbContext;
        public static int _userId = 0;
        public ResultView _resultView = new ResultView();
        public List<string> ErrorMessages = new List<string>();

        public Utilities(ManpowerRequisitionContext dbContext, IConfiguration iconfiguration) : base(
            "ManpowerRequisition"
            , iconfiguration.GetSection("ErrorLogPath").GetSection("DefaultErrorLogPath").Value
            , iconfiguration.GetSection("ErrorLogPath").GetSection("ErrorLogPathInetpub").Value
            , true
            , iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value
            , _userId)
        {
            _dbContext = dbContext;
            _iconfiguration = iconfiguration;
        }

    }
}
