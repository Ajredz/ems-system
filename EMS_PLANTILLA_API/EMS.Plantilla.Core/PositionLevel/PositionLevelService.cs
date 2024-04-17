using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.PositionLevel;
using EMS.Plantilla.Transfer.PositionLevel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.Core.PositionLevel
{
    public interface IPositionLevelService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetPositionLevelDropDown(APICredentials credentials, int SelectedID);

        Task<IActionResult> GetAll(APICredentials credentials);

        Task<IActionResult> GetByOrgGroupID(APICredentials credentials, GetByPositionLevelIDInput param);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);
    }

    public class PositionLevelService : Core.Shared.Utilities, IPositionLevelService
    {
        private readonly IPositionLevelDBAccess _dbAccess;

        public PositionLevelService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IPositionLevelDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarPositionLevel> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Description = x.Description
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            Data.PositionLevel.PositionLevel result = await _dbAccess.GetByID(input.ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Description = result.Description,
                    CreatedBy = result.CreatedBy
                }

            );
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.Description = param.Description.Trim();

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            else
            {
                if ((await _dbAccess.GetByDescription(param.Description)).Count() > 0)
                {
                    ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                //if (!Regex.IsMatch(param.Description, RegexUtilities.REGEX_CODE))
                //{
                //    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                //}
            }

            if (string.IsNullOrEmpty(param.CompanyID.ToString()))
                ErrorMessages.Add("Company " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.PositionLevel.PositionLevel
                {
                    Description = param.Description,
                    CreatedBy = param.CreatedBy,
                    CompanyID = param.CompanyID,
                    IsActive = true,
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            param.Description = param.Description.Trim();

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            else
            {
                List<Data.PositionLevel.PositionLevel> form = (await _dbAccess.GetByDescription(param.Description)).ToList();

                if (form.Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                //if (!Regex.IsMatch(param.Description, RegexUtilities.REGEX_CODE))
                //{
                //    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                //}
            }

            if (string.IsNullOrEmpty(param.CompanyID.ToString()))
                ErrorMessages.Add("Company " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                Data.PositionLevel.PositionLevel form =
                   await _dbAccess.GetByID(param.ID);

                form.Description = param.Description;
                form.CompanyID = param.CompanyID;
                form.ModifiedBy = credentials.UserID;
                form.ModifiedDate = DateTime.Now;
                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            EMS.Plantilla.Data.PositionLevel.PositionLevel positionLevel = await _dbAccess.GetByID(ID);
            positionLevel.IsActive = false;
            positionLevel.ModifiedBy = credentials.UserID;
            positionLevel.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(positionLevel))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param.Term, param.TopResults))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Description
                })
            );
        }

        public async Task<IActionResult> GetPositionLevelDropDown(APICredentials credentials, int SelectedID)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Description).ToList(), "ID", "Description", null, SelectedID));
        }

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetAll());
        }

        public async Task<IActionResult> GetByOrgGroupID(APICredentials credentials, GetByPositionLevelIDInput param)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown(
                (await _dbAccess.GetByOrgGroupID(param.OrgGroupID))
                .ToList(), "ID", "Description", null, param.SelectedValue));
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));
        }
    }
}