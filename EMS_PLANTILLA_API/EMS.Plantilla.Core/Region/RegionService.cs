using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.Region;
using EMS.Plantilla.Transfer.Region;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.Core.Region
{
    public interface IRegionService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetDropDown(APICredentials credentials);
    }

    public class RegionService : Core.Shared.Utilities, IRegionService
    {
        private readonly EMS.Plantilla.Data.Region.IRegionDBAccess _dbAccess;

        public RegionService(PlantillaContext dbContext, IConfiguration iconfiguration,
            EMS.Plantilla.Data.Region.IRegionDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarRegion> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                Description = x.Description
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            Data.Region.Region result = await _dbAccess.GetByID(input.ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Code = result.Code,
                    Description = result.Description,
                    CreatedBy = result.CreatedBy
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.Code = param.Code.Trim();
            param.Description = param.Description.Trim();

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Code.Length > 50)
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

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.Region.Region
                {
                    Code = param.Code,
                    Description = param.Description,
                    CreatedBy = param.CreatedBy
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
            param.Code = param.Code.Trim();
            param.Description = param.Description.Trim();

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Code.Length > 50)
                ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            else
            {
                List<Data.Region.Region> form = (await _dbAccess.GetByCode(param.Code)).ToList();

                if (form.Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (ErrorMessages.Count == 0)
            {
                Data.Region.Region form = await _dbAccess.GetByID(param.ID);

                form.Code = param.Code;
                form.Description = param.Description;
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
            await _dbAccess.Delete(ID);
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param.Term, param.TopResults))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "ID", "Code", "Description", null));
        }
    }
}