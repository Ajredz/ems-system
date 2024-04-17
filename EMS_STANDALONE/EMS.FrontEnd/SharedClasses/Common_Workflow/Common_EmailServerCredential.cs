using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.EmailServerCredential;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using NPOI.Util;
using System.Globalization;

namespace EMS.FrontEnd.SharedClasses.Common_Workflow
{

    public class Common_EmailServerCredential : Utilities
    {

        public Common_EmailServerCredential(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<(bool,string)> AddEmailLogs(EmailLogsInput param)
        {
            EmailLogsInput emailLogsInput = new EmailLogsInput()
            {
                SenderName = param.SenderName,
                FromEmailAddress = param.FromEmailAddress,
                ToEmailAddress = param.ToEmailAddress,
                PositionTitle = param.PositionTitle,
                Name = param.Name,
                Status = param.Status,
                SystemCode = param.SystemCode,
                Subject = param.Subject,
                Body = param.Body,
                CreatedBy = _globalCurrentUser.UserID.ToString()
            };

            var emailLogsInputURL = string.Concat(_workflowBaseURL,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmailServerCredential").GetSection("AddEmailLogs").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(emailLogsInput, emailLogsInputURL);
        }

        public async Task<(bool, string)> AddMultipleEmailLogs(List<EmailLogsInput> param)
        {
            var emailLogsInputURL = string.Concat(_workflowBaseURL,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmailServerCredential").GetSection("AddMultipleEmailLogs").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, emailLogsInputURL);
        }
        public async Task<(bool, string)> AddCronLogs(List<CronLogsInput> param)
        {
            var input = string.Concat(_workflowBaseURL,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmailServerCredential").GetSection("AddCronLogs").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, input);
        }

        public async Task<(bool, string)> SendTextMessage(SmsInput param)
        {
            SmsInput sms = new SmsInput();
            sms.phone_number = param.phone_number;
            sms.system = "EMS";
            sms.module = param.module;
            sms.content = param.content;

            var HeaderName = "tokenizer";
            var HeaderValue = SharedUtilities.ComputeSHA256Hash("EMS" + DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).ToLower();
            var URL = "http://192.168.150.16/api-cmc/api/sendSms";
            return await SharedUtilities.PostFromAPIWithHeader(sms, URL, HeaderName, HeaderValue);
        }
    }
}