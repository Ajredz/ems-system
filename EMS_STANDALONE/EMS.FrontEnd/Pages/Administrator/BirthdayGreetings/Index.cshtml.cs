using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;

namespace EMS.FrontEnd.Pages.Administrator.BirthdayGreetings
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.EmailForm EmailForm { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync()
        {
            EmailForm = new EmailForm();

            var EmailSubject = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_SUBJECT")).FirstOrDefault();
            var EmailBody = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_BODY")).FirstOrDefault();

            EmailForm.subject = EmailSubject.Description;
            EmailForm.body = EmailBody.Description;
        }

        public async Task<JsonResult> OnPostAsync() 
        {
            List<Utilities.API.ReferenceMaintenance.ReferenceValue> ReferenceValue = new List<Utilities.API.ReferenceMaintenance.ReferenceValue>();

            var OldValue = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => !x.Value.Equals("BIRTHDAY_SUBJECT") || !x.Value.Equals("BIRTHDAY_BODY")).ToList();

            var EmailSubject = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_SUBJECT")).FirstOrDefault();
            var EmailBody = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_BODY")).FirstOrDefault();

            EmailSubject.Description = EmailForm.subject;
            EmailBody.Description = EmailForm.body;

            ReferenceValue.Add(EmailSubject);
            ReferenceValue.Add(EmailBody);
            ReferenceValue.AddRange(OldValue);

            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("Reference").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(ReferenceValue, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSendEmailBirthday() 
        {
            var EmailSubject = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_SUBJECT")).Select(y => y.Description).FirstOrDefault();
            var EmailBody = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_BODY")).Select(y => y.Description).FirstOrDefault();

            var (Result, IsSuccess, Message) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetEmployeeByBirthday();
            Result = Result.Where(x => !string.IsNullOrEmpty(x.CorporateEmail)).ToList();

            if (IsSuccess)
            {
                var Position = (await new Common_Position(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                        .GetAll());
                var OrgGroup = (await new Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .GetOrgGroupByIDs(Result.Select(x => x.OrgGroupID).ToList())).Item1;

                var emails = (from left in Result
                              join rightPosition in Position on left.PositionID equals rightPosition.ID
                              join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                              select new EmailLogsInput()
                              {
                                  PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                  Status = left.EmploymentStatus,
                                  SystemCode = "EMS PLANTILLA",
                                  SenderName = "MOTORTRADE",
                                  FromEmailAddress = "noreply@motortrade.com.ph",
                                  Name = String.Concat("(", left.Code, ") ", left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)),
                                  ToEmailAddress = left.CorporateEmail,
                                  Subject = EmailSubject
                                         .Replace("@FullName", String.Concat(left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)))
                                         .Replace("@FirstName", left.FirstName)
                                         .Replace("@LastName", left.LastName)
                                         .Replace("@Position", String.Concat(rightPosition.Code, " - ", rightPosition.Title))
                                         .Replace("@OrgGroup", String.Concat(rightOrgGroup.Code, " - ", rightOrgGroup.Description)),
                                  Body = EmailBody
                                         .Replace("@FullName", String.Concat(left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)))
                                         .Replace("@FirstName", left.FirstName)
                                         .Replace("@LastName", left.LastName)
                                         .Replace("@Position", String.Concat(rightPosition.Code, " - ", rightPosition.Title))
                                         .Replace("@OrgGroup", String.Concat(rightOrgGroup.Code, " - ", rightOrgGroup.Description)),
                              }).ToList();

                var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .AddMultipleEmailLogs(emails);
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = MessageUtilities.SCSSMSG_REC_COMPLETE;

            return new JsonResult(_resultView);
        }
    }
}

