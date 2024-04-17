using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_PSGC;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.Applicant.Form Applicant { get; set; }
        

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                Applicant = new EMS.Recruitment.Transfer.Applicant.Form();

                ViewData["OrgGroupSelectList"] = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
                ViewData["PositionSelectList"] = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown();
                //ViewData["WorkflowSelectList"] = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflowDropDown();

                var applicationSource = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.APPLICATION_SOURCE.ToString());
                ViewData["ApplicationSourceSelectList"] = applicationSource.Select(x =>  new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var course = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.COURSE.ToString());
                ViewData["CourseSelectList"] = course.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                ViewData["PSGCRegionSelectList"] = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetRegionDropdown();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Applicant.CreatedBy = _globalCurrentUser.UserID;
            Applicant.Attachments = Applicant.Attachments.Select(x =>
                {
                    string dateTimePreFix = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
                    x.ServerFile = string.Concat(dateTimePreFix, Guid.NewGuid().ToString("N").Substring(0, 4), "_", x.File.FileName);
                    x.SourceFile = x.File.FileName;
                    return x;
                }).ToList();


            var URL = string.Concat(_recruitmentBaseURL,
                    _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("Add").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            //if (string.IsNullOrEmpty(Applicant.CurrentStepCode))
            //{
            //    EMS.Workflow.Transfer.Workflow.Form Workflow =
            //            await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(Applicant.WorkflowID);

            //    Applicant.CurrentStepCode = Workflow.WorkflowStepList.First().StepCode;
            //    Applicant.CurrentStepDescription = Workflow.WorkflowStepList.First().StepDescription;
            //    Applicant.CurrentStepApproverRoleIDs = 
            //        string.Join(",", Workflow.WorkflowStepList.First().WorkflowStepApproverList.Select(x => x.RoleID).ToList()); 
            //}


            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Applicant, URL);

            if (IsSuccess)
            {
                foreach (var item in Applicant.Attachments) 
                { 
                   await CopyToServerPath(Path.Combine(_env.WebRootPath, 
                       _iconfiguration.GetSection("RecruitmentService_Attachment_Path").Value), item.File, item.ServerFile);                
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "Applicant",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Applicant.PersonalInformation.FirstName, " ", Applicant.PersonalInformation.MiddleName, " ", Applicant.PersonalInformation.LastName,  " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (Applicant.Attachments != null && Applicant.Attachments.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "ApplicantAttachment",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Applicant.PersonalInformation.FirstName, " ", Applicant.PersonalInformation.MiddleName, " ", Applicant.PersonalInformation.LastName, " Attachment added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}