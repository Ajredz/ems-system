using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using Utilities.API.ReferenceMaintenance;
using NPOI.SS.Formula.PTG;
using EMS.Workflow.Transfer.EmailServerCredential;
using MySqlX.XDevAPI.Common;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class SendEmailModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.EmailServerCredential.EmailLogsInput emailLogsInput { get; set; }


        public SendEmailModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID,string Position, string Mrf)
        {
            var ApplicantDetails = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                .GetApplicant(ID);

            var EmailFrom = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CAREER_SENDER_EMAIL.ToString());

            var SenderName = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CAREER_SENDER_NAME.ToString());

            var ContactNumber = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CAREER_CONTACT_NUMBER.ToString());

            var EmailBody = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.BODY_MRF_SEND_EMAIL.ToString());

            ViewData["ID"] = ID;
            ViewData["PositionTitle"] = Position;
            ViewData["ContactNumber"] = ContactNumber.Description;
            ViewData["SenderName"] = SenderName.Description;
            ViewData["EmailFrom"] = EmailFrom.Description;
            ViewData["Name"] = String.Concat(ApplicantDetails.PersonalInformation.FirstName, " ", ApplicantDetails.PersonalInformation.LastName);
            ViewData["NameOnly"] = ApplicantDetails.PersonalInformation.FirstName;
            ViewData["EmailTo"] = ApplicantDetails.PersonalInformation.Email;
            ViewData["EmailSubject"] = Mrf+" | "+Position +" | "+ String.Concat(ApplicantDetails.PersonalInformation.FirstName, " ", ApplicantDetails.PersonalInformation.LastName);
            ViewData["EmailBody"] = EmailBody.Description
                .Replace("&lt;Name&gt;", ApplicantDetails.PersonalInformation.FirstName)
                .Replace("&lt;ContactNumber&gt;",ContactNumber.Description);
        }


        public async Task<JsonResult> OnPostAsync()
        {
            var EmailSenderName = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CAREER_SENDER_NAME.ToString());


            EmailLogsInput emailLogs = new EmailLogsInput()
            {
                PositionTitle = emailLogsInput.PositionTitle,
                SenderName = emailLogsInput.SenderName,
                FromEmailAddress = emailLogsInput.FromEmailAddress,
                Name = emailLogsInput.Name,
                ToEmailAddress = emailLogsInput.ToEmailAddress,
                Subject = emailLogsInput.Subject,
                Body = emailLogsInput.Body,
            };

            var (IsSuccess,Message) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                .AddEmailLogs(emailLogs);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = MessageUtilities.SCSSMSG_EMAIL_INVITE_SENT;

            return new JsonResult(_resultView);
        }
    }
}