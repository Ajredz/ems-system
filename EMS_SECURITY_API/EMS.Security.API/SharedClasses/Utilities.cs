using Utilities.API;
using EMS_SecurityService.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EMS_SecurityService.SharedClasses
{
    public class Utilities : ErrorLog
    {
        public readonly IConfiguration _iconfiguration;
        public readonly SystemAccessContext _dbContext;
        public static int _userID = 0;
        public ResultView _resultView = new ResultView();
        public List<string> ErrorMessages = new List<string>();

        public Utilities(SystemAccessContext dbContext, IConfiguration iconfiguration) : base(
            "Security"
            , iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value)
        {
            _dbContext = dbContext;
            _iconfiguration = iconfiguration;
        }
    }
}
