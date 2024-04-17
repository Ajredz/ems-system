using EMS.Manpower.Data.DBContexts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.Manpower.Core.Shared
{
    public class Utilities : ErrorLog
    {
        public readonly IConfiguration _iconfiguration;
        public readonly ManpowerContext _dbContext;
        public ResultView _resultView = new ResultView();
        public List<string> ErrorMessages = new List<string>();

        public Utilities(ManpowerContext dbContext, IConfiguration iconfiguration) : base(
            "Manpower"
            , iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value)
        {
            _dbContext = dbContext;
            _iconfiguration = iconfiguration;
        }

        public static string _MRFDescriptionDefault = "Workflow for MRF Signatories";
    }
}