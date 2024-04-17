using EMS.Common.Data.DBContexts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.Common.Core.Shared
{
    public class Utilities : ErrorLog
    {
        public readonly IConfiguration _iconfiguration;
        public readonly SystemAccessContext _dbContext;
        public static int _userID = 0;
        public ResultView _resultView = new ResultView();
        public List<string> ErrorMessages = new List<string>();

        public Utilities(SystemAccessContext dbContext, IConfiguration iconfiguration) : base(
            "Common"
            , iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value)
        {
            _dbContext = dbContext;
            _iconfiguration = iconfiguration;
        }

    }
}