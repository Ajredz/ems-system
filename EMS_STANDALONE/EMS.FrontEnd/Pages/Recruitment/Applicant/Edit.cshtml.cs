using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;
using EMS.FrontEnd.SharedClasses.Common_PSGC;

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.Applicant.Form Applicant { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.UpdateApplicantLogActivityAssignedUserForm AssignedUser { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_globalCurrentUser != null)
            {
                if (_systemURL != null)
                {
                    ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/APPLICANT/DELETE")).Count() > 0 ? "true" : "false";
                }

                Applicant = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ID);
                
                //Applicant.ApplicationHistory = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                //.GetTransactionByRecordID(new GetTransactionByRecordIDInput { WorkflowID = Applicant.WorkflowID, RecordID = ID }))
                //.History.Select(x => new EMS.Recruitment.Transfer.Applicant.ApplicationHistory { 
                //    OrderNo = x.Order,
                //    Step = x.Step,
                //    Result = x.Status,
                //    Remarks = x.Remarks,
                //    Timestamp = x.Timestamp
                //}).ToList();


                if (Applicant.ReferredByUserID > 0)
                {
                    Applicant.ReferredByUserIDDescription =
                      (await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserBySyncID(Applicant.ReferredByUserID)).Description;
                }

                var uploadedBy = Applicant.Attachments.Select(x => x.CreatedBy).FirstOrDefault();
                if (uploadedBy > 0)
                {
                    ViewData["AttachmentUploadedBy"] = (await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserBySyncID(uploadedBy)).Description;
                }

                //ViewData["OrgGroupSelectList"] = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown(Applicant.OrgGroupID);
                //ViewData["PositionSelectList"] = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(Applicant.PositionID);
                //ViewData["WorkflowSelectList"] = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflowDropDown(Applicant.WorkflowID);
                var attachmentList = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env).GetReferenceValueByRefCode(ReferenceCodes_Recruitment.ATTACHMENT_TYPE.ToString());
                ViewData["AttachmentTypeSelectList"] =
                    attachmentList.Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                //var region = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                //.GetReferenceValueByRefCode("GEOGRAPHICAL_REGION");
                //ViewData["GeographicalRegionSelectList"] = region.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCRegionCode))
                {
                    var regionList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetRegionDropdown(Applicant.PersonalInformation.PSGCRegionCode);

                    ViewData["PSGCRegionSelectList"] = regionList.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text,
                        Selected = x.Value == Applicant.PersonalInformation.PSGCRegionCode
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCProvinceCode))
                {
                    var provinceList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetProvinceDropdownByRegion(Applicant.PersonalInformation.PSGCRegionCode);

                    ViewData["PSGCProvinceSelectList"] = provinceList.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text,
                        Selected = x.Value == Applicant.PersonalInformation.PSGCProvinceCode
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCCityMunicipalityCode))
                {
                    var cityMunicipalityList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetCityMunicipalityDropdownByProvince(Applicant.PersonalInformation.PSGCProvinceCode);

                    ViewData["PSGCCityMunicipalitySelectList"] = cityMunicipalityList.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text,
                        Selected = x.Value == Applicant.PersonalInformation.PSGCCityMunicipalityCode
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCBarangayCode))
                {
                    var barangayList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetBarangayDropdownByCityMunicipality(Applicant.PersonalInformation.PSGCCityMunicipalityCode);

                    ViewData["PSGCBarangaySelectList"] = barangayList.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text,
                        Selected = x.Value == Applicant.PersonalInformation.PSGCBarangayCode
                    }).ToList();
                }

                var applicationSource = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.APPLICATION_SOURCE.ToString());
                ViewData["ApplicationSourceSelectList"] = applicationSource.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var course = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.COURSE.ToString());
                ViewData["CourseSelectList"] = course.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Applicant.CreatedBy = _globalCurrentUser.UserID;
            Applicant.Attachments = Applicant.Attachments.Select(x =>
            {
                if (x.File != null)
                { 
                    string dateTimePreFix = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
                    x.ServerFile = string.Concat(dateTimePreFix, Guid.NewGuid().ToString("N").Substring(0, 4), "_", x.File.FileName);
                    x.SourceFile = x.File.FileName;
                } 
                return x;
            }).ToList();

            //if (string.IsNullOrEmpty(Applicant.CurrentStepCode))
            //{
            //    EMS.Workflow.Transfer.Workflow.Form Workflow =
            //            await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(Applicant.WorkflowID);

            //    Applicant.CurrentStepCode = Workflow.WorkflowStepList.First().StepCode;
            //    Applicant.CurrentStepDescription = Workflow.WorkflowStepList.First().StepDescription;
            //    Applicant.CurrentStepApproverRoleIDs =
            //        string.Join(",", Workflow.WorkflowStepList.First().WorkflowStepApproverList.Select(x => x.RoleID).ToList());
            //}

            var URL = string.Concat(_recruitmentBaseURL,
                _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Applicant, URL);

            if (IsSuccess)
            {
                foreach (var item in Applicant.Attachments)
                {
                    if (item.File != null)
                    {
                        await CopyToServerPath(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("RecruitmentService_Attachment_Path").Value), item.File, item.ServerFile); 
                    }
                }

                if (Applicant.DeletedAttachments?.Count > 0)
                {
                    foreach (var item in Applicant.DeletedAttachments)
                    {
                        DeleteFileFromServer(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("RecruitmentService_Attachment_Path").Value), item.ServerFile);

                    }
                }
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostUpdateAssignedUser()
        {
            AssignedUser.ModifiedBy = _globalCurrentUser.UserID;

            if (AssignedUser.IsAssignToSelf)
            {
                AssignedUser.AssignedUserID = _globalCurrentUser.UserID;
            }

            var URL = string.Concat(_workflowBaseURL,
                 _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("UpdateApplicantLogActivityAssignedUser").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(AssignedUser, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}