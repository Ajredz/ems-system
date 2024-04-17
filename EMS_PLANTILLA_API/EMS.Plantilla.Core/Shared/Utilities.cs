using EMS.Plantilla.Data.DBContexts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.Plantilla.Core.Shared
{
    public class Utilities : ErrorLog
    {
        public readonly IConfiguration _iconfiguration;
        public readonly PlantillaContext _dbContext;
        public ResultView _resultView = new ResultView();
        public List<string> ErrorMessages = new List<string>();

        public Utilities(PlantillaContext dbContext, IConfiguration iconfiguration) : base(
            "Plantilla"
            , iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value)
        {
            _dbContext = dbContext;
            _iconfiguration = iconfiguration;
        }

        public enum ORGGROUPTYPE
        {
            TOP,
            BCH,
            DIV,
            DEPT,
            SECT,
            UNIT,
            REG,
            AREA,
            ZONE
        }

        public enum REFERENCEVALUECODE
        {
            ORGGROUPTYPE,
            COMPANY_TAG,
            REGION_TAG,
            ORG_BCH_TAGS,
            ORG_DEPT_TAGS
        }
    }
}