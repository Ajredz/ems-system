using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Reference;
using EMS.Workflow.Transfer.Reference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;
using Utilities.API.ReferenceMaintenance;

namespace EMS.Workflow.Core.Reference
{
    public interface IReferenceService
    {
        Task<IActionResult> GetByCodes(APICredentials credentials, List<string> Codes);

        Task<IActionResult> GetByRefCodes(APICredentials credentials, List<string> RefCodes);

        Task<IActionResult> UpdateSet(APICredentials credentials, List<Utilities.API.ReferenceMaintenance.ReferenceValue> param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetDropDown(APICredentials credentials);

        Task<IActionResult> GetMaintainable(APICredentials credentials);

        Task<IActionResult> GetByRefCodeAndValuePrefix(APICredentials credentials, GetByRefCodeAndValuePrefixInput param);

        Task<IActionResult> GetByRefCodeValue(APICredentials credentials, GetByRefCodeValueInput param);

        Task<IActionResult> GetReferenceValueList(APICredentials credentials, string RefCode);

        Task<IActionResult> AddReferenceValue(APICredentials credentials, Utilities.API.ReferenceMaintenance.ReferenceValue param);

        Task<IActionResult> UpdateReferenceValue(APICredentials credentials, Utilities.API.ReferenceMaintenance.ReferenceValue param);

        Task<IActionResult> GetReferenceValueByID(APICredentials credentials, int ID);
    }

    public class ReferenceService : Core.Shared.Utilities, IReferenceService
    {
        private readonly IReferenceDBAccess _dbAccess;

        public ReferenceService(WorkflowContext dbContext, IConfiguration iconfiguration,
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
                .OrderBy(y => y.Description)
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

        public async Task<IActionResult> GetByRefCodeAndValuePrefix(APICredentials credentials, GetByRefCodeAndValuePrefixInput param)
        {
            return new OkObjectResult(
                    (await _dbAccess.GetByRefCodeAndValuePrefix(param)).Select(x => 
                    new Utilities.API.ReferenceMaintenance.ReferenceValue
                    {
                        ID = x.ID,
                        RefCode = x.RefCode,
                        Value = x.Value,
                        Description = x.Description,
                        UserID = x.UserID
                    }).ToList()
                );
        }

        public async Task<IActionResult> GetByRefCodeValue(APICredentials credentials, GetByRefCodeValueInput param)
        {
            return new OkObjectResult(await _dbAccess.GetByRefCodeValue(param.RefCode, param.Value));
        }

        public async Task<IActionResult> GetReferenceValueList(APICredentials credentials, string RefCode)
        {
            return new OkObjectResult(
                (await _dbAccess.GetReferenceValueList(RefCode))
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new GetRefValueListOutput
                {
                    ID = x.ID,
                    Value = x.Value,
                    Description = x.Description
                }).ToList()
            );
        }

        public async Task<IActionResult> AddReferenceValue(APICredentials credentials, Utilities.API.ReferenceMaintenance.ReferenceValue param)
        {

            if (string.IsNullOrEmpty(param.RefCode))
                ErrorMessages.Add(string.Concat("Reference Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if (param.RefCode.Length > 20)
                {
                    ErrorMessages.Add(string.Concat("Reference Code", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                }

                if (!Regex.IsMatch(param.RefCode, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(string.Concat("Reference Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                }
            }

            if (string.IsNullOrEmpty(param.Value))
                ErrorMessages.Add(string.Concat("Value ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if (param.Value.Length > 20)
                {
                    ErrorMessages.Add(string.Concat("Value", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add(string.Concat("Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if (param.Description.Length > 255)
                {
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                }
            }

            if ((await _dbAccess.GetByCodeValue(param.RefCode, param.Value)).Count() > 0)
            {
                ErrorMessages.Add("Reference Value " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.AddReferenceValue(new Data.Reference.ReferenceValue
                {
                    RefCode = param.RefCode,
                    Value = param.Value,
                    Description = param.Description,
                    UserID = credentials.UserID
                });

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> UpdateReferenceValue(APICredentials credentials, Utilities.API.ReferenceMaintenance.ReferenceValue param)
        {

            if (string.IsNullOrEmpty(param.RefCode))
                ErrorMessages.Add(string.Concat("Reference Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if (param.RefCode.Length > 20)
                {
                    ErrorMessages.Add(string.Concat("Reference Code", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                }

                if (!Regex.IsMatch(param.RefCode, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(string.Concat("Reference Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                }
            }

            if (string.IsNullOrEmpty(param.Value))
                ErrorMessages.Add(string.Concat("Value ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if (param.Value.Length > 20)
                {
                    ErrorMessages.Add(string.Concat("Value", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add(string.Concat("Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if (param.Description.Length > 255)
                {
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                }
            }

            if ((await _dbAccess.GetByCodeValue(param.RefCode, param.Value)).Where(x => x.ID != param.ID).Count() > 0)
            {
                ErrorMessages.Add("Reference Value " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            if (ErrorMessages.Count == 0)
            {

                Data.Reference.ReferenceValue form = await _dbAccess.GetReferenceValueByID(param.ID);

                form.RefCode = param.RefCode;
                form.Value = param.Value;
                form.Description = param.Description;

                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetReferenceValueByID(APICredentials credentials, int ID)
        {
            Data.Reference.ReferenceValue result = await _dbAccess.GetReferenceValueByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Data.Reference.ReferenceValue
                {
                    ID = result.ID,
                    RefCode = result.RefCode,
                    Value = result.Value,
                    Description = result.Description
                });
        }

    }
}