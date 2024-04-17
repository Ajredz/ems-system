using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.Reference;
using EMS.Manpower.Transfer.Reference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Utilities.API.ReferenceMaintenance;

namespace EMS.Manpower.Core.Reference
{
    public interface IReferenceService
    {
        Task<IActionResult> GetByCodes(APICredentials credentials, List<string> Codes);

        Task<IActionResult> GetByRefCodes(APICredentials credentials, List<string> RefCodes);

        Task<IActionResult> UpdateSet(APICredentials credentials, List<Utilities.API.ReferenceMaintenance.ReferenceValue> param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetDropDown(APICredentials credentials);

        Task<IActionResult> GetByRefCodeValue(APICredentials credentials, GetByRefCodeValueInput param);

        Task<IActionResult> GetMaintainable(APICredentials credentials);
    }

    public class ReferenceService : Core.Shared.Utilities, IReferenceService
    {
        private readonly IReferenceDBAccess _dbAccess;

        public ReferenceService(ManpowerContext dbContext, IConfiguration iconfiguration,
            IReferenceDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetByCodes(APICredentials credentials, List<string> Codes)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByCodes(Codes))
                .Select(x =>
                new Utilities.API.ReferenceMaintenance.Reference
                {
                    ID = x.ID,
                    Code = x.Code,
                    Description = x.Description,
                    UserID = x.UserID
                }));
        }

        public async Task<IActionResult> GetByRefCodes(APICredentials credentials, List<string> RefCodes)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByRefCodes(RefCodes))
                .Select(x =>
                new Utilities.API.ReferenceMaintenance.ReferenceValue
                {
                    ID = x.ID,
                    RefCode = x.RefCode,
                    Value = x.Value,
                    Description = x.Description,
                    UserID = x.UserID
                }));
        }

        public async Task<IActionResult> UpdateSet(APICredentials credentials, List<Utilities.API.ReferenceMaintenance.ReferenceValue> param)
        {
            string refCode = param.FirstOrDefault().RefCode;

            List<Utilities.API.ReferenceMaintenance.ReferenceValue> oldSet =
                (await _dbAccess.GetByRefCodes(new List<string> { refCode })).Select(x =>
                new Utilities.API.ReferenceMaintenance.ReferenceValue
                {
                    ID = x.ID,
                    RefCode = x.RefCode,
                    Value = x.Value,
                    Description = x.Description,
                    UserID = x.UserID
                }).ToList();

            List<Utilities.API.ReferenceMaintenance.ReferenceValue> ReferenceValueToDelete = new Common_Reference().GetReferenceToDelete(oldSet, param).ToList();
            List<Utilities.API.ReferenceMaintenance.ReferenceValue> ReferenceValueToAdd = new Common_Reference().GetReferenceToAdd(oldSet, param).ToList();
            List<Utilities.API.ReferenceMaintenance.ReferenceValue> ReferenceValueToUpdate = new Common_Reference().GetReferenceToUpdate(oldSet, param).ToList();

            _resultView.IsSuccess = await _dbAccess.UpdateSet(
                ReferenceValueToDelete.Select(x =>
                    new Data.Reference.ReferenceValue
                    {
                        ID = x.ID,
                        RefCode = x.RefCode,
                        Value = x.Value,
                        Description = x.Description,
                        UserID = x.UserID
                    }).ToList(),
                ReferenceValueToAdd.Select(x =>
                    new Data.Reference.ReferenceValue
                    {
                        ID = x.ID,
                        RefCode = x.RefCode,
                        Value = x.Value,
                        Description = x.Description,
                        UserID = x.UserID
                    }).ToList(),
                ReferenceValueToUpdate.Select(x =>
                    new Data.Reference.ReferenceValue
                    {
                        ID = x.ID,
                        RefCode = x.RefCode,
                        Value = x.Value,
                        Description = x.Description,
                        UserID = x.UserID
                    }).ToList());

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
               (await _dbAccess.GetAutoComplete(param.RefCode, param.Term, param.TopResults))
               .Select(x => new GetIDByAutoCompleteOutput
               {
                   ID = x.ID,
                   Description = string.Concat(x.Value, " - ", x.Description)
               })
           );
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials)
        {
            return new OkObjectResult(
                SharedUtilities.GetDropdown((await _dbAccess.GetMaintainedReference()).ToList(), "ID", "Description")
                );
        }

        public async Task<IActionResult> GetByRefCodeValue(APICredentials credentials, GetByRefCodeValueInput param)
        {
            return new OkObjectResult(await _dbAccess.GetByRefCodeValue(param.RefCode, param.Value));
        }

        public async Task<IActionResult> GetMaintainable(APICredentials credentials)
        {
            return new OkObjectResult(
                    (await _dbAccess.GetByIsMaintainable(true)).Select(x => new GetIsMaintainableOutput
                    {
                        ID = x.ID,
                        Code = x.Code,
                        Description = x.Description
                    }).ToList()
                );
        }
    }
}