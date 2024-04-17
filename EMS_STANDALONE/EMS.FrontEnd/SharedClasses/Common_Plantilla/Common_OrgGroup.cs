using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.Formula.Functions;
using EMS.IPM.Data.DataDuplication.OrgGroup;

namespace EMS.FrontEnd.SharedClasses.Common_Plantilla
{
    public class Common_OrgGroup : Utilities
    {
        public Common_OrgGroup(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }


        public async Task<Form> GetOrgGroup(int ID)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("getbyid").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<List<SelectListItem>> GetOrgGroupDropDown(int id = 0)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetOrgGroupDropDownDetailed()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetDropDownDetailed").Value, "?",
                "userid=", _globalCurrentUser.UserID);
            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<List<OrgGroupPositionForm>> GetOrgGroupPosition(GetByIDInput param)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupPosition").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "ID=", param.ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<OrgGroupPositionForm>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<OrgGroupPositionForm>> GetOrgGroupPositionByID(int ID = 0)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupPosition").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<OrgGroupPositionForm>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<OrgGroupNPRFForm>> GetOrgGroupNPRF(int OrgGroupID)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupNPRF").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "OrgGroupID=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<OrgGroupNPRFForm>(), URL);

            if (IsSuccess)
            {
                // Get Employee Names by User IDs
                List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                   await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                   .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
                   .Union(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy))
                   .Distinct().ToList());

                Result = Result
                        .GroupJoin(systemUsers,
                        x => new { x.CreatedBy },
                        y => new { CreatedBy = y.ID },
                        (x, y) => new { attachment = x, employees = y })
                        .SelectMany(x => x.employees.DefaultIfEmpty(),
                        (x, y) => new { attachment = x, employees = y })
                        .Select(x => new OrgGroupNPRFForm
                        {
                            NPRFNumber = x.attachment.attachment.NPRFNumber,
                            ServerFile = x.attachment.attachment.ServerFile,
                            SourceFile = x.attachment.attachment.SourceFile,
                            Timestamp = x.attachment.attachment.Timestamp,
                            UploadedBy = x.employees == null ? "" : string.Concat(x.employees.LastName,
                                string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
                                string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
                        }).ToList();
            }

            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetOrgGroupByOrgType(string OrgType)
        {

            var URL = string.Concat(_plantillaBaseURL,
           _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetDropdownByOrgType").Value, "?",
            "userid=", _globalCurrentUser.UserID,
            "&OrgType=", OrgType);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetByOrgGroupIDAndPositionIDOutput> GetByOrgGroupAndPosition(GetByOrgGroupIDAndPositionIDInput param)
        {

            var URL = string.Concat(_plantillaBaseURL,
           _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetByOrgGroupAndPosition").Value, "?",
            "userid=", _globalCurrentUser.UserID,
            "&OrgGroupID=", param.OrgGroupID,
            "&PositionID=", param.PositionID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetByOrgGroupIDAndPositionIDOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetOrgGroupHierarchyOutput>> GetOrgGroupHierarchy(int ID)
        {

            var URL = string.Concat(_plantillaBaseURL,
           _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupHierarchy").Value, "?",
            "userid=", _globalCurrentUser.UserID,
            "&ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetOrgGroupHierarchyOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
		
        public async Task<List<GetIDByOrgTypeAutoCompleteOutput>> GetOrgGroupByOrgTypeAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetIDByOrgTypeAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "OrgType=", param.OrgType, "&",
                     "TopResults=", param.TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByOrgTypeAutoCompleteOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetOrgGroupAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<int>> GetOrgGroupDescendants(int OrgGroupID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupDescendants").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "OrgGroupID=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<int>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<int>> GetOrgGroupDescendantsList(List<int> OrgGroupIDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupDescendantsList").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.PostFromAPI(new List<int>(),OrgGroupIDs, URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<PlantillaCountUpdateForm>> GetOrgGroupPositionWithDescription(int OrgGroupID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetPositionWithDescription").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "OrgGroupID=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<PlantillaCountUpdateForm>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

         public async Task<List<SelectListItem>> GetChildrenOrgDropDown(GetByIDInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetChildrenOrgDropDown").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", param.ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<Form>, bool, string)> GetOrgGroupByIDs(List<int> IDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetByIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<Form>(), IDs, URL);
        }

        public async Task<List<SelectListItem>> GetOrgGroupCodeDropDown(int id = 0)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetCodeDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetOrgGroupRollupPositionDropdownOutput>> GetOrgGroupRollupPositionDropdown(int OrgGroupID = 0)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupRollupPositionDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&OrgGroupID=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetOrgGroupRollupPositionDropdownOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<GetRegionByOrgGroupIDOutput> GetRegionByOrgGroupID(int OrgGroupID = 0)
        {

            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetRegionByOrgGroupID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&OrgGroupID=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetRegionByOrgGroupIDOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<Form>> GetByCodes(string CodesDelimited)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetByCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "CodesDelimited=", CodesDelimited);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<Form>(), URL);
			if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
		
        public async Task<List<GetPositionOrgGroupUpwardAutoCompleteOutput>> GetPositionUpwardAutoComplete(GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetPositionUpwardAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "OrgGroupID=", param.OrgGroupID, "&",
                     "TopResults=", param.TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetPositionOrgGroupUpwardAutoCompleteOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<List<GetPositionOrgGroupUpwardAutoCompleteOutput>> GetOrgGroupUpwardAutoComplete(GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupUpwardAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "OrgGroupID=", param.OrgGroupID, "&",
                     "TopResults=", param.TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetPositionOrgGroupUpwardAutoCompleteOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<CorporateEmailOutput> GetEmployeeEmailByOrgID(int OrgGroupID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetEmployeeEmailByOrgID").Value, "?",
                     "OrgGroupID=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new CorporateEmailOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<OrgGroupFormatOutput>, bool, string)> GetOrgGroupFormatByID(List<int> ID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupFormatByID").Value);

            return await SharedUtilities.PostFromAPI(new List<OrgGroupFormatOutput>(), ID, URL);
        }
        public async Task<(List<OrgGroupFormatOutput>, bool, string)> GetOrgGroupParent(GetOrgGroupParentInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupParent").Value);

            return await SharedUtilities.PostFromAPI(new List<OrgGroupFormatOutput>(), param, URL);
        }
        public async Task<(List<OrgGroupSOMDOutput>, bool, string)> GetOrgGroupSOMD(List<int> IDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupSOMD").Value);

            return await SharedUtilities.PostFromAPI(new List<OrgGroupSOMDOutput>(), IDs, URL);
        }
    }
}
