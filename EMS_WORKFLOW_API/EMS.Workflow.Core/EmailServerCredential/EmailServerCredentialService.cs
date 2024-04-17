using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.EmailServerCredential;
using EMS.Workflow.Data.LogActivity;
using EMS.Workflow.Transfer.EmailServerCredential;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.EmailServerCredential
{
    public interface IEmailServerCredentialService
    {
        Task<IActionResult> GetByTemplateCode(APICredentials credentials, string TemplateCode);
        Task<IActionResult> GetPendingEmail(APICredentials credentials);
        Task<IActionResult> PostEmailLogs(APICredentials credentials, EmailLogsInput param);
        Task<IActionResult> PostMultipleEmailLogs(APICredentials credentials, List<EmailLogsInput> param);
        Task<IActionResult> PutEmailLogs(APICredentials credentials, int ID);
        Task<IActionResult> PostCronLogs(APICredentials credentials, List<CronLogsInput> param);
    }

    public class EmailServerCredentialService : Core.Shared.Utilities, IEmailServerCredentialService
    {
        private readonly IEmailServerCredentialDBAccess _dbAccess;
        
        public EmailServerCredentialService(WorkflowContext dbContext, IConfiguration iconfiguration,
            IEmailServerCredentialDBAccess dBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
        }

        public async Task<IActionResult> GetByTemplateCode(APICredentials credentials, string TemplateCode)
        {
            Data.EmailServerCredential.EmailServerCredential result = await _dbAccess.GetByTemplateCode(TemplateCode);
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> GetPendingEmail(APICredentials credentials)
        {
            List<EmailLogs> result = (await _dbAccess.GetPendingEmail()).ToList();
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> PostEmailLogs(APICredentials credentials, EmailLogsInput param)
        {
            await _dbAccess.PostEmailLogs(new Data.EmailServerCredential.EmailLogs
            {
                CreatedDate = DateTime.Now,
                CreatedBy = param.CreatedBy,
                SenderName = param.SenderName,
                FromEmailAddress = param.FromEmailAddress,
                ToEmailAddress = param.ToEmailAddress,
                CCEmailAddress = param.CCEmailAddress,
                PositionTitle = param.PositionTitle,
                Name = param.Name,
                Status = param.Status,
                isSent = 0,
                SentStatus = "PENDING",
                SystemCode = param.SystemCode,
                Subject = param.Subject,
                EmailBody = param.Body,
                AttachmentName = param.AttachmentName,
                AttachmentLink = param.AttachmentLink
            });

            _resultView.IsSuccess = true;

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> PostMultipleEmailLogs(APICredentials credentials, List<EmailLogsInput> param)
        {
            await _dbAccess.PostMultipleEmailLogs(param.Select(x => new EmailLogs()
            {
                CreatedDate = DateTime.Now,
                CreatedBy = credentials.UserID.ToString(),
                SenderName = x.SenderName,
                FromEmailAddress = x.FromEmailAddress,
                ToEmailAddress = x.ToEmailAddress,
                CCEmailAddress = x.CCEmailAddress,
                PositionTitle = x.PositionTitle,
                Name = x.Name,
                Status = x.Status,
                isSent = 0,
                SentStatus = "PENDING",
                SystemCode = x.SystemCode,
                Subject = x.Subject,
                EmailBody = x.Body,
                AttachmentName = x.AttachmentName,
                AttachmentLink = x.AttachmentLink
            }).ToList());

            _resultView.IsSuccess = true;

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> PutEmailLogs(APICredentials credentials, int ID)
        {
            Data.EmailServerCredential.EmailLogs emailLogs = await _dbAccess.GetEmailByID(ID);
            emailLogs.isSent = 1;
            emailLogs.SentStatus = "SUCCEED";
            emailLogs.SentDate = DateTime.Now;

            await _dbAccess.PutEmailLogs(emailLogs);
            _resultView.IsSuccess = true;

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }


        public async Task<IActionResult> PostCronLogs(APICredentials credentials, List<CronLogsInput> param)
        {
            await _dbAccess.PostCronLogs(param.Select(x => new CronLogs()
            {
                CronName = x.CronName,
                CronLink = x.CronLink,
                Status = x.Status,
                Remarks = x.Remarks,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = credentials.UserID
            }).ToList());

            _resultView.IsSuccess = true;

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
    }
}