using EMS.Recruitment.Data.ApplicantTagging;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.ApplicantTagging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.ApplicantTagging
{
    public interface IApplicantTaggingService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

    }

    public class ApplicantTaggingService : Shared.Utilities, IApplicantTaggingService
    {
        private readonly IApplicantTaggingDBAccess _dbAccess;

        public ApplicantTaggingService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            IApplicantTaggingDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarApplicantTagging> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.ApplicantTagging.GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                ApplicantName = x.ApplicantName,
                ApplicationSource = x.ApplicationSource,
                PositionRemarks = x.PositionRemarks,
                ReferredBy = x.ReferredBy
            }).ToList());
        }

    }
}
