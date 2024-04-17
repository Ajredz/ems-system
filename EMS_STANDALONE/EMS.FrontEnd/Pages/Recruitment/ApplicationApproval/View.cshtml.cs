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

namespace EMS.FrontEnd.Pages.Recruitment.ApplicationApproval
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

                //ViewData["OrgGroupSelectList"] = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown(Applicant.OrgGroupID);
                //ViewData["PositionSelectList"] = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(Applicant.PositionID);
                //ViewData["WorkflowSelectList"] = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflowDropDown(Applicant.WorkflowID);
                var attachmentList = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env).GetReferenceValueByRefCode("ATTACHMENT_TYPE");
                ViewData["AttachmentTypeSelectList"] =
                    attachmentList.Select(x => new SelectListItem { 
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                var region = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode("GEOGRAPHICAL_REGION");
                ViewData["GeographicalRegionSelectList"] = region.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var applicationSource = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode("APPLICATION_SOURCE");
                ViewData["ApplicationSourceSelectList"] = applicationSource.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();
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