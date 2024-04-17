using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.OrgGroup;
using EMS.Plantilla.Data.Position;
using EMS.Plantilla.Data.Reference;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EMS.Plantilla.Core.OrgGroup
{
    public interface IOrgGroupService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetChart(APICredentials credentials, GetChartInput param);

        Task<IActionResult> GetChartPosition(APICredentials credentials, GetChartInput param);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetDropDownByOrgType(APICredentials credentials, string OrgType);

        Task<IActionResult> GetChildrenOrgDropDown(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> GetOrgGroupPosition(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> GetDropDownDetailed(APICredentials credentials);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> GetLastModifiedOrgGroupPosition(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFileEntity> param);

        Task<IActionResult> UploadEdit(APICredentials credentials, List<UploadFileEntity> param);

        Task<IActionResult> GetExportCountByOrgTypeData(APICredentials credentials, GetPlantillaCountInput input);

        Task<IActionResult> GetByOrgGroupAndPosition(APICredentials credentials, GetByOrgGroupIDAndPositionIDInput param);

        Task<IActionResult> GetExportList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetOrgGroupHierarchy(APICredentials credentials, int ID);
        Task<IActionResult> GetOrgGroupHierarchyBomd(APICredentials credentials, int ID);

        Task<IActionResult> GetOrgGroupEmployeeList(APICredentials credentials, GetEmployeeListInput input);

        Task<IActionResult> GetIDByOrgTypeAutoComplete(APICredentials credentials, GetByOrgTypeAutoCompleteInput param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetOrgGroupHierarchyLevelsDropDown(APICredentials credentials);

        Task<IActionResult> GetOrgGroupDescendants(APICredentials credentials, int OrgGroupID);
        Task<IActionResult> GetOrgGroupDescendantsList(APICredentials credentials, List<int> OrgGroupIDs);
        Task<IActionResult> GetOrgGroupDescendantsBomd(APICredentials credentials, int OrgGroupID);

        Task<IActionResult> GetPositionWithDescription(APICredentials credentials, int OrgGroupID);

        Task<IActionResult> UpdatePlantillaCount(APICredentials credentials, List<PlantillaCountUpdateForm> param);

        Task<IActionResult> GetOrgGroupNPRF(APICredentials credentials, int OrgGroupID);

        Task<IActionResult> AddNPRF(APICredentials credentials, List<OrgGroupNPRFForm> param);

        Task<IActionResult> GetByIDs(APICredentials credentials, List<int> IDs);

        Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetOrgGroupRollupPositionDropdown(APICredentials credentials, int OrgGroupID);

        Task<IActionResult> GetRegionByOrgGroupID(APICredentials credentials, int OrgGroupID);

        Task<IActionResult> GetByCodes(APICredentials credentials, string CodesDelimited);

        Task<IActionResult> GetPositionUpwardAutoComplete(APICredentials credentials, GetPositionOrgGroupUpwardAutoCompleteInput param);

        Task<IActionResult> GetOrgGroupUpwardAutoComplete(APICredentials credentials, GetPositionOrgGroupUpwardAutoCompleteInput param);
        Task<IActionResult> GetBranchInfoList(APICredentials credentials, GetBranchInfoListInput input);
        Task<IActionResult> GetOrgGroupHistoryList(APICredentials credentials, OrgGroupHistoryListInput param);
        Task<IActionResult> GetOrgGroupHistoryByDate(APICredentials credentials, OrgGroupHistoryByDateInput param);
        Task<IActionResult> AddOrgGroupHistory(APICredentials credentials,string TDate,bool IsLatest, List<AddOrgGroupHistoryInput> param);
        Task<IActionResult> GetAllOrgGroupHistory(APICredentials credentials);
        Task<IActionResult> GetEmployeeEmailByOrgId(int OrgGroupID);
        Task<IActionResult> GetOrgGroupFormatByID(List<int> ID);
        Task<IActionResult> GetOrgGroupParent(GetOrgGroupParentInput param);
        Task<IActionResult> GetOrgGroupSOMD(List<int> IDs);
    }

    public class OrgGroupService : Core.Shared.Utilities, IOrgGroupService
    {
        private readonly IOrgGroupDBAccess _dbAccess;
        private readonly IReferenceDBAccess _referenceDBAccess;
        private readonly IPositionDBAccess _positionDBAccess;

        public OrgGroupService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IOrgGroupDBAccess dbAccess, IReferenceDBAccess referenceDBAccess, IPositionDBAccess positionDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _referenceDBAccess = referenceDBAccess;
            _positionDBAccess = positionDBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarOrgGroup> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                Description = x.Description,
                OrgTypeDescription = x.OrgTypeDescription,
                ParentOrgDescription = x.ParentOrgDescription,
                IsBranchActive = x.IsBranchActive,
                ServiceBayCount = x.ServiceBayCount,

            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            Data.OrgGroup.OrgGroup result = await _dbAccess.GetByID(input.ID);
            //IEnumerable<Data.OrgGroup.OrgGroup> _childrenList = await _dbAccess.GetByParentOrgID(input.ID);
            //IEnumerable<Data.OrgGroup.TableVarOrgGroupPosition> _positionList = await _dbAccess.GetOrgGroupPositionByOrgID(input.ID);
            IEnumerable<Data.OrgGroup.OrgGroupTag> _orgTagList = (await _dbAccess.GetTagsByOrgGroupID(input.ID));
            List<OrgGroupTagForm> orgGroupTagWithDesc = new List<OrgGroupTagForm>();
            //IEnumerable<Data.OrgGroup.OrgGroupNPRF> _nprfList = await _dbAccess.GetOrgGroupNPRF(input.ID);

            foreach (var item in _orgTagList)
            {
                string Description = _referenceDBAccess.GetRefCodeAndValue(string.Concat("ORG_BRN_TAGS"), item.TagRefCode).Result?.Description;
                if (!string.IsNullOrEmpty(Description))
                    orgGroupTagWithDesc.Add(new OrgGroupTagForm
                    {
                        ID = item.ID,
                        Code = item.TagRefCode,
                        Value = item.TagValue,
                        Description = Description
                    });
            }

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Code = result.Code,
                    Description = result.Description,
                    Category = result.Category,
                    Email = result.Email,
                    Number = result.Number,
                    OrgType = result.OrgType,
                    Psgc = result.Psgc,
                    Address = result.Address,
                    BranchSize = result.BranchSize,
                    ParkingSize = result.Parkingsize,
                    Sign = result.Sign,
                    Page = result.Page,
                    IsBranchActive = result.IsBranchActive,
                    ServiceBayCount = result.ServiceBayCount,
                    ParentOrgID = result.ParentOrgID,
                    CSODAM = result.CSODAM,
                    HRBP = result.HRBP,
                    RRT = result.RRT,
                    //ChildrenOrgIDList = _childrenList.Select(x => x.ID).ToList(),
                    //OrgGroupPositionList = _positionList.Select(x => new Transfer.OrgGroup.OrgGroupPositionForm
                    //{
                    //    PositionLevelID = x.PositionLevelID,
                    //    PositionID = x.PositionID,
                    //    PlannedCount = x.PlannedCount,
                    //    ActiveCount = x.ActiveCount,
                    //    InactiveCount = x.InactiveCount,
                    //    IsHead = x.IsHead
                    //}).ToList(),
                    OrgGroupTagList = orgGroupTagWithDesc.OrderBy(x => x.Description).ToList(),
                    //OrgGroupNPRFList = _nprfList.Select(x => new Transfer.OrgGroup.OrgGroupNPRF
                    //{ 
                    //    NPRFNumber = x.NPRFNumber,
                    //    ApprovedDate = x.ApprovedDate
                    //}).ToList(),
                    CreatedBy = result.CreatedBy
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.Code = param.Code.Trim();
            param.Description = param.Description.Trim();

            int CompanyOrgTypeCount = (await _dbAccess.GetAll()).Where(x => x.OrgType.Equals(ORGGROUPTYPE.TOP)).Count();

            if (CompanyOrgTypeCount > 0)
            {
                if (param.OrgType.Equals(ORGGROUPTYPE.TOP))
                {
                    ErrorMessages.Add(MessageUtilities.ERRMSG_TOP_LEVEL_EXIST);
                }
            }

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

            if (param.OrgType != null)
            {
                if (string.IsNullOrEmpty(param.OrgType))
                    ErrorMessages.Add(string.Concat("Organization Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else

                    if (param.OrgGroupPositionList != null)
                {
                    foreach (var item in param.OrgGroupPositionList)
                    {
                        if ((await _dbAccess.GetHierarchy(param.ParentOrgID, item.PositionID)).Count() > 0) // , param.ParentOrgID
                        {
                            //ErrorMessages.Add("Position already exists within the Hierarchy."); //Validation if position already exist in hierarchy
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (!string.IsNullOrEmpty(param.Address))
            {
                param.Address = param.Address.Trim();
                if (param.Description.Length > 1000)
                    ErrorMessages.Add(string.Concat("Address", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
            }

            if (ErrorMessages.Count == 0)
            {
                var GetTDate = (await _dbAccess.GetAll()).OrderByDescending(x => x.TDate).Select(y => y.TDate).FirstOrDefault();
                await _dbAccess.Post(new Data.OrgGroup.OrgGroup
                {
                    Code = param.Code,
                    Description = param.Description,
                    OrgType = param.OrgType,
                    Address = param.Address,
                    IsBranchActive = param.IsBranchActive,
                    ServiceBayCount = param.ServiceBayCount,
                    ParentOrgID = param.ParentOrgID ?? 0,
                    IsActive = true,
                    CreatedBy = param.CreatedBy,
                    IsLatest = true,
                    TDate = GetTDate,
                    CSODAM = param.CSODAM ?? 0,
                    HRBP = param.HRBP ?? 0,
                    RRT = param.RRT ?? 0
                }
                , new Data.OrgGroup.OrgGroupHistory
                {
                    Code = param.Code,
                    Description = param.Description,
                    OrgType = param.OrgType,
                    Address = param.Address,
                    IsBranchActive = param.IsBranchActive,
                    ServiceBayCount = param.ServiceBayCount,
                    ParentOrgID = param.ParentOrgID ?? 0,
                    IsActive = true,
                    CreatedBy = param.CreatedBy,
                    IsLatest = true,
                    TDate = GetTDate,
                    CSODAM = param.CSODAM ?? 0,
                    HRBP = param.HRBP ?? 0,
                    RRT = param.RRT ?? 0
                }
                , param.ChildrenOrgIDList?.ToList()
                , param.OrgGroupPositionList?.Select(x => new Data.OrgGroup.OrgGroupPosition
                {
                    PositionID = x.PositionID,
                    PlannedCount = x.PlannedCount,
                    ActiveCount = x.ActiveCount,
                    InactiveCount = x.InactiveCount,
                    IsHead = x.IsHead,
                    CreatedBy = param.CreatedBy,
                    IsActive = true,
                    ReportingPositionID = x.ReportingPositionID
                }).ToList()
                , param.OrgGroupTagList?.Select(x => new Data.OrgGroup.OrgGroupTag
                {
                    TagRefCode = x.Code,
                    TagValue = x.Value
                }).ToList()
                );

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
                List<Data.OrgGroup.OrgGroup> form = (await _dbAccess.GetByCode(param.Code)).ToList();
                int CompanyOrgTypeCount = (await _dbAccess.GetAll()).Where(x => x.OrgType.Equals(ORGGROUPTYPE.TOP)).Count();

                if (form.Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                }

                if (form.Where(x => !x.OrgType.Equals(ORGGROUPTYPE.TOP)).Count() > 0)
                {
                    if (CompanyOrgTypeCount > 0 && param.OrgType.Equals(ORGGROUPTYPE.TOP))
                    {
                        ErrorMessages.Add(MessageUtilities.ERRMSG_TOP_LEVEL_EXIST);
                    }
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (!string.IsNullOrEmpty(param.Address))
            {
                param.Address = param.Address.Trim();
                if (param.Description.Length > 1000)
                    ErrorMessages.Add(string.Concat("Address", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
            }

            if (ErrorMessages.Count == 0)
            {
                List<int> OldChildID = (await _dbAccess.GetByParentOrgID(param.ID)).Select(x => x.ID).ToList();

                List<int> OrgChildToUpdate = GetOrgGroupChildrenID(param.ChildrenOrgIDList, OldChildID).ToList();
                List<int> OrgChildToRemove = GetOrgGroupChildrenID(OldChildID, param.ChildrenOrgIDList).ToList();

                List<Data.OrgGroup.OrgGroupPosition> OldOrgPosition = (await _dbAccess.GetOrgGroupPosition(param.ID)).Where(x => x.IsActive).ToList();
                List<Data.OrgGroup.OrgGroupTag> OldOrgTag = (await _dbAccess.GetTagsByOrgGroupID(param.ID)).ToList();

                List<Data.OrgGroup.OrgGroupPosition> OrgPositionToAdd = GetOrgGroupPositionToAdd(OldOrgPosition,
                    param.OrgGroupPositionList == null ? new List<Data.OrgGroup.OrgGroupPosition>() :
                    param.OrgGroupPositionList.Select(x => new Data.OrgGroup.OrgGroupPosition
                    {
                        OrgGroupID = param.ID,
                        PositionID = x.PositionID,
                        PlannedCount = x.PlannedCount,
                        ActiveCount = x.ActiveCount,
                        InactiveCount = x.InactiveCount,
                        IsHead = x.IsHead,
                        CreatedBy = credentials.UserID,
                        IsActive = true,
                        ReportingPositionID = x.ReportingPositionID
                    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupPosition> OrgPositionToUpdate = GetOrgGroupPositionToUpdate(OldOrgPosition,
                    param.OrgGroupPositionList == null ? new List<Data.OrgGroup.OrgGroupPosition>() :
                    param.OrgGroupPositionList.Select(x => new Data.OrgGroup.OrgGroupPosition
                    {
                        OrgGroupID = param.ID,
                        PositionID = x.PositionID,
                        PlannedCount = x.PlannedCount,
                        ActiveCount = x.ActiveCount,
                        InactiveCount = x.InactiveCount,
                        IsHead = x.IsHead,
                        IsActive = true,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now,
                        ReportingPositionID = x.ReportingPositionID
                    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupPosition> OrgPositionToDelete = GetOrgGroupPositionToDelete(OldOrgPosition,
                    param.OrgGroupPositionList == null ? new List<Data.OrgGroup.OrgGroupPosition>() :
                    param.OrgGroupPositionList.Select(x => new Data.OrgGroup.OrgGroupPosition
                    {
                        OrgGroupID = param.ID,
                        PositionID = x.PositionID,
                        IsActive = false,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now,
                        ReportingPositionID = x.ReportingPositionID
                    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupTag> OrgTagToAdd = GetOrgGroupTagToAdd(OldOrgTag,
                 param.OrgGroupTagList == null ? new List<Data.OrgGroup.OrgGroupTag>() :
                 param.OrgGroupTagList.Select(x => new Data.OrgGroup.OrgGroupTag
                 {
                     OrgGroupID = param.ID,
                     TagRefCode = x.Code,
                     TagValue = x.Value
                 }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupTag> OrgTagToUpdate = GetOrgGroupTagToUpdate(OldOrgTag,
                    param.OrgGroupTagList == null ? new List<Data.OrgGroup.OrgGroupTag>() :
                    param.OrgGroupTagList.Select(x => new Data.OrgGroup.OrgGroupTag
                    {
                        OrgGroupID = param.ID,
                        TagRefCode = x.Code,
                        TagValue = x.Value
                    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupTag> OrgTagToDelete = GetOrgGroupTagToDelete(OldOrgTag,
                    param.OrgGroupTagList == null ? new List<Data.OrgGroup.OrgGroupTag>() :
                    param.OrgGroupTagList.Select(x => new Data.OrgGroup.OrgGroupTag
                    {
                        OrgGroupID = param.ID,
                        TagRefCode = x.Code,
                        TagValue = x.Value
                    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupNPRF> OrgNPRF = param.OrgGroupNPRFList == null ? new List<Data.OrgGroup.OrgGroupNPRF>() :
                    param.OrgGroupNPRFList.Select(x => new Data.OrgGroup.OrgGroupNPRF
                    {
                        OrgGroupID = param.ID,
                        NPRFNumber = x.NPRFNumber,
                        ApprovedDate = Convert.ToDateTime(x.ApprovedDate),
                        CreatedBy = credentials.UserID,
                        SourceFile = x.SourceFile,
                        ServerFile = x.ServerFile
                    }).ToList();

                Data.OrgGroup.OrgGroup OrgGroupData = await _dbAccess.GetByID(param.ID);
                OrgGroupData.Code = param.Code;
                OrgGroupData.Description = param.Description;
                OrgGroupData.CSODAM = param.CSODAM ?? 0;
                OrgGroupData.HRBP = param.HRBP ?? 0;
                OrgGroupData.RRT = param.RRT ?? 0;
                OrgGroupData.Category = param.Category;
                OrgGroupData.Email = param.Email;
                OrgGroupData.Number = param.Number;
                OrgGroupData.OrgType = param.OrgType;
                OrgGroupData.Psgc = param.Psgc;
                OrgGroupData.Address = param.Address;
                OrgGroupData.BranchSize = param.BranchSize;
                OrgGroupData.Parkingsize = param.ParkingSize;
                OrgGroupData.Sign = param.Sign;
                OrgGroupData.Page = param.Page;
                OrgGroupData.IsBranchActive = param.IsBranchActive;
                OrgGroupData.ServiceBayCount = param.ServiceBayCount;
                OrgGroupData.ParentOrgID = param.ParentOrgID ?? 0;
                OrgGroupData.ModifiedBy = credentials.UserID;
                OrgGroupData.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(
                     OrgGroupData
                    , OrgChildToUpdate
                    , OrgChildToRemove
                    , OrgPositionToAdd
                    , OrgPositionToUpdate
                    , OrgPositionToDelete
                    , OrgTagToAdd
                    , OrgTagToUpdate
                    , OrgTagToDelete
                    , OrgNPRF
                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            EMS.Plantilla.Data.OrgGroup.OrgGroup orgGroup = await _dbAccess.GetByID(ID);
            orgGroup.IsActive = false;
            orgGroup.ModifiedBy = credentials.UserID;
            orgGroup.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(orgGroup))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> GetChart(APICredentials credentials, GetChartInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetChart(param))
                .Select(x =>
                    new GetChartOutput
                    {
                        ID = x.ID,
                        Code = x.Code,
                        Description = x.Description,
                        OrgType = x.OrgType,
                        ParentOrgID = (
                            x.OrgType == "REG" && x.HRBP == param.OrgGroupID ? x.HRBP :
                            x.OrgType == "BRN" && (((param.AdminAccess.OrgGroupDescendantsDelimited.Split()).Where(y => y.Contains(x.CSODAM.ToString()))).Count() > 0) ? x.CSODAM : x.ParentOrgID
                        )
                    })
                );
        }

        public async Task<IActionResult> GetChartPosition(APICredentials credentials, GetChartInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetChartPosition(param))
                .Select(x =>
                    new GetChartPositionOutput
                    {
                        ID = x.ID,
                        PositionCode = x.PositionCode,
                        PositionTitle = x.PositionTitle,
                        PlannedCount = x.PlannedCount,
                        ActiveCount = x.ActiveCount,
                        InactiveCount = x.InactiveCount,
                        IsHead = x.IsHead,
                    })
                );
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "ID", "Code", "Description", param.ID)
                );
        }

        public async Task<IActionResult> GetDropDownByOrgType(APICredentials credentials, string OrgType)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetByOrgType(OrgType)).OrderBy(x => x.Code).ToList(), "ID", "Code", "Description", OrgType)
                );
        }

        public async Task<IActionResult> GetChildrenOrgDropDown(APICredentials credentials, GetByIDInput input)
        {
            return new OkObjectResult(
                SharedUtilities.GetDropdown((await _dbAccess.GetByParentOrgID(input.ID)).OrderBy(x => x.Code).ToList(), "ID", "Code", "Description", input.ID)
            );
        }

        public async Task<IActionResult> GetOrgGroupPosition(APICredentials credentials, GetByIDInput input)
        {
            return new OkObjectResult((await _dbAccess.GetOrgGroupPositionByOrgID(input.ID)).Select(x =>
            new OrgGroupPositionForm
            {
                PositionID = x.PositionID,
                PositionLevelID = x.PositionLevelID,
                PlannedCount = x.PlannedCount,
                ActiveCount = x.ActiveCount,
                ActiveProbCount = x.ActiveProbCount,
                OutgoingCount = x.OutgoingCount,
                TotalActiveCount = x.TotalActiveCount,
                InactiveCount = x.InactiveCount,
                ReportingPositionID = x.ReportingPositionID,
                PositionDescription = x.PositionDescription,
                ReportingPositionDescription = x.ReportingPositionDescription,
                IsHead = x.IsHead
            }));
        }

        private IEnumerable<int> GetOrgGroupChildrenID(List<int> left, List<int> right)
        {
            return (left ?? new List<int>()).ToList().Except(right ?? new List<int>());
        }

        public async Task<IActionResult> GetDropDownDetailed(APICredentials credentials)
        {
            return new OkObjectResult(
                 (await _dbAccess.GetAll()).Select(x => new SelectListItem
                 {
                     Value = JsonConvert.SerializeObject(
                         new
                         {
                             x.ID,
                             x.Code,
                             x.Description,
                             x.OrgType,
                             x.ParentOrgID
                         }),
                     Text = string.Concat(x.Code, " - ", x.Description)

                 }).ToList()
               );
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));
        }

        public async Task<IActionResult> GetLastModifiedOrgGroupPosition(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModifiedOrgGroupPosition(From, To));
        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFileEntity> param)
        {

            /*Checking of required and invalid fields*/
            foreach (UploadFileEntity obj in param)
            {

                /*Parent Org Group*/
                if (!string.IsNullOrEmpty(obj.ParentOrgCode))
                {
                    if (obj.ParentOrgCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                    else
                    {
                        if (!Regex.IsMatch(obj.ParentOrgCode, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }
                    }
                }

                /*Org Group Code*/
                if (string.IsNullOrEmpty(obj.OrgGroupCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.OrgGroupCode.Length > 50)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                }
                else
                {
                    if (!Regex.IsMatch(obj.OrgGroupCode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                }

                /*Org Group Description*/
                if (string.IsNullOrEmpty(obj.OrgGroupDescription))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.OrgGroupDescription.Length > 255)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                }

                /*Org Type*/
                if (string.IsNullOrEmpty(obj.OrgType))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    if (obj.OrgType != ORGGROUPTYPE.TOP.ToString() && string.IsNullOrEmpty(obj.ParentOrgCode))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                }

                /*Address*/
                if (!string.IsNullOrEmpty(obj.Address))
                {
                    obj.Address = obj.Address.Trim();
                    if (obj.Address.Length > 1000)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                    }
                }

                /*Position Code*/
                //if (!string.IsNullOrEmpty(obj.PositionCode))
                //{
                //    if (string.IsNullOrEmpty(obj.PlannedCount))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Planned Count ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }

                //    if (string.IsNullOrEmpty(obj.InactiveCount))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Inactive Count ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                //if (!string.IsNullOrEmpty(obj.PlannedCount) || !string.IsNullOrEmpty(obj.InactiveCount))
                //{
                //    if (string.IsNullOrEmpty(obj.PositionCode))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Position Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                /*Planned Count*/
                if (!string.IsNullOrEmpty(obj.PlannedCount))
                {
                    if (!int.TryParse(obj.PlannedCount, out int result))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Planned " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.PlannedCount));
                    }
                }

                ///*Active Count*/
                //if (!string.IsNullOrEmpty(obj.ActiveCount))
                //{
                //    if (!int.TryParse(obj.ActiveCount, out int result))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Active " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.ActiveCount));
                //    }
                //}

                ///*Inactive Count*/
                //if (!string.IsNullOrEmpty(obj.InactiveCount))
                //{
                //    if (!int.TryParse(obj.InactiveCount, out int result))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Inactive " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.InactiveCount));
                //    }
                //}

                /*IsHead*/
                //if (!string.IsNullOrEmpty(obj.IsHead))
                //{
                //    if (!(obj.IsHead == "YES" || obj.IsHead == "NO"))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Is Head ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //    }
                //}
            }

            List<string> orgTagList = (await _referenceDBAccess.GetByRefCodes(param.Select(x => string.Concat("ORG_", x.OrgType, "_TAGS")).Distinct().ToList()))
                                        .Where(x => x.Value == REFERENCEVALUECODE.COMPANY_TAG.ToString())
                                        .Select(x => x.RefCode.Replace("ORG_", "").Replace("_TAGS", "")).Distinct().ToList();

            /*Checking if Code was existing on database*/
            if (ErrorMessages.Count == 0)
            {
                List<string> parentOrgList = (await _dbAccess.GetByCodes(param.Select(x => x.ParentOrgCode).Distinct().ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> orgGroupCodeList = (await _dbAccess.GetByCodes(param.Select(x => x.OrgGroupCode).Distinct().ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> orgTypeList = (await _referenceDBAccess.GetByRefCode(REFERENCEVALUECODE.ORGGROUPTYPE.ToString())).Select(y => y.Value).Distinct().ToList();
                List<string> companyTagList = (await _referenceDBAccess.GetByRefCode(REFERENCEVALUECODE.COMPANY_TAG.ToString())).Select(y => y.Value).Distinct().ToList();
                List<string> positionList = (await _positionDBAccess.GetByCodes(param.Select(x => x.PositionCode).ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> reportingPositionList = (await _positionDBAccess.GetByCodes(param.Select(x => x.ReportingPositionCode).ToList())).Select(x => x.Code).Distinct().ToList();

                foreach (UploadFileEntity obj in param)
                {

                    /*Parent Org Group*/
                    if (!string.IsNullOrEmpty(obj.ParentOrgCode))
                    {
                        if (!parentOrgList.Contains(obj.ParentOrgCode))
                        {
                            bool isExist = param.Select(x => x.OrgGroupCode).Distinct().Contains(obj.ParentOrgCode);

                            if (!isExist)
                            {
                                ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.ParentOrgCode));
                            }
                        }
                    }

                    /*Org Group Code*/
                    if (orgGroupCodeList.Contains(obj.OrgGroupCode))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.SUFF_ERRMSG_REC_EXISTS));
                    }

                    /*Org Type*/
                    if (!orgTypeList.Contains(obj.OrgType))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Type " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.OrgType));
                    }

                    if (orgTagList.Contains(obj.OrgType))
                    {
                        if (string.IsNullOrEmpty(obj.CompanyTag))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Company Tag ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        }
                    }

                    /*Company Tag*/
                    if (!string.IsNullOrEmpty(obj.CompanyTag))
                    {
                        if (!companyTagList.Contains(obj.CompanyTag))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Company Tag " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.CompanyTag));
                        }
                    }

                    /*Position Code*/
                    if (!string.IsNullOrEmpty(obj.PositionCode))
                    {
                        if (!positionList.Contains(obj.PositionCode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Position Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.PositionCode));
                        }
                    }

                    /*Reporting Position Code*/
                    if (!string.IsNullOrEmpty(obj.ReportingPositionCode))
                    {
                        if (!reportingPositionList.Contains(obj.ReportingPositionCode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Reporting Position Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.ReportingPositionCode));
                        }
                    }

                    /*Address*/
                    if (!string.IsNullOrEmpty(obj.Address))
                    {
                        obj.Address = obj.Address.Trim();
                        if (obj.Address.Length > 1000)
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                        }
                    }

                    /*IsBranchActive*/
                    if (!string.IsNullOrEmpty(obj.IsBranchActive))
                    {
                        obj.IsBranchActive = obj.IsBranchActive.Trim();
                        if (!obj.IsBranchActive.Equals("YES") & !obj.IsBranchActive.Equals("NO"))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Is Branch Active " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.IsBranchActive));
                        }
                    }

                    /*ServiceBayCount*/
                    if (!string.IsNullOrEmpty(obj.ServiceBayCount))
                    {
                        obj.ServiceBayCount = obj.ServiceBayCount.Trim();

                        if (!int.TryParse(obj.ServiceBayCount, out int result))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Service Bay Count " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.ServiceBayCount));
                        }
                    }
                    else
                    {
                        obj.ServiceBayCount = "0";
                    }

                }
            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                IEnumerable<Data.Position.Position> positionList = await _positionDBAccess.GetByCodes(param.Select(x => x.PositionCode).ToList());
                IEnumerable<Data.Position.Position> reportingPositionList = await _positionDBAccess.GetByCodes(param.Select(x => x.ReportingPositionCode).ToList());
                List<OrgGroupEntity> lstOrgGroup = new List<OrgGroupEntity>();

                foreach (var obj in param.Select(x => x.OrgGroupCode).Distinct())
                {

                    lstOrgGroup.Add(param.Where(x => x.OrgGroupCode == obj)
                        .Select(x => new OrgGroupEntity
                        {
                            ParentOrgCode = x.ParentOrgCode,
                            OrgGroupCode = x.OrgGroupCode,
                            OrgGroupDescription = x.OrgGroupDescription,
                            OrgType = x.OrgType,
                            Address = x.Address,
                            IsBranchActive = x.IsBranchActive.Equals("YES"),
                            ServiceBayCount = Convert.ToInt32(x.ServiceBayCount),
                            UploadedBy = credentials.UserID,
                            OrgGroupTagList = param.Where(y => y.OrgGroupCode == obj && y.OrgType == x.OrgType
                                && orgTagList.Contains(x.OrgType)).Count() == 0 ? null : new List<OrgGroupTagEntity>
                                {   
                                    //CompanyTag
                                    param.Where(y => y.OrgGroupCode == obj && y.OrgType == x.OrgType && orgTagList.Contains(x.OrgType))
                                    .Select(x => new OrgGroupTagEntity
                                    {
                                        RefCode = REFERENCEVALUECODE.COMPANY_TAG.ToString(),
                                        Value = x.CompanyTag
                                    }).FirstOrDefault()
                                },
                            OrgGroupPositionList = param
                                .Join(positionList,
                                    x => new { x.PositionCode },
                                    y => new { PositionCode = y.Code },
                                   (x, y) => new { x, y })
                                .Where(x => x.x.OrgGroupCode == obj && x.x.PositionCode != "")
                                .Select(x => new OrgGroupPositionEntity
                                {
                                    PositionID = x.y.ID,
                                    PositionCode = x.x.PositionCode,
                                    ReportingPositionID = reportingPositionList.Where(y => y.Code.Equals(x.x.ReportingPositionCode)).ToList().Count > 0 ?
                                        reportingPositionList.Where(z => z.Code.Equals(x.x.ReportingPositionCode)).ToList().First().ID : 0,
                                    PlannedCount = String.IsNullOrEmpty(x.x.PlannedCount) ? 0 : Convert.ToInt32(x.x.PlannedCount),
                                    InactiveCount = 0, //String.IsNullOrEmpty(x.x.InactiveCount) ? 0 : Convert.ToInt32(x.x.InactiveCount),
                                    ActiveCount = 0, //String.IsNullOrEmpty(x.x.ActiveCount) ? 0 : Convert.ToInt32(x.x.ActiveCount),
                                    IsHead = false,
                                    //IsHead = String.IsNullOrEmpty(x.x.IsHead) ? false : x.x.IsHead == "YES" ? true : false,
                                }).ToList()
                        }).FirstOrDefault()
                    );
                }

                await _dbAccess.UploadInsert(lstOrgGroup);

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
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> UploadEdit(APICredentials credentials, List<UploadFileEntity> param)
        {

            /*Checking of required and invalid fields*/
            foreach (UploadFileEntity obj in param)
            {

                /*Parent Org Group*/
                if (!string.IsNullOrEmpty(obj.ParentOrgCode))
                {
                    if (obj.ParentOrgCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                    else
                    {
                        if (!Regex.IsMatch(obj.ParentOrgCode, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }
                    }
                }

                /*Org Group Code*/
                if (string.IsNullOrEmpty(obj.OrgGroupCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.OrgGroupCode.Length > 50)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                }
                else
                {
                    if (!Regex.IsMatch(obj.OrgGroupCode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                }

                /*Org Group Description*/
                if (string.IsNullOrEmpty(obj.OrgGroupDescription))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.OrgGroupDescription.Length > 255)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                }

                /*Org Type*/
                if (string.IsNullOrEmpty(obj.OrgType))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    if (obj.OrgType != ORGGROUPTYPE.TOP.ToString() && string.IsNullOrEmpty(obj.ParentOrgCode))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                }

                /*Address*/
                if (!string.IsNullOrEmpty(obj.Address))
                {
                    obj.Address = obj.Address.Trim();
                    if (obj.Address.Length > 1000)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                    }
                }

                /*Position Code*/
                //if (!string.IsNullOrEmpty(obj.PositionCode))
                //{
                //    if (string.IsNullOrEmpty(obj.PlannedCount))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Planned ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }

                //    if (string.IsNullOrEmpty(obj.InactiveCount))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Inactive ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                //if (!(string.IsNullOrEmpty(obj.PlannedCount) || string.IsNullOrEmpty(obj.InactiveCount)))
                //{
                //    if (string.IsNullOrEmpty(obj.PositionCode))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Position Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                /*Planned Count*/
                if (!string.IsNullOrEmpty(obj.PlannedCount))
                {
                    if (!int.TryParse(obj.PlannedCount, out int result))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Planned " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.PlannedCount));
                    }
                }

                ///*Active Count*/
                //if (!string.IsNullOrEmpty(obj.ActiveCount))
                //{
                //    if (!int.TryParse(obj.ActiveCount, out int result))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Active " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.ActiveCount));
                //    }
                //}

                ///*Inactive Count*/
                //if (!string.IsNullOrEmpty(obj.InactiveCount))
                //{
                //    if (!int.TryParse(obj.InactiveCount, out int result))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Inactive " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.InactiveCount));
                //    }
                //}

                /*IsHead*/
                //if (!string.IsNullOrEmpty(obj.IsHead))
                //{
                //    if (!(obj.IsHead == "YES" || obj.IsHead == "NO"))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Is Head ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //    }
                //}

                /*IsBranchActive*/
                if (!string.IsNullOrEmpty(obj.IsBranchActive))
                {
                    obj.IsBranchActive = obj.IsBranchActive.Trim();
                    if (!obj.IsBranchActive.Equals("YES") & !obj.IsBranchActive.Equals("NO"))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Is Branch Active " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.IsBranchActive));
                    }
                }

                /*ServiceBayCount*/
                if (!string.IsNullOrEmpty(obj.ServiceBayCount))
                {
                    obj.ServiceBayCount = obj.ServiceBayCount.Trim();

                    if (!int.TryParse(obj.ServiceBayCount, out int result))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Service Bay Count " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.ServiceBayCount));
                    }
                }
                else
                {
                    obj.ServiceBayCount = "0";
                }
            }

            List<string> orgTagList = (await _referenceDBAccess.GetByRefCodes(param.Select(x => string.Concat("ORG_", x.OrgType, "_TAGS")).Distinct().ToList()))
                                            .Where(x => x.Value == REFERENCEVALUECODE.COMPANY_TAG.ToString())
                                            .Select(x => x.RefCode.Replace("ORG_", "").Replace("_TAGS", "")).Distinct().ToList();

            /*Checking if Code was existing on database*/
            if (ErrorMessages.Count == 0)
            {
                List<string> parentOrgList = (await _dbAccess.GetByCodes(param.Select(x => x.ParentOrgCode).ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> orgGroupCodeList = (await _dbAccess.GetByCodes(param.Select(x => x.OrgGroupCode).ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> orgTypeList = (await _referenceDBAccess.GetByRefCode(REFERENCEVALUECODE.ORGGROUPTYPE.ToString())).Select(y => y.Value).Distinct().ToList();
                List<string> companyTagList = (await _referenceDBAccess.GetByRefCode(REFERENCEVALUECODE.COMPANY_TAG.ToString())).Select(y => y.Value).Distinct().ToList();
                List<string> positionList = (await _positionDBAccess.GetByCodes(param.Select(x => x.PositionCode).ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> reportingPositionList = (await _positionDBAccess.GetByCodes(param.Select(x => x.ReportingPositionCode).ToList())).Select(x => x.Code).Distinct().ToList();

                foreach (UploadFileEntity obj in param)
                {

                    /*Parent Org Group*/
                    if (!string.IsNullOrEmpty(obj.ParentOrgCode))
                    {
                        if (!parentOrgList.Contains(obj.ParentOrgCode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Parent Org Group " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.ParentOrgCode));
                        }
                    }

                    /*Org Group Code*/
                    if (!orgGroupCodeList.Contains(obj.OrgGroupCode))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.OrgGroupCode));
                    }

                    /*Org Type*/
                    if (!orgTypeList.Contains(obj.OrgType))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Type " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.OrgType));
                    }

                    if (orgTagList.Contains(obj.OrgType))
                    {
                        if (string.IsNullOrEmpty(obj.CompanyTag))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Company Tag ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        }
                    }


                    /*Address*/
                    if (!string.IsNullOrEmpty(obj.Address))
                    {
                        obj.Address = obj.Address.Trim();
                        if (obj.Address.Length > 1000)
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                        }
                    }

                    /*Company Tag*/
                    if (!string.IsNullOrEmpty(obj.CompanyTag))
                    {
                        if (!companyTagList.Contains(obj.CompanyTag))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Company Tag " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.CompanyTag));
                        }
                    }

                    /*Position Code*/
                    if (!string.IsNullOrEmpty(obj.PositionCode))
                    {
                        if (!positionList.Contains(obj.PositionCode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Position Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.PositionCode));
                        }
                    }

                    /*Reporting Position Code*/
                    if (!string.IsNullOrEmpty(obj.ReportingPositionCode))
                    {
                        if (!reportingPositionList.Contains(obj.ReportingPositionCode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Reporting Position Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.ReportingPositionCode));
                        }
                    }
                }
            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                IEnumerable<Data.Position.Position> positionList = await _positionDBAccess.GetByCodes(param.Select(x => x.PositionCode).ToList());
                IEnumerable<Data.Position.Position> reportingPositionList = await _positionDBAccess.GetByCodes(param.Select(x => x.ReportingPositionCode).ToList());
                IEnumerable<Data.OrgGroup.OrgGroup> orgGroupList = await _dbAccess.GetByCodes(param.Select(x => x.OrgGroupCode).ToList());
                IEnumerable<Data.OrgGroup.OrgGroup> parentOrgList = await _dbAccess.GetByCodes(param.Select(x => x.ParentOrgCode).ToList());

                List<Data.OrgGroup.OrgGroup> lstOrgGroup = new List<Data.OrgGroup.OrgGroup>();
                List<Data.OrgGroup.OrgGroupPosition> lstOrgGroupPosition = new List<Data.OrgGroup.OrgGroupPosition>();
                List<Data.OrgGroup.OrgGroupTag> lstOrgGroupTag = new List<Data.OrgGroup.OrgGroupTag>();

                List<Data.OrgGroup.OrgGroupPosition> OldOrgPosition = (await _dbAccess.GetOrgGroupPositionByOrgCodes(param.Select(x => x.OrgGroupCode).Distinct().ToList())).Where(x => x.IsActive).ToList();
                List<Data.OrgGroup.OrgGroupTag> OldOrgTag = (await _dbAccess.GetOrgGroupTagByOrgCodes(param.Select(x => x.OrgGroupCode).ToList())).ToList();

                foreach (var obj in param.Select(x => x.OrgGroupCode).Distinct())
                {

                    lstOrgGroup.Add(parentOrgList
                        .Join(param,
                            x => new { x.Code },
                            y => new { Code = y.ParentOrgCode },
                           (x, y) => new { x, y })
                        .Join(orgGroupList,
                            x => new { x.y.OrgGroupCode },
                            y => new { OrgGroupCode = y.Code },
                           (x, y) => new { x, y })
                        .Where(x => x.x.y.OrgGroupCode == obj)
                        .Select(x => new Data.OrgGroup.OrgGroup
                        {
                            ID = x.y.ID,
                            Code = x.x.y.OrgGroupCode,
                            Description = x.x.y.OrgGroupDescription,
                            OrgType = x.y.OrgType,
                            Address = x.y.Address,
                            IsBranchActive = x.x.y.IsBranchActive.Equals("YES"),
                            ServiceBayCount = Convert.ToInt32(x.x.y.ServiceBayCount),
                            ParentOrgID = x.y.ParentOrgID,
                            CSODAM = x.y.CSODAM,
                            HRBP = x.y.HRBP,
                            RRT = x.y.RRT,
                            Category = x.y.Category,
                            Email = x.y.Email,
                            Number = x.y.Number,
                            Psgc = x.y.Psgc,
                            BranchSize = x.y.BranchSize,
                            Parkingsize = x.y.Parkingsize,
                            Sign = x.y.Sign,
                            Page = x.y.Page,
                            IsLatest = x.y.IsLatest,
                            TDate = x.y.TDate,
                            IsActive = true,
                            CreatedBy = x.y.CreatedBy,
                            CreatedDate = x.y.CreatedDate,
                            ModifiedBy = credentials.UserID,
                            ModifiedDate = DateTime.Now
                        }).FirstOrDefault());

                    lstOrgGroupPosition.AddRange(orgGroupList
                        .Join(param,
                            x => new { x.Code },
                            y => new { Code = y.OrgGroupCode },
                           (x, y) => new { x, y })
                        .Join(positionList,
                            x => new { x.y.PositionCode },
                            y => new { PositionCode = y.Code },
                           (x, y) => new { x, y })
                        .Where(x => x.x.y.OrgGroupCode == obj)
                        .Select(y => new Data.OrgGroup.OrgGroupPosition
                        {
                            OrgGroupID = y.x.x.ID,
                            PositionID = y.y.ID,
                            ReportingPositionID = reportingPositionList.Where(x => x.Code.Equals(y.x.y.ReportingPositionCode)).ToList().Count > 0 ?
                            reportingPositionList.Where(x => x.Code.Equals(y.x.y.ReportingPositionCode)).ToList().First().ID : 0,
                            PlannedCount = String.IsNullOrEmpty(y.x.y.PlannedCount) ? 0 : Convert.ToInt32(y.x.y.PlannedCount),
                            InactiveCount = 0, //String.IsNullOrEmpty(y.x.y.InactiveCount) ? 0 : Convert.ToInt32(y.x.y.InactiveCount),
                            ActiveCount = 0, //String.IsNullOrEmpty(y.x.y.ActiveCount) ? 0 : Convert.ToInt32(y.x.y.ActiveCount),
                            //IsHead = false
                            IsHead = String.IsNullOrEmpty(y.x.y.IsHead) ? false : y.x.y.IsHead == "YES" ? true : false
                        }).ToList());


                    lstOrgGroupTag.AddRange(lstOrgGroup
                        .Join(param,
                            x => new { x.Code },
                            y => new { Code = y.OrgGroupCode },
                           (x, y) => new { x, y })
                        .Where(x => x.y.OrgGroupCode == obj && x.y.OrgType == x.x.OrgType
                            && orgTagList.Contains(x.y.OrgType))
                        .Count() == 0 ? new List<OrgGroupTag>() : new List<OrgGroupTag>
                            {
                                lstOrgGroup
                                .Join(param,
                                    x => new { x.Code },
                                    y => new { Code = y.OrgGroupCode },
                                   (x, y) => new { x, y })
                                .Where(x => x.y.OrgGroupCode == obj && orgTagList.Contains(x.y.OrgType))
                                .Select(y => new OrgGroupTag
                                {
                                    OrgGroupID = y.x.ID,
                                    TagRefCode = REFERENCEVALUECODE.COMPANY_TAG.ToString(),
                                    TagValue = y.y.CompanyTag
                                }).FirstOrDefault()
                            }
                        );
                }

                List<Data.OrgGroup.OrgGroupPosition> OrgPositionToAdd = GetOrgGroupPositionToAdd(OldOrgPosition,
                    lstOrgGroupPosition.Select(x => new Data.OrgGroup.OrgGroupPosition
                    {
                        OrgGroupID = x.OrgGroupID,
                        PositionID = x.PositionID,
                        ReportingPositionID = x.ReportingPositionID,
                        PlannedCount = x.PlannedCount,
                        InactiveCount = x.InactiveCount,
                        ActiveCount = x.ActiveCount,
                        IsHead = x.IsHead,
                        CreatedBy = credentials.UserID,
                        IsActive = true
                    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupPosition> OrgPositionToUpdate = GetOrgGroupPositionToUpdate(OldOrgPosition, lstOrgGroupPosition
                    .Join(OldOrgPosition,
                        x => new { x.OrgGroupID, x.PositionID },
                        y => new { y.OrgGroupID, y.PositionID },
                       (x, y) => new { x, y })
                    .Select(x => new Data.OrgGroup.OrgGroupPosition
                    {
                        OrgGroupID = x.x.OrgGroupID,
                        PositionID = x.x.PositionID,
                        ReportingPositionID = x.x.ReportingPositionID,
                        PlannedCount = x.x.PlannedCount,
                        InactiveCount = x.x.InactiveCount,
                        ActiveCount = x.x.ActiveCount,
                        IsHead = x.x.IsHead,
                        IsActive = true,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();

                //List<Data.OrgGroup.OrgGroupPosition> OrgPositionToDelete = GetOrgGroupPositionToDelete(OldOrgPosition,
                //    lstOrgGroupPosition.Select(x => new Data.OrgGroup.OrgGroupPosition
                //    {
                //        OrgGroupID = x.OrgGroupID,
                //        PositionID = x.PositionID,
                //        IsActive = false,
                //        ModifiedBy = credentials.UserID,
                //        ModifiedDate = DateTime.Now
                //    }).ToList()).ToList();

                List<Data.OrgGroup.OrgGroupTag> OrgTagToAdd = GetOrgGroupTagToAdd(OldOrgTag,
                    lstOrgGroupTag ?? new List<Data.OrgGroup.OrgGroupTag>()).ToList();

                List<Data.OrgGroup.OrgGroupTag> OrgTagToUpdate = GetOrgGroupTagToUpdate(OldOrgTag,
                    lstOrgGroupTag ?? new List<Data.OrgGroup.OrgGroupTag>()).ToList();

                //List<Data.OrgGroup.OrgGroupTag> OrgTagToDelete = GetOrgGroupTagToDelete(OldOrgTag,
                //    lstOrgGroupTag ?? new List<Data.OrgGroup.OrgGroupTag>()).ToList();


                await _dbAccess.UploadEdit(lstOrgGroup, OrgPositionToAdd, OrgPositionToUpdate.GroupBy(x => x.ID).Select(x => x.First()).ToList()/*, OrgPositionToDelete*/,
                    OrgTagToAdd, OrgTagToUpdate/*, OrgTagToDelete*/);

                _resultView.IsSuccess = true;
            }

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Batch update was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_FILE_UPDATE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        private IEnumerable<Data.OrgGroup.OrgGroupPosition> GetOrgGroupPositionToAdd(List<Data.OrgGroup.OrgGroupPosition> left, List<Data.OrgGroup.OrgGroupPosition> right)
        {
            return right.GroupJoin(
                left,
                     x => new { x.OrgGroupID, x.PositionID },
                     y => new { y.OrgGroupID, y.PositionID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.OrgGroup.OrgGroupPosition
                    {
                        OrgGroupID = x.newSet.newSet.OrgGroupID,
                        PositionID = x.newSet.newSet.PositionID,
                        PlannedCount = x.newSet.newSet.PlannedCount,
                        ActiveCount = x.newSet.newSet.ActiveCount,
                        InactiveCount = x.newSet.newSet.InactiveCount,
                        IsHead = x.newSet.newSet.IsHead,
                        IsActive = true,
                        CreatedBy = x.newSet.newSet.CreatedBy,
                        ReportingPositionID = x.newSet.newSet.ReportingPositionID
                    }).ToList();
        }

        private IEnumerable<Data.OrgGroup.OrgGroupPosition> GetOrgGroupPositionToUpdate(List<Data.OrgGroup.OrgGroupPosition> left, List<Data.OrgGroup.OrgGroupPosition> right)
        {
            return left.Join(
                right,
                     x => new { x.OrgGroupID, x.PositionID },
                     y => new { y.OrgGroupID, y.PositionID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.PlannedCount.Equals(x.newSet.PlannedCount) ||
                            !x.oldSet.ActiveCount.Equals(x.newSet.ActiveCount) ||
                            !x.oldSet.InactiveCount.Equals(x.newSet.InactiveCount) ||
                            x.oldSet.ReportingPositionID != x.newSet.ReportingPositionID ||
                            (x.oldSet.IsHead != x.newSet.IsHead))
                .Select(y =>
                    new Data.OrgGroup.OrgGroupPosition
                    {
                        ID = y.oldSet.ID,
                        OrgGroupID = y.newSet.OrgGroupID,
                        PositionID = y.newSet.PositionID,
                        PlannedCount = y.newSet.PlannedCount,
                        ActiveCount = y.newSet.ActiveCount,
                        InactiveCount = y.newSet.InactiveCount,
                        IsHead = y.newSet.IsHead,
                        IsActive = true,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = y.newSet.ModifiedBy,
                        ModifiedDate = y.newSet.ModifiedDate,
                        ReportingPositionID = y.newSet.ReportingPositionID
                    }).ToList();
        }

        private IEnumerable<Data.OrgGroup.OrgGroupPosition> GetOrgGroupPositionToDelete(List<Data.OrgGroup.OrgGroupPosition> left, List<Data.OrgGroup.OrgGroupPosition> right)
        {
            return left.GroupJoin(
                right,
                     x => new { x.OrgGroupID, x.PositionID },
                     y => new { y.OrgGroupID, y.PositionID },
                (x, y) => new { oldSet = x, newSet = y })
                .SelectMany(x => x.newSet.DefaultIfEmpty(),
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.newSet == null)
                .Select(x =>
                    new Data.OrgGroup.OrgGroupPosition
                    {
                        ID = x.oldSet.oldSet.ID,
                        OrgGroupID = x.oldSet.oldSet.OrgGroupID,
                        PositionID = x.oldSet.oldSet.PositionID,
                        PlannedCount = x.oldSet.oldSet.PlannedCount,
                        ActiveCount = x.oldSet.oldSet.ActiveCount,
                        InactiveCount = x.oldSet.oldSet.InactiveCount,
                        CreatedBy = x.oldSet.oldSet.CreatedBy,
                        CreatedDate = x.oldSet.oldSet.CreatedDate,
                        ReportingPositionID = x.oldSet.oldSet.ReportingPositionID
                    }).ToList();
        }

        private IEnumerable<Data.OrgGroup.OrgGroupTag> GetOrgGroupTagToAdd(List<Data.OrgGroup.OrgGroupTag> left, List<Data.OrgGroup.OrgGroupTag> right)
        {
            return right.GroupJoin(
                left,
                     x => new { x.OrgGroupID, x.TagRefCode },
                     y => new { y.OrgGroupID, y.TagRefCode },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.OrgGroup.OrgGroupTag
                    {
                        OrgGroupID = x.newSet.newSet.OrgGroupID,
                        TagRefCode = x.newSet.newSet.TagRefCode,
                        TagValue = x.newSet.newSet.TagValue
                    }).ToList();
        }

        private IEnumerable<Data.OrgGroup.OrgGroupTag> GetOrgGroupTagToUpdate(List<Data.OrgGroup.OrgGroupTag> left, List<Data.OrgGroup.OrgGroupTag> right)
        {
            return left.Join(
                right,
                     x => new { x.OrgGroupID, x.TagRefCode },
                     y => new { y.OrgGroupID, y.TagRefCode },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.TagValue.Equals(x.newSet.TagValue))
                .Select(y =>
                    new Data.OrgGroup.OrgGroupTag
                    {
                        ID = y.oldSet.ID,
                        OrgGroupID = y.newSet.OrgGroupID,
                        TagRefCode = y.newSet.TagRefCode,
                        TagValue = y.newSet.TagValue
                    }).ToList();
        }

        private IEnumerable<Data.OrgGroup.OrgGroupTag> GetOrgGroupTagToDelete(List<Data.OrgGroup.OrgGroupTag> left, List<Data.OrgGroup.OrgGroupTag> right)
        {
            return left.GroupJoin(
                right,
                     x => new { x.OrgGroupID, x.TagRefCode },
                     y => new { y.OrgGroupID, y.TagRefCode },
                (x, y) => new { oldSet = x, newSet = y })
                .SelectMany(x => x.newSet.DefaultIfEmpty(),
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.newSet == null)
                .Select(x =>
                    new Data.OrgGroup.OrgGroupTag
                    {
                        ID = x.oldSet.oldSet.ID
                    }).ToList();
        }

        public async Task<IActionResult> GetByOrgGroupAndPosition(APICredentials credentials, GetByOrgGroupIDAndPositionIDInput param)
        {
            //Data.OrgGroup.OrgGroupPosition result = await _dbAccess.GetByOrgGroupAndPosition(param.OrgGroupID, param.PositionID);

            List<TableVarOrgGroupPosition> orgGroup = (await _dbAccess.GetOrgGroupPositionByOrgID(param.OrgGroupID)).Where(x => x.PositionID == param.PositionID).ToList();
            Data.OrgGroup.OrgGroupPosition result = null;
            if (orgGroup != null)
            {
                if (orgGroup.Count > 0)
                {
                    result = new OrgGroupPosition
                    {
                        OrgGroupID = param.OrgGroupID,
                        PositionID = orgGroup.FirstOrDefault().PositionID,
                        PlannedCount = orgGroup.FirstOrDefault().PlannedCount,
                        ActiveCount = (orgGroup.FirstOrDefault().ActiveCount + orgGroup.FirstOrDefault().ActiveProbCount),
                        InactiveCount = (orgGroup.FirstOrDefault().InactiveCount + orgGroup.FirstOrDefault().OutgoingCount),
                        IsHead = orgGroup.FirstOrDefault().IsHead
                    };
                }
            }

            return new OkObjectResult(result == null ? new GetByOrgGroupIDAndPositionIDOutput() :
            new GetByOrgGroupIDAndPositionIDOutput
            {
                OrgGroupID = result.OrgGroupID,
                PositionID = result.PositionID,
                PlannedCount = result.PlannedCount,
                ActiveCount = result.ActiveCount,
                InactiveCount = result.InactiveCount,
                IsHead = result.IsHead
            });
        }

        public async Task<IActionResult> GetExportList(APICredentials credentials, GetListInput input)
        {
            IEnumerable<TableVarOrgGroupExportList> result = await _dbAccess.GetExportList(input);

            if (result?.Count() == 0)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_REC_EXPORT);
            else
                return new OkObjectResult(result.Select(x => new GetExportListOutput
                {
                    ParentOrgGroup = x.ParentOrgGroup,
                    OrgGroupCode = x.OrgGroupCode,
                    OrgGroupDesc = x.OrgGroupDesc,
                    OrgType = x.OrgType,
                    Address = x.Address,
                    IsBranchActive = x.IsBranchActive,
                    ServiceBayCount = x.ServiceBayCount,
                    CompanyTag = x.CompanyTag,
                    PositionCode = x.PositionCode,
                    ReportingPositionCode = x.ReportingPositionCode,
                    PlannedCount = x.PlannedCount,
                    ActiveCount = x.ActiveCount,
                    InactiveCount = x.InactiveCount,
                    IsHead = x.IsHead
                }).ToList()
                );
        }

        public async Task<IActionResult> GetOrgGroupHierarchy(APICredentials credentials, int ID)
        {
            IEnumerable<TableVarOrgGroupHierarchy> result = await _dbAccess.GetOrgGroupHierarchy(ID);

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> GetOrgGroupHierarchyBomd(APICredentials credentials, int ID)
        {
            IEnumerable<TableVarOrgGroupHierarchy> result = await _dbAccess.GetOrgGroupHierarchyBomd(ID);

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> GetOrgGroupEmployeeList(APICredentials credentials, GetEmployeeListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;
            IEnumerable<TableVarOrgGroupEmployee> result = await _dbAccess.GetOrgGroupEmployeeList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetOrgGroupEmployeeOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                Position = x.Position,
                EmployeeName = x.EmployeeName
            }).ToList());
        }

        public async Task<IActionResult> GetExportCountByOrgTypeData(APICredentials credentials, GetPlantillaCountInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarPlantillaCount> result = await _dbAccess.GetExportCountByOrgTypeData(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetPlantillaCountOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ScopeOrgGroupID = x.ScopeOrgGroupID,
                ScopeOrgGroup = x.ScopeOrgGroup,
                OrgGroupID = x.OrgGroupID,
                OrgGroup = x.OrgGroup,
                PositionID = x.PositionID,
                Position = x.Position,
                PlannedCount = x.PlannedCount,
                ActiveCount = x.ActiveCount,
                ActiveProbCount = x.ActiveProbCount,
                OutgoingCount = x.OutgoingCount,
                TotalActiveCount = x.TotalActiveCount,
                InactiveCount = x.InactiveCount,
                VarianceCount = x.VarianceCount,
                TotalPlanned = x.TotalPlanned,
                TotalActiveReg = x.TotalActiveReg,
                TotalActiveProb = x.TotalActiveProb,
                TotalActive = x.TotalActive,
                TotalInactive = x.TotalInactive,
                TotalOutgoing = x.TotalOutgoing,
                TotalVariance = x.TotalVariance
            }).ToList());
        }

        public async Task<IActionResult> GetIDByOrgTypeAutoComplete(APICredentials credentials, GetByOrgTypeAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByOrgGroupAutoComplete(param))
                .Select(x => new GetIDByOrgTypeAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }

        public async Task<IActionResult> GetOrgGroupHierarchyLevelsDropDown(APICredentials credentials)
        {
            return new OkObjectResult(

                SharedUtilities.GetDropdown((
                await _dbAccess.GetOrgGroupHierarchyLevels()).OrderBy(x => x.HierarchyLevel).ToList()
                , "HierarchyLevel", "HierarchyLevel")
            );
        }

        public async Task<IActionResult> GetOrgGroupDescendants(APICredentials credentials, int OrgGroupID)
        {
            return new OkObjectResult(await _dbAccess.GetDescendants(OrgGroupID));
        }
        public async Task<IActionResult> GetOrgGroupDescendantsList(APICredentials credentials, List<int> OrgGroupIDs)
        {
            return new OkObjectResult(await _dbAccess.GetOrgGroupDescendantsList(OrgGroupIDs));
        }
        public async Task<IActionResult> GetOrgGroupDescendantsBomd(APICredentials credentials, int OrgGroupID)
        {
            return new OkObjectResult(await _dbAccess.GetDescendantsBomd(OrgGroupID));
        }

        public async Task<IActionResult> GetPositionWithDescription(APICredentials credentials, int OrgGroupID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetOrgGroupPositionByOrgID(OrgGroupID))
                .Select(x => new PlantillaCountUpdateForm
                {
                    OrgGroupID = OrgGroupID,
                    PositionID = x.PositionID,
                    PositionDescription = x.PositionDescription,
                    PlannedCount = x.PlannedCount,
                    ActiveCount = x.ActiveCount,
                    InactiveCount = x.InactiveCount,
                    ReportingPositionID = x.ReportingPositionID,
                    ReportingPositionDescription = x.ReportingPositionDescription,
                    IsHead = x.IsHead
                }).ToList()
                );
        }

        public async Task<IActionResult> GetOrgGroupNPRF(APICredentials credentials, int OrgGroupID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetOrgGroupNPRF(OrgGroupID))
                .Select(x => new OrgGroupNPRFForm
                {
                    NPRFNumber = x.NPRFNumber,
                    ApprovedDate = x.ApprovedDate,
                    SourceFile = x.SourceFile,
                    ServerFile = x.ServerFile,
                    Timestamp = x.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    CreatedBy = x.CreatedBy
                }).ToList()
                );
        }

        public async Task<IActionResult> UpdatePlantillaCount(APICredentials credentials, List<PlantillaCountUpdateForm> param)
        {
            List<Data.OrgGroup.OrgGroupPosition> OldOrgPosition = (await _dbAccess.GetOrgGroupPosition(param.First().OrgGroupID)).ToList();

            List<Data.OrgGroup.OrgGroupPosition> OrgPositionToUpdate = GetOrgGroupPositionToUpdate(OldOrgPosition,
                   param == null ? new List<Data.OrgGroup.OrgGroupPosition>() :
                   param.Select(x => new Data.OrgGroup.OrgGroupPosition
                   {
                       OrgGroupID = x.OrgGroupID,
                       PositionID = x.PositionID,
                       PlannedCount = x.PlannedCount,
                       ActiveCount = x.ActiveCount,
                       InactiveCount = x.InactiveCount,
                       ModifiedBy = credentials.UserID,
                       ModifiedDate = DateTime.Now,
                       IsActive = true,
                       ReportingPositionID = x.ReportingPositionID,
                       IsHead = x.IsHead
                   }).ToList()).ToList();

            if (await _dbAccess.UpdatePlantillaCount(OrgPositionToUpdate))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> AddNPRF(APICredentials credentials, List<OrgGroupNPRFForm> param)
        {
            param = param.Select(x => {
                x.NPRFNumber = x.NPRFNumber.Trim();
                if (string.IsNullOrEmpty(x.NPRFNumber))
                    ErrorMessages.Add("NPRFNumber " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
                else
                    if (x.NPRFNumber.Length > 255)
                    ErrorMessages.Add(string.Concat("NPRFNumber", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                return x;
            }).ToList();

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.AddNPRF(param.Select(x => new OrgGroupNPRF
                {
                    OrgGroupID = x.OrgGroupID,
                    NPRFNumber = x.NPRFNumber,
                    ApprovedDate = x.ApprovedDate,
                    CreatedBy = credentials.UserID,
                    SourceFile = x.SourceFile,
                    ServerFile = x.ServerFile
                }).ToList());

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetByIDs(APICredentials credentials, List<int> IDs)
        {
            List<Data.OrgGroup.OrgGroup> result = (await _dbAccess.GetByIDs(IDs)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                    result.Select(x =>
                new Form
                {
                    ID = x.ID,
                    Code = x.Code,
                    Description = x.Description,
                    OrgType = x.OrgType,
                    Address = x.Address,
                    IsBranchActive = x.IsBranchActive,
                    ServiceBayCount = x.ServiceBayCount,
                    ParentOrgID = x.ParentOrgID,
                    CreatedBy = x.CreatedBy
                }).ToList());
        }

        public async Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "Code", "Code", "Description", param.ID)
                );
        }

        public async Task<IActionResult> GetOrgGroupRollupPositionDropdown(APICredentials credentials, int OrgGroupID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetOrgGroupRollupPositionDropdown(OrgGroupID))
                .Select(x => new GetOrgGroupRollupPositionDropdownOutput
                {
                    ID = x.ID,
                    OrgGroupID = x.OrgGroupID,
                    OrgGroup = x.OrgGroup,
                    Position = x.Position,
                    PlannedCount = x.PlannedCount,
                    ActiveCount = x.ActiveCount,
                    InactiveCount = x.InactiveCount,
                    VarianceCount = x.VarianceCount
                })
            );
        }

        public async Task<IActionResult> GetRegionByOrgGroupID(APICredentials credentials, int OrgGroupID)
        {
            var result = await _dbAccess.GetRegionByOrgGroupID(OrgGroupID);
            return new OkObjectResult(
                new GetRegionByOrgGroupIDOutput
                {
                    Region = result.Region,
                    RegionCode = result.RegionCode,
                    MonthlyRate = result.MonthlyRate
                }
            );
        }

        public async Task<IActionResult> GetByCodes(APICredentials credentials, string CodesDelimited)
        {
            List<string> CodesList = CodesDelimited == null ? new List<string>() : CodesDelimited.Split(",").ToList();

            List<Data.OrgGroup.OrgGroup> result = (await _dbAccess.GetByCodes(CodesList)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                    result.Select(x =>
                new Form
                {
                    ID = x.ID,
                    ParentOrgID = x.ParentOrgID,
                    Code = x.Code,
                    Description = x.Description,
                    OrgType = x.OrgType,
                    Address = x.Address,
                    IsBranchActive = x.IsBranchActive,
                    ServiceBayCount = x.ServiceBayCount,
                }).ToList());
        }

        public async Task<IActionResult> GetPositionUpwardAutoComplete(APICredentials credentials, GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetPositionUpwardAutoComplete(param))
                .Select(x => new GetPositionOrgGroupUpwardAutoCompleteOutput
                {
                    ID = x.ID,
                    Position = x.Position
                })
            );
        }

        public async Task<IActionResult> GetOrgGroupUpwardAutoComplete(APICredentials credentials, GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetOrgGroupUpwardAutoComplete(param))
                .Select(x => new GetPositionOrgGroupUpwardAutoCompleteOutput
                {
                    ID = x.ID,
                    OrgGroup = x.OrgGroup
                })
            );
        }

        public async Task<IActionResult> GetBranchInfoList(APICredentials credentials, GetBranchInfoListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarOrgGroupBranchInfo> result = await _dbAccess.GetBranchInfoList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetBranchListListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                Description = x.Description,
                Category = x.Category,
                Email = x.Email,
                Number = x.Number,
                Address = x.Address,
                OrgTypeDescription = x.OrgTypeDescription,
                ParentOrgDescription = x.ParentOrgDescription,
                IsBranchActive = x.IsBranchActive,
                ServiceBayCount = x.ServiceBayCount,

            }).ToList());
        }

        public async Task<IActionResult> GetOrgGroupHistoryList(APICredentials credentials, OrgGroupHistoryListInput param)
        {
            int rowStart = 1;
            rowStart = param.pageNumber > 1 ? param.pageNumber * param.rows - param.rows + 1 : rowStart;
            IEnumerable<TableVarOrgGroupHistoryList> result = await _dbAccess.GetOrgGroupHistoryList(param, rowStart);
            return new OkObjectResult(result.Select(x => new GetOrgGroupHistoryListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)param.rows),
                ID = x.ID,
                TDate = x.TDate,
                IsLatest = x.IsLatest,
                IsLatestDescription = x.IsLatestDescription
            }).ToList());
        }

        public async Task<IActionResult> GetOrgGroupHistoryByDate(APICredentials credentials, OrgGroupHistoryByDateInput param)
        {
            int rowStart = 1;
            rowStart = param.pageNumber > 1 ? param.pageNumber * param.rows - param.rows + 1 : rowStart;
            IEnumerable<TableVarOrgGroupHistoryByDate> result = await _dbAccess.GetOrgGroupHistoryByDate(param, rowStart);
            return new OkObjectResult(result.Select(x => new GetOrgGroupHistoryByDateOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)param.rows),
                ID = x.ID,
                TDate = x.TDate,
                OrgType = x.OrgType,
                IsLatest = x.IsLatest,
                IsLatestDescription = x.IsLatestDescription,
                Code = x.Code,
                Description = x.Description,
                ParentOrgId = x.ParentOrgId,
                ParentDescription = x.ParentDescription
            }).ToList());
        }

        public async Task<IActionResult> AddOrgGroupHistory(APICredentials credentials, string TDate, bool IsLatest, List<AddOrgGroupHistoryInput> param)
        {
            List<Data.OrgGroup.OrgGroup> updateOrgGroup = new List<Data.OrgGroup.OrgGroup>();
            List<Data.OrgGroup.OrgGroupHistory> updateOrgGroupHistory = new List<Data.OrgGroup.OrgGroupHistory>();
            List<Data.OrgGroup.OrgGroupHistory> OrgGroupHistory = new List<Data.OrgGroup.OrgGroupHistory>();

            List<Data.OrgGroup.OrgGroup> oldOrgGroup = ((List<Data.OrgGroup.OrgGroup>)await _dbAccess.GetAll()).ToList();
            List<Data.OrgGroup.OrgGroupHistory> oldOrgGroupHistory = ((List<Data.OrgGroup.OrgGroupHistory>)await _dbAccess.GetAllHistory()).Where(x => x.IsLatest.Equals(IsLatest) && x.TDate.Equals(Convert.ToDateTime(TDate)) && x.IsActive && x.IsBranchActive).ToList();
            List<Data.OrgGroup.OrgGroupHistory> oldOrgGroupHistoryUpdate = ((List<Data.OrgGroup.OrgGroupHistory>)await _dbAccess.GetAllHistory()).Where(x => x.IsLatest.Equals(IsLatest) && x.IsActive && x.IsBranchActive).ToList();

            List<Data.OrgGroup.OrgGroup> newOrgGroup = param.Select(x =>
                new Data.OrgGroup.OrgGroup()
                {
                    Code = x.Code,
                    ParentOrgID = (x.ParentCodeDescriptionValue == null ? x.ParentOrgId : Convert.ToInt32(x.ParentCodeDescriptionValue)),
                    TDate = Convert.ToDateTime(TDate),
                    IsLatest = IsLatest
                }
            ).ToList();

            if (IsLatest && oldOrgGroupHistory.Count() == 0 && Convert.ToDateTime(TDate) <= DateTime.Now) // (Add button) No update on old group history and date <
            {
                OrgGroupHistory = (from left in oldOrgGroupHistoryUpdate
                                   join right in newOrgGroup on left.Code equals right.Code into joinedList
                                   from sub in joinedList.DefaultIfEmpty()
                                   select new Data.OrgGroup.OrgGroupHistory()
                                   {
                                       ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                       IsLatest = IsLatest,
                                       TDate = Convert.ToDateTime(TDate),
                                       Code = left.Code,
                                       Description = left.Description,
                                       CSODAM = left.CSODAM,
                                       HRBP = left.HRBP,
                                       RRT = left.RRT,
                                       Category = left.Category,
                                       Email = left.Email,
                                       Number = left.Number,
                                       OrgType = left.OrgType,
                                       Psgc = left.Psgc,
                                       Address = left.Address,
                                       BranchSize = left.BranchSize,
                                       Parkingsize = left.Parkingsize,
                                       Sign = left.Sign,
                                       Page = left.Page,
                                       IsBranchActive = left.IsBranchActive,
                                       ServiceBayCount = left.ServiceBayCount,
                                       IsActive = left.IsActive,
                                       CreatedBy = credentials.UserID,
                                       CreatedDate = DateTime.Now
                                   }).ToList();
                
                updateOrgGroupHistory = oldOrgGroupHistoryUpdate.Select(x => new Data.OrgGroup.OrgGroupHistory()
                {
                    ID = x.ID,
                    ParentOrgID = x.ParentOrgID,
                    IsLatest = false,
                    TDate = x.TDate,
                    Code = x.Code,
                    Description = x.Description,
                    CSODAM = x.CSODAM,
                    HRBP = x.HRBP,
                    RRT = x.RRT,
                    Category = x.Category,
                    Email = x.Email,
                    Number = x.Number,
                    OrgType = x.OrgType,
                    Psgc = x.Psgc,
                    Address = x.Address,
                    BranchSize = x.BranchSize,
                    Parkingsize = x.Parkingsize,
                    Sign = x.Sign,
                    Page = x.Page,
                    IsBranchActive = x.IsBranchActive,
                    ServiceBayCount = x.ServiceBayCount,
                    IsActive = x.IsActive,
                    ModifiedBy = credentials.UserID,
                    ModifiedDate = DateTime.Now
                }).ToList();

                updateOrgGroup = (from left in oldOrgGroup
                                    join right in newOrgGroup on left.Code equals right.Code into joinedList
                                    from sub in joinedList.DefaultIfEmpty()
                                    select new Data.OrgGroup.OrgGroup()
                                    {
                                        ID = left.ID,
                                        ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                        IsLatest = IsLatest,
                                        TDate = Convert.ToDateTime(TDate),
                                        Code = left.Code,
                                        Description = left.Description,
                                        CSODAM = left.CSODAM,
                                        HRBP = left.HRBP,
                                        RRT = left.RRT,
                                        Category = left.Category,
                                        Email = left.Email,
                                        Number = left.Number,
                                        OrgType = left.OrgType,
                                        Psgc = left.Psgc,
                                        Address = left.Address,
                                        BranchSize = left.BranchSize,
                                        Parkingsize = left.Parkingsize,
                                        Sign = left.Sign,
                                        Page = left.Page,
                                        IsBranchActive = left.IsBranchActive,
                                        ServiceBayCount = left.ServiceBayCount,
                                        IsActive = left.IsActive,
                                        ModifiedBy = credentials.UserID,
                                        ModifiedDate = DateTime.Now,
                                        CreatedBy = left.CreatedBy,
                                        CreatedDate = left.CreatedDate
                                    }).ToList();
            }
            else if (IsLatest && oldOrgGroupHistory.Count() > 0 && Convert.ToDateTime(TDate) <= DateTime.Now) // old_group_history has been edited and date <
            {
                if (Convert.ToDateTime(TDate) <= DateTime.Now)
                {
                    updateOrgGroup = (from left in oldOrgGroup
                                      join right in newOrgGroup on left.Code equals right.Code into joinedList
                                      from sub in joinedList.DefaultIfEmpty()
                                      select new Data.OrgGroup.OrgGroup()
                                      {
                                          ID = left.ID,
                                          ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                          IsLatest = IsLatest,
                                          TDate = Convert.ToDateTime(TDate),
                                          Code = left.Code,
                                          Description = left.Description,
                                          CSODAM = left.CSODAM,
                                          HRBP = left.HRBP,
                                          RRT = left.RRT,
                                          Category = left.Category,
                                          Email = left.Email,
                                          Number = left.Number,
                                          OrgType = left.OrgType,
                                          Psgc = left.Psgc,
                                          Address = left.Address,
                                          BranchSize = left.BranchSize,
                                          Parkingsize = left.Parkingsize,
                                          Sign = left.Sign,
                                          Page = left.Page,
                                          IsBranchActive = left.IsBranchActive,
                                          ServiceBayCount = left.ServiceBayCount,
                                          IsActive = left.IsActive,
                                          ModifiedBy = credentials.UserID,
                                          ModifiedDate = DateTime.Now,
                                          CreatedBy = left.CreatedBy,
                                          CreatedDate = left.CreatedDate
                                      }).ToList();
                }
                updateOrgGroupHistory = (from left in oldOrgGroupHistory
                                   join right in newOrgGroup on left.Code equals right.Code into joinedList
                                   from sub in joinedList.DefaultIfEmpty()
                                   select new Data.OrgGroup.OrgGroupHistory()
                                   {
                                       ID = left.ID,
                                       ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                       IsLatest = IsLatest,
                                       TDate = Convert.ToDateTime(TDate),
                                       Code = left.Code,
                                       Description = left.Description,
                                       CSODAM = left.CSODAM,
                                       HRBP = left.HRBP,
                                       RRT = left.RRT,
                                       Category = left.Category,
                                       Email = left.Email,
                                       Number = left.Number,
                                       OrgType = left.OrgType,
                                       Psgc = left.Psgc,
                                       Address = left.Address,
                                       BranchSize = left.BranchSize,
                                       Parkingsize = left.Parkingsize,
                                       Sign = left.Sign,
                                       Page = left.Page,
                                       IsBranchActive = left.IsBranchActive,
                                       ServiceBayCount = left.ServiceBayCount,
                                       IsActive = left.IsActive,
                                       ModifiedBy = credentials.UserID,
                                       ModifiedDate = DateTime.Now
                                   }).ToList();
            }
            else if (IsLatest && oldOrgGroupHistory.Count() == 0 && Convert.ToDateTime(TDate) > DateTime.Now) // old_group history is not edited and date >
            {
                OrgGroupHistory = (from left in oldOrgGroupHistoryUpdate
                                   join right in newOrgGroup on left.Code equals right.Code into joinedList
                                   from sub in joinedList.DefaultIfEmpty()
                                   select new Data.OrgGroup.OrgGroupHistory()
                                   {
                                       ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                       IsLatest = false,
                                       TDate = Convert.ToDateTime(TDate),
                                       Code = left.Code,
                                       Description = left.Description,
                                       CSODAM = left.CSODAM,
                                       HRBP = left.HRBP,
                                       RRT = left.RRT,
                                       Category = left.Category,
                                       Email = left.Email,
                                       Number = left.Number,
                                       OrgType = left.OrgType,
                                       Psgc = left.Psgc,
                                       Address = left.Address,
                                       BranchSize = left.BranchSize,
                                       Parkingsize = left.Parkingsize,
                                       Sign = left.Sign,
                                       Page = left.Page,
                                       IsBranchActive = left.IsBranchActive,
                                       ServiceBayCount = left.ServiceBayCount,
                                       IsActive = left.IsActive,
                                       CreatedBy = credentials.UserID,
                                       CreatedDate = DateTime.Now
                                   }).ToList();

                updateOrgGroupHistory = oldOrgGroupHistoryUpdate.Select(x => new Data.OrgGroup.OrgGroupHistory()
                {
                    ID = x.ID,
                    ParentOrgID = x.ParentOrgID,
                    IsLatest = true,
                    TDate = x.TDate,
                    Code = x.Code,
                    Description = x.Description,
                    CSODAM = x.CSODAM,
                    HRBP = x.HRBP,
                    RRT = x.RRT,
                    Category = x.Category,
                    Email = x.Email,
                    Number = x.Number,
                    OrgType = x.OrgType,
                    Psgc = x.Psgc,
                    Address = x.Address,
                    BranchSize = x.BranchSize,
                    Parkingsize = x.Parkingsize,
                    Sign = x.Sign,
                    Page = x.Page,
                    IsBranchActive = x.IsBranchActive,
                    ServiceBayCount = x.ServiceBayCount,
                    IsActive = x.IsActive,
                    ModifiedBy = credentials.UserID,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                }).ToList();
            }
            else
            {
                updateOrgGroupHistory = (from left in oldOrgGroupHistory
                                         join right in newOrgGroup on left.Code equals right.Code into joinedList
                                         from sub in joinedList.DefaultIfEmpty()
                                         select new Data.OrgGroup.OrgGroupHistory()
                                         {
                                             ID = left.ID,
                                             ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                             IsLatest = IsLatest,
                                             TDate = left.TDate,
                                             Code = left.Code,
                                             Description = left.Description,
                                             CSODAM = left.CSODAM,
                                             HRBP = left.HRBP,
                                             RRT = left.RRT,
                                             Category = left.Category,
                                             Email = left.Email,
                                             Number = left.Number,
                                             OrgType = left.OrgType,
                                             Psgc = left.Psgc,
                                             Address = left.Address,
                                             BranchSize = left.BranchSize,
                                             Parkingsize = left.Parkingsize,
                                             Sign = left.Sign,
                                             Page = left.Page,
                                             IsBranchActive = left.IsBranchActive,
                                             ServiceBayCount = left.ServiceBayCount,
                                             IsActive = left.IsActive,
                                             ModifiedBy = credentials.UserID,
                                             ModifiedDate = DateTime.Now,
                                             CreatedBy = left.CreatedBy,
                                             CreatedDate = left.CreatedDate
                                         }).ToList();
            }

            /*if (oldOrgGroupHistory.Select(x => x.TDate).FirstOrDefault().Equals(newOrgGroup.Select(x => x.TDate).FirstOrDefault()))
            {

                updateOrgGroup = (from left in oldOrgGroup
                                  join right in newOrgGroup on left.Code equals right.Code into joinedList
                                  from sub in joinedList.DefaultIfEmpty()
                                  select new Data.OrgGroup.OrgGroup()
                                  {
                                      ID = left.ID,
                                      ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                      IsLatest = true,
                                      TDate = Convert.ToDateTime(TDate),
                                      Code = left.Code,
                                      Description = left.Description,
                                      Category = left.Category,
                                      Email = left.Email,
                                      Number = left.Number,
                                      OrgType = left.OrgType,
                                      Psgc = left.Psgc,
                                      Address = left.Address,
                                      BranchSize = left.BranchSize,
                                      Parkingsize = left.Parkingsize,
                                      Sign = left.Sign,
                                      Page = left.Page,
                                      IsBranchActive = left.IsBranchActive,
                                      ServiceBayCount = left.ServiceBayCount,
                                      IsActive = left.IsActive,
                                      CreatedBy = credentials.UserID,
                                      CreatedDate = DateTime.Now,
                                  }).ToList();

                OrgGroupHistory = (from left in oldOrgGroupHistory
                                         join right in newOrgGroup on left.Code equals right.Code into joinedList
                                         from sub in joinedList.DefaultIfEmpty()
                                         select new Data.OrgGroup.OrgGroupHistory()
                                         {
                                             ID = left.ID,
                                             ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                             IsLatest = true,
                                             TDate = Convert.ToDateTime(TDate),
                                             Code = left.Code,
                                             Description = left.Description,
                                             Category = left.Category,
                                             Email = left.Email,
                                             Number = left.Number,
                                             OrgType = left.OrgType,
                                             Psgc = left.Psgc,
                                             Address = left.Address,
                                             BranchSize = left.BranchSize,
                                             Parkingsize = left.Parkingsize,
                                             Sign = left.Sign,
                                             Page = left.Page,
                                             IsBranchActive = left.IsBranchActive,
                                             ServiceBayCount = left.ServiceBayCount,
                                             IsActive = left.IsActive,
                                             ModifiedBy = credentials.UserID,
                                             ModifiedDate = DateTime.Now
                                         }).ToList();
            }
            else 
            {
                updateOrgGroupHistory = oldOrgGroupHistory.Select(x=> new Data.OrgGroup.OrgGroupHistory()
                                         {
                                             ID = x.ID,
                                             ParentOrgID = x.ParentOrgID,
                                             IsLatest = false,
                                             TDate = x.TDate,
                                             Code = x.Code,
                                             Description = x.Description,
                                             Category = x.Category,
                                             Email = x.Email,
                                             Number = x.Number,
                                             OrgType = x.OrgType,
                                             Psgc = x.Psgc,
                                             Address = x.Address,
                                             BranchSize = x.BranchSize,
                                             Parkingsize = x.Parkingsize,
                                             Sign = x.Sign,
                                             Page = x.Page,
                                             IsBranchActive = x.IsBranchActive,
                                             ServiceBayCount = x.ServiceBayCount,
                                             IsActive = x.IsActive,
                                             ModifiedBy = credentials.UserID,
                                             ModifiedDate = DateTime.Now
                                         }).ToList();

                OrgGroupHistory = (from left in oldOrgGroupHistory
                                         join right in newOrgGroup on left.Code equals right.Code into joinedList
                                         from sub in joinedList.DefaultIfEmpty()
                                         select new Data.OrgGroup.OrgGroupHistory()
                                         {
                                             ParentOrgID = sub == null ? left.ParentOrgID : sub.ParentOrgID,
                                             IsLatest = true,
                                             TDate = Convert.ToDateTime(TDate),
                                             Code = left.Code,
                                             Description = left.Description,
                                             Category = left.Category,
                                             Email = left.Email,
                                             Number = left.Number,
                                             OrgType = left.OrgType,
                                             Psgc = left.Psgc,
                                             Address = left.Address,
                                             BranchSize = left.BranchSize,
                                             Parkingsize = left.Parkingsize,
                                             Sign = left.Sign,
                                             Page = left.Page,
                                             IsBranchActive = left.IsBranchActive,
                                             ServiceBayCount = left.ServiceBayCount,
                                             IsActive = left.IsActive,
                                             CreatedBy = credentials.UserID,
                                             CreatedDate = DateTime.Now,
                                         }).ToList();
            }*/
            await _dbAccess.AddOrgGroup(updateOrgGroup);
            _resultView.IsSuccess = await _dbAccess.AddOrgGroupHistory(OrgGroupHistory, IsLatest, updateOrgGroupHistory);

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> GetAllOrgGroupHistory(APICredentials credentials)
        {
            List<Data.OrgGroup.OrgGroupHistory> oldOrgGroupHistory = ((List<Data.OrgGroup.OrgGroupHistory>)await _dbAccess.GetAllHistory()).ToList();
            return new OkObjectResult(oldOrgGroupHistory);
        }
        public async Task<IActionResult> GetEmployeeEmailByOrgId(int OrgGroupID)
        {
            CorporateEmailOutput corporateEmailOutput = new CorporateEmailOutput();
            var OrgCode = (await _dbAccess.GetByID(OrgGroupID));
            var Company = (await _dbAccess.GetTagsByOrgGroupID(OrgGroupID)).Where(y=>y.TagRefCode.Equals("COMPANY_TAG"));

            if(Company.Count() > 0 && OrgCode.OrgType == "BRN")
            {
                corporateEmailOutput.Email = String.Concat((Company.Select(x => x.TagValue).FirstOrDefault()).ToLower(), OrgCode.Code, "@motortrade.com.ph");
            }

            return new OkObjectResult(corporateEmailOutput);
        }
        public async Task<IActionResult> GetOrgGroupFormatByID(List<int> ID)
        {

            return new OkObjectResult(await _dbAccess.GetOrgGroupFormatByID(ID));
        }
        public async Task<IActionResult> GetOrgGroupParent(GetOrgGroupParentInput param)
        {

            return new OkObjectResult(await _dbAccess.GetOrgGroupParent(param));
        }
        public async Task<IActionResult> GetOrgGroupSOMD(List<int> IDs)
        {
            return new OkObjectResult(await _dbAccess.GetOrgGroupSOMD(IDs));
        }
    }
}