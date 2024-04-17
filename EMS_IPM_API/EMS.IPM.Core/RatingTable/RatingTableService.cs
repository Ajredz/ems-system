using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.KPI;
using EMS.IPM.Data.RatingTable;
using EMS.IPM.Data.Reference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using EMS.IPM.Transfer.RatingTable;

namespace EMS.IPM.Core.RatingTable
{
    public interface IRatingTableService
    {
        Task<IActionResult> GetAll(APICredentials credentials);
    }

    public class RatingTableService : Core.Shared.Utilities, IRatingTableService
    {
        private readonly IRatingTableDBAccess _dbAccess;

        public RatingTableService(IPMContext dbContext, IConfiguration iconfiguration,
            IRatingTableDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            List<Data.RatingTable.RatingTable> result = (await _dbAccess.GetAll()).ToList();

            return new OkObjectResult(result.Select(x => new GetAllOutput
            {
                ID = x.ID,
                Code = x.Code,
                Description = x.Description,
                MinScore = x.MinScore,
                MaxScore = x.MaxScore,
            }).ToList());
        }

    }
}