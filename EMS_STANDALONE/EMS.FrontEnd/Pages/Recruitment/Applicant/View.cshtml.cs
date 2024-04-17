using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Plantilla.Transfer.PositionLevel;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_PSGC;

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.Applicant.Form Applicant { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_globalCurrentUser != null)
            {
                if (_systemURL != null)
                {
                    ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/APPLICANT/DELETE")).Count() > 0 ? "true" : "false";
                    ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/APPLICANT/EDIT")).Count() > 0 ? "true" : "false";
                }
                
                Applicant = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ID);

                //Applicant.ApplicationHistory = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                //.GetTransactionByRecordID(new GetTransactionByRecordIDInput { WorkflowID = Applicant.WorkflowID, RecordID = ID }))
                //.History.Select(x => new EMS.Recruitment.Transfer.Applicant.ApplicationHistory
                //{
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
                    attachmentList.Select(x => new SelectListItem { 
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                //var region = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                //.GetReferenceValueByRefCode("GEOGRAPHICAL_REGION");
                //ViewData["GeographicalRegionSelectList"] = region.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCRegionCode))
                {
                    var region = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetRegionValueByCode(Applicant.PersonalInformation.PSGCRegionCode);
                    ViewData["PSGCRegionSelectList"] =
                        new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = region.Code,
                            Text = region.Description,
                            Selected = true
                        }
                        };
                }

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCProvinceCode))
                {
                    var province = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetProvinceValueByCode(Applicant.PersonalInformation.PSGCProvinceCode);
                    ViewData["PSGCProvinceSelectList"] =
                        new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = province.Code,
                            Text = province.Description,
                            Selected = true
                        }
                        };
                }

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCCityMunicipalityCode))
                {
                    var cityMun = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetCityMunicipalityValueByCode(Applicant.PersonalInformation.PSGCCityMunicipalityCode);
                    ViewData["PSGCCityMunicipalitySelectList"] =
                        new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = cityMun.Code,
                            Text = cityMun.Description,
                            Selected = true
                        }
                        };
                }

                if (!string.IsNullOrEmpty(Applicant.PersonalInformation.PSGCBarangayCode))
                {
                    var barangay = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                    .GetBarangayValueByCode(Applicant.PersonalInformation.PSGCBarangayCode);
                    ViewData["PSGCBarangaySelectList"] =
                        new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = barangay.Code,
                            Text = barangay.Description,
                            Selected = true
                        }
                        };
                }

                var applicationSource = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.APPLICATION_SOURCE.ToString());
                ViewData["ApplicationSourceSelectList"] = applicationSource.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var course = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
               .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.COURSE.ToString());
                ViewData["CourseSelectList"] = course.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();
            }
        }

        public async Task<JsonResult> OnGetPositionLevelDropDownByOrgGroupID([FromQuery] GetByPositionLevelIDInput param)
        {
            var result = await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdownByOrgGroupID(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDown(int PositionLevelID)
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetDropdownDetailedByPositionLevel(PositionLevelID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRegionByID(int RegionID)
        {
            _resultView.Result = await new Common_Region(_iconfiguration, _globalCurrentUser, _env).GetRegion(RegionID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}