using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.KPI;
using EMS.IPM.Data.KRAGroup;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer.KPI;
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

namespace EMS.IPM.Core.KPI
{
    public interface IKPIService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetAll(APICredentials credentials);

        Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetAllDetails(APICredentials credentials);

        Task<IActionResult> Upload(APICredentials credentials, List<UploadFileEntity> param);

        Task<IActionResult> GetByRefCodes(APICredentials credentials, List<string> RefCodes);
    }

    public class KPIService : Core.Shared.Utilities, IKPIService
    {
        private readonly IKPIDBAccess _dbAccess;
        private readonly IKRAGroupDBAccess _kRAGroupDBAccess;

        public KPIService(IPMContext dbContext, IConfiguration iconfiguration,
            IKPIDBAccess dbAccess, IKRAGroupDBAccess kRAGroupDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _kRAGroupDBAccess = kRAGroupDBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarKPI> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                KRAType = x.KRAType,
                KRAGroup = x.KRAGroup,
                KRASubGroup = x.KRASubGroup,                
                Name = x.Name,
                OldKPICode = x.OldKPICode,
                KPIType = x.KPIType,
                SourceType = x.SourceType
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)    
        {
            Data.KPI.KPI result = await _dbAccess.GetByID(input.ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Code = result.Code,
                    Description = result.Description,
                    TargetGuidelines = result.Guidelines,
                    Name = result.Name,
                    KRAGroup = result.KRAGroup,
                    KRASubGroup = result.KRASubGroup ?? 0,
                    OldKPICode = result.OldKPICode,
                    Type = result.Type,
                    SourceType = result.SourceType,
                    ModifiedBy = result.ModifiedBy
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            int ctr = 1;

            if ((await _dbAccess.GetByOldKPICode(param.OldKPICode)).Count() > 0)
            {
                ErrorMessages.Add(param.OldKPICode + " " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            var NewKPICode = (await _dbAccess.GetNewKPICode(ctr)).ToList();
            if (NewKPICode != null)
            {
                int newKPIcode = Convert.ToInt32(NewKPICode.First().NewKPICode) - 1;
                param.Code = Convert.ToString(newKPIcode);
            }
            param.OldKPICode = (param.OldKPICode ?? "").Trim();
            param.Name = (param.Name ?? "").Trim();
            param.Description = (param.Description ?? "").Trim();
            
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

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.KPI.KPI
                {
                    Code = param.Code,
                    OldKPICode = param.OldKPICode,
                    Name = param.Name,
                    Description = param.Description,
                    Guidelines = param.TargetGuidelines,
                    KRAGroup = param.KRAGroup,
                    KRASubGroup = param.KRASubGroup,
                    Type = param.Type,
                    SourceType = param.SourceType,
                    ModifiedBy = param.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
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
            var currentKPI = await _dbAccess.GetByID(param.ID);

            if (currentKPI.OldKPICode != param.OldKPICode)
            {
                var checkOldKPICode = await _dbAccess.GetByOldKPICode(param.OldKPICode);

                if (checkOldKPICode.Count() > 0)
                {
                    ErrorMessages.Add(param.OldKPICode + " " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (string.IsNullOrEmpty(param.TargetGuidelines))
                ErrorMessages.Add("Target Guidelines " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                Data.KPI.KPI form = await _dbAccess.GetByID(param.ID);

                //form.Code = param.Code;
                form.OldKPICode = param.OldKPICode;
                form.Name = param.Name;
                form.Description = param.Description;
                form.Guidelines = param.TargetGuidelines;
                form.KRAGroup = param.KRAGroup;
                form.KRASubGroup = param.KRASubGroup;
                form.Type = param.Type;
                form.SourceType = param.SourceType;
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
            if ((await _dbAccess.GetKPIIfUsed(ID)).ToList().Count() > 0)
                ErrorMessages.Add("KPI " + MessageUtilities.SUFF_ERRMSG_REC_INUSE);

            if (ErrorMessages.Count == 0)
            {

                Data.KPI.KPI kpi = await _dbAccess.GetByID(ID);

                kpi.ModifiedBy = credentials.UserID;
                kpi.ModifiedDate = DateTime.Now;
                kpi.IsActive = false;

                await _dbAccess.Put(kpi);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll())
                                                                .Where(x => x.IsActive == true).ToList(), "ID", "Code", "Description")
                );
        }

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAll()).ToList());
        }

        public async Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "Code", "Code", "Description", param.ID)
                );
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Name)
                })
            );
        }

        public async Task<IActionResult> GetAllDetails(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAllDetails()).ToList()
                .Select(x => new GetAllKPIDetailsOutput { 
                    ID = x.ID,
                    KPICode = x.KPICode,
                    KPIName = x.KPIName,
                    KPIDescription = x.KPIDescription,
                    KRAGroup = x.KRAGroup,
                    KRASubGroup = x.KRASubGroup,
                    OldKPICode = x.OldKPICode
                }).ToList());
        }

        public async Task<IActionResult> GetByRefCodes(APICredentials credentials, List<string> RefCodes)
        {
            return new OkObjectResult(
                (await _dbAccess.GetRefCodes(RefCodes))
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

        public async Task<IActionResult> Upload(APICredentials credentials, List<UploadFileEntity> param)
        {
            //Checking if file is empty
            if (param.Count == 0)
            {
                ErrorMessages.Add("File is empty.");
            }

            /*Checking of required and invalid fields*/
            foreach (UploadFileEntity obj in param)
            {
                /*KRA Group*/
                if (string.IsNullOrEmpty(obj.KRAGroup))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KRA Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }

                /*Old KPI Code*/
                if (string.IsNullOrEmpty(obj.OldKPICode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Old KPI Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }

                /*KPI Name*/
                if (string.IsNullOrEmpty(obj.KPIName))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }


                /*Description*/
                if (string.IsNullOrEmpty(obj.Description))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }

                /*Target Guidelines*/
                if (string.IsNullOrEmpty(obj.TargetGuidelines))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Target Guidelines ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }

                /*KPI Type*/
                if (string.IsNullOrEmpty(obj.KPIType))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }

                if (string.IsNullOrEmpty(obj.SourceType))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Source Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }


            /*Checking if Old KPI Code was existing on database*/
            if (ErrorMessages.Count == 0)
            {
                List<string> oldkpiList = (await _dbAccess.GetOldKPICodes(param.Select(x => x.OldKPICode).ToList())).Select(x => x.OldKPICode).Distinct().ToList();
                List<string> kraGroupList = (await _kRAGroupDBAccess.GetAll()).Select(x => x.Name).Distinct().ToList();

                foreach (UploadFileEntity obj in param)
                {
                    /*KRA Group*/
                    if (!string.IsNullOrEmpty(obj.KRAGroup))
                    {
                        if (!kraGroupList.Contains(obj.KRAGroup))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KRA Group " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.KRAGroup));
                        }
                    }

                    /*Old KPI Code*/
                    if (!string.IsNullOrEmpty(obj.OldKPICode))
                    {
                        if (oldkpiList.Contains(obj.OldKPICode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Old KPI Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS, " Value: " + obj.OldKPICode));
                        }
                    }
                }
            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                List<Data.KRAGroup.KRAGroup> kraGroupList = (await _kRAGroupDBAccess.GetAll()).ToList();
                
                var NewKPICode = (await _dbAccess.GetNewKPICode(param.Count)).ToList();
                int ctr = Convert.ToInt32(NewKPICode.First().NewKPICode) - param.Count;

                var uploadKPIList = param.Join(kraGroupList,
                                    upload => upload.KRAGroup,
                                    kragroup => kragroup.Name,
                                    (upload, kragroup) => new { upload, kragroup }
                                    )
                                    .Select(x => new Data.KPI.KPI
                                    {
                                        Code = (ctr++).ToString("000"),
                                        OldKPICode = x.upload.OldKPICode,
                                        Name = x.upload.KPIName,
                                        Description = x.upload.Description,
                                        Guidelines = x.upload.TargetGuidelines,
                                        KRAGroup = x.kragroup.ID,
                                        Type = x.upload.KPIType,
                                        SourceType = x.upload.SourceType,
                                        ModifiedBy = credentials.UserID,
                                        ModifiedDate = DateTime.Now,
                                        IsActive = true
                                    }).ToList();

                await _dbAccess.Upload(uploadKPIList);

                _resultView.IsSuccess = true;
            }

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Upload was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_FILE_UPLOAD);
            else
            {
                if (ErrorMessages.Count > 52)
                {
                    string ErrorMessage = string.Join("<br>", ErrorMessages.Take(52).ToArray());
                    ErrorMessage += string.Concat("<br><br> ", ErrorMessages.Count - 52, " other errors found.");
                    return new BadRequestObjectResult(ErrorMessage);
                }
                else
                {
                    return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
                }
            }
        }
    }
}