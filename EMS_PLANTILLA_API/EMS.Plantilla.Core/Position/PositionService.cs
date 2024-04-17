using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.OrgGroup;
using EMS.Plantilla.Data.Position;
using EMS.Plantilla.Data.Reference;
using EMS.Plantilla.Transfer;
using EMS.Plantilla.Transfer.Position;
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

namespace EMS.Plantilla.Core.Position
{
    public interface IPositionService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetDropDown(APICredentials credentials);

        Task<IActionResult> GetDropDownByPositionLevel(APICredentials credentials, int PositionLevelID);

        Task<IActionResult> GetAll(APICredentials credentials);

        Task<IActionResult> GetDropDownDetailedByPositionLevel(APICredentials credentials, int PositionLevelID);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetDropdownWithCountByOrgGroup(APICredentials credentials, GetDropdownByOrgGroupInput param);

        Task<IActionResult> GetCodeDropDown(APICredentials credentials);

        Task<IActionResult> GetPositionWithLevelByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);
    }

    public class PositionService : Core.Shared.Utilities, IPositionService
    {
        private readonly IPositionDBAccess _dbAccess;
        private readonly IOrgGroupDBAccess _orgGroupdbAccess;
        private readonly IReferenceDBAccess _referenceDBAccess;

        public PositionService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IPositionDBAccess dbAccess, IOrgGroupDBAccess orgGroupdbAccess, IReferenceDBAccess referenceDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _orgGroupdbAccess = orgGroupdbAccess;
            _referenceDBAccess = referenceDBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarPosition> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                PositionLevelDescription = x.PositionLevelDescription,
                Code = x.Code,
                Title = x.Title,
                ParentPositionDescription = x.ParentPositionDescription
                
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            Data.Position.Position result = await _dbAccess.GetByID(input.ID);
           var jobClassDescription = (await _referenceDBAccess
                .GetByRefCodeValue(Enums.ReferenceCodes.JOB_CLASS.ToString(), result.JobClassCode));

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Code = result.Code,
                    Title = result.Title,
                    PositionLevelID = result.PositionLevelID,
                    ParentPositionID = result.ParentPositionID,
                    JobClassCode = result.JobClassCode,
                    JobClassDescription = jobClassDescription != null ?
                        jobClassDescription.Count() > 0 ?
                        jobClassDescription.First().Description : "" : "",
                    CreatedBy = result.CreatedBy,
                    OnlinePosition = result.OnlinePosition,
                    OnlineLocation = result.OnlineLocation,
                    OnlineJobDescription = result.OnlineJobDescription,
                    OnlineJobQualification = result.OnlineJobQualification,
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Code = param.Code.Trim();
                if (param.Code.Length > 50)
                    ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                else
                {
                    if ((await _dbAccess.GetByCode(param.Code)).Count() > 0)
                    {
                        ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                    }

                    if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                    }
                }
            }
            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Title = param.Title.Trim();
                if (param.Title.Length > 255)
                    ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (string.IsNullOrEmpty(param.JobClassCode))
                ErrorMessages.Add("Job Class " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.JobClassCode = param.JobClassCode.Trim();
                if (param.JobClassCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Job Class", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (param.PositionLevelID <= 0)
                ErrorMessages.Add("Position Level " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.Position.Position
                {
                    PositionLevelID = param.PositionLevelID,
                    Code = param.Code,
                    Title = param.Title,
                    //ParentPositionID = param.ParentPositionID,
                    JobClassCode = param.JobClassCode,
                    IsActive = true,
                    CreatedBy = param.CreatedBy,
                    OnlinePosition = param.OnlinePosition,
                    OnlineLocation=param.OnlineLocation,
                    OnlineJobDescription=param.OnlineJobDescription,
                    OnlineJobQualification = param.OnlineJobQualification
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

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Code = param.Code.Trim();
                if (param.Code.Length > 50)
                    ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                else
                {
                    List<Data.Position.Position> form = (await _dbAccess.GetByCode(param.Code)).ToList();

                    if (form.Where(x => x.ID != param.ID).Count() > 0)
                    {
                        ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                    }

                    if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                    }
                }
            }

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Title = param.Title.Trim();
                if (param.Title.Length > 255)
                    ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (string.IsNullOrEmpty(param.JobClassCode))
                ErrorMessages.Add("Job Class " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.JobClassCode = param.JobClassCode.Trim();
                if (param.JobClassCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Job Class", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (param.PositionLevelID <= 0)
                ErrorMessages.Add("Position Level " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                Data.Position.Position form =
                    await _dbAccess.GetByID(param.ID);

                form.PositionLevelID = param.PositionLevelID;
                //form.ParentPositionID = param.ParentPositionID;
                form.Code = param.Code;
                form.Title = param.Title;
                form.JobClassCode = param.JobClassCode;
                form.OnlinePosition = param.OnlinePosition;
                form.OnlineLocation = param.OnlineLocation;
                form.OnlineJobDescription = param.OnlineJobDescription;
                form.OnlineJobQualification = param.OnlineJobQualification;
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
            EMS.Plantilla.Data.Position.Position position = await _dbAccess.GetByID(ID);
            position.IsActive = false;
            position.ModifiedBy = credentials.UserID;
            position.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(position))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials)
        {
            //return new OkObjectResult(
            //    (await _dbAccess.GetAll())
            //    .Select(x =>
            //    new GetDropDownOutput
            //    {
            //        ID = x.ID,
            //        Description = string.Concat(x.Code, " - ", x.Title)
            //    }
            //    ));

            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "ID", "Code", "Title", null));
        }

        public async Task<IActionResult> GetDropDownByPositionLevel(APICredentials credentials, int PositionLevelID)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetByPositionLevel(PositionLevelID)).OrderBy(x => x.Code).ToList(), "ID", "Code", "Title", null));
        }

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetAll());
        }

        public async Task<IActionResult> GetDropDownDetailedByPositionLevel(APICredentials credentials, int PositionLevelID)
        {
            return new OkObjectResult(
                  (await _dbAccess.GetByPositionLevel(PositionLevelID)).Select(x => new SelectListItem
                  {
                      Value = JsonConvert.SerializeObject(new { x.ID, x.Code, x.Title, x.PositionLevelID}),
                      Text = string.Concat(x.Code, " - ", x.Title)
                      
                  }).ToList()
                );
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));   
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Title)
                })
            );
        }

        public async Task<IActionResult> GetDropdownWithCountByOrgGroup(APICredentials credentials, GetDropdownByOrgGroupInput param)
        {
            List<TableVarOrgGroupPosition> orgGroupPositions = (await _orgGroupdbAccess.GetOrgGroupPositionByOrgID(param.OrgGroupID)).ToList();
            var positions = (await _dbAccess.GetByIDs(
                    orgGroupPositions.Select(x => x.PositionID).ToList())).ToList();
            return new OkObjectResult(
                SharedUtilities.GetDropdown(
               positions.Join(
                   orgGroupPositions,
                   x => new { x.ID },
                   y => new { ID = y.PositionID },
                   (x, y) => new { x, y}
                   ).Select(x => new Data.Position.Position {
                        ID = x.x.ID,
                        Code = x.x.Code,
                        Title = string.Concat(x.x.Title,
                        " (P:", x.y.PlannedCount, " | ",
                        "A:", (x.y.ActiveCount + x.y.ActiveProbCount), " | ",
                        "I:", (x.y.InactiveCount + x.y.OutgoingCount), " | ",
                        "V:", ((x.y.ActiveCount + x.y.ActiveProbCount) /*+ x.y.InactiveCount*/ - x.y.PlannedCount), ")")
                   }).ToList(),
                    "ID",
                    "Code",
                    "Title", param.SelectedValue)
                );
        }

        public async Task<IActionResult> GetCodeDropDown(APICredentials credentials)
        {
            //return new OkObjectResult(
            //    (await _dbAccess.GetAll())
            //    .Select(x =>
            //    new GetDropDownOutput
            //    {
            //        ID = x.ID,
            //        Description = string.Concat(x.Code, " - ", x.Title)
            //    }
            //    ));

            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "Code", "Code", "Title", null));
        }

        public async Task<IActionResult> GetPositionWithLevelByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetPositionWithLevelByAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Description
                })
            );
        }

    }
}