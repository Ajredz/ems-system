using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EMS_SecurityService.DBContexts;
using EMS_SecurityServiceModel.Reference;
using Microsoft.Extensions.Configuration;
using EMS_SecurityService.SharedClasses;
using Utilities.API;
using Microsoft.EntityFrameworkCore;

namespace EMS_SecurityService.Controllers
{
    [Route("security/[controller]")]
    [ApiController]
    public class ReferenceController : SharedClasses.Utilities
    {
        public ReferenceController(SystemAccessContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
        }


        [HttpGet]
        [Route("get-reference-by-codes")]
        public async Task<IActionResult> GetReferenceByCodes([FromQuery] int userid, [FromQuery] List<string> codes)
        {
            _userID = userid;
            var SystemPage = await _dbContext.Reference.AsNoTracking().Where(x => codes.Contains(x.Code)).Distinct().ToListAsync();

            if (SystemPage.Count > 0)
                return new OkObjectResult(SystemPage);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
        }

        [HttpGet]
        [Route("getreferencevaluebyrefcode")]
        public async Task<IActionResult> GetReferenceValueByRefCode([FromQuery] int userid, [FromQuery] string refcode)
        {
            _userID = userid;
            var SystemPage = await _dbContext.ReferenceValue.AsNoTracking().Where(x => x.RefCode == refcode).Distinct().ToListAsync();

            if (SystemPage.Count > 0)
                return new OkObjectResult(SystemPage);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
        }

        [HttpGet]
        [Route("getreferencevalueautocomplete")]
        public async Task<IActionResult> GetReferenceValueAutoComplete([FromQuery] int userid, [FromQuery] string refcode, [FromQuery] string term)
        {
            _userID = userid;
            List<ReferenceValue> result = new List<ReferenceValue>();
            List<ReferenceValue> collection = await _dbContext.ReferenceValue.AsNoTracking()
                .Where(x => x.RefCode.Equals(refcode) && x.Description.ToString().ToLower().Contains((term ?? "").ToLower()))
                .Select(x =>
                    new ReferenceValue
                    {
                        Index = x.Description.ToString().ToLower().IndexOf((term ?? "").ToLower()),
                        ID = x.ID,
                        RefCode = x.RefCode,
                        Value = x.Value,
                        Description = x.Description,
                        UserID = x.UserID,
                        CreatedDate = x.CreatedDate
                    }
                ).ToListAsync();
            result = collection.OrderBy(x => x.Index).ThenBy(x => x.Description).ToList();
            return new OkObjectResult(result);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromBody] List<ReferenceValue> ReferenceValues, [FromQuery] int userid)
        {
            _userID = userid;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var refCode = ReferenceValues.FirstOrDefault().RefCode;

                //var duplicates = _dbContext.ReferenceValue.AsNoTracking().Where(x => x.RefCode == refCode).ToList()
                //    .Union(
                //        ReferenceValues
                //    ).GroupBy(x => new { x.Value }).Where(y => y.Count() > 1);

                var oldSet = await _dbContext.ReferenceValue.AsNoTracking().Where(x => x.RefCode == refCode).ToListAsync();

                List<ReferenceValue> ReferenceValueToDelete = new Common_Reference().GetReferenceToDelete(oldSet, ReferenceValues).ToList();
                List<ReferenceValue> ReferenceValueToAdd = new Common_Reference().GetReferenceToAdd(oldSet, ReferenceValues).ToList();
                List<ReferenceValue> ReferenceValueToUpdate = new Common_Reference().GetReferenceToUpdate(oldSet, ReferenceValues).ToList();

                // Execute filtered records to their respective actions.
                _dbContext.ReferenceValue.RemoveRange(ReferenceValueToDelete);
                _dbContext.ReferenceValue.AddRange(ReferenceValueToAdd);
                ReferenceValueToUpdate.Select(x => {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                _resultView.IsSuccess = true;

            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
    }
}
