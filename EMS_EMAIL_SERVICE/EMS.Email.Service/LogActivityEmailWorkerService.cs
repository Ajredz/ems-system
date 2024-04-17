using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utilities.API;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Authentication;
using EMS.Workflow.Data.EmailServerCredential;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml.Linq;

namespace EMS.Sync.Security.Worker
{
    public class LogActivityEmailWorkerService : BackgroundService
    {
        private readonly ILogger<LogActivityEmailWorkerService> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly int _IntervalInSeconds = 0;
        private readonly string _getEmailServerCredentialURL = "";
        private readonly string _getApplicantLogActivityURL = "";
        private readonly string _updateApplicantLogActivityURL = "";
        private readonly string _getEmployeeLogActivityURL = "";
        private readonly string _updateEmployeeLogActivityURL = "";
        private static EmailServerCredential _LogActivity;

        private readonly string _getApplicantPendingEmailURL = "";
        private readonly string _updateApplicantPendingEmailURL = "";

        public LogActivityEmailWorkerService(ILogger<LogActivityEmailWorkerService> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;

            _IntervalInSeconds = Convert.ToInt32(_iconfiguration.GetSection("LogActivity_IntervalInSeconds").Value);

            _getEmailServerCredentialURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("GetByTemplateCode").Value);
            _getApplicantLogActivityURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("GetApplicantLogActivityPendingEmail").Value);
            _updateApplicantLogActivityURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("UpdateApplicantLogActivityPendingEmail").Value);
            _getEmployeeLogActivityURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("GetEmployeeLogActivityPendingEmail").Value);
            _updateEmployeeLogActivityURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("UpdateEmployeeLogActivityPendingEmail").Value);

            _getApplicantPendingEmailURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("GetPendingEmail").Value);
            _updateApplicantPendingEmailURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
              _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("UpdatePendingEmail").Value);

            var (APIResult, IsSuccess, ErrorMessage) =
                SharedUtilities.GetFromAPI(new EmailServerCredential(), string.Concat(_getEmailServerCredentialURL, "?TemplateCode=LOG_ACTIVITY_NOTIFICATION")).Result;

            if (IsSuccess)
                _LogActivity = APIResult;
            else
                throw new Exception(ErrorMessage);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var IsSuccess = false;
                IsSuccess = await DoWork();
                if(IsSuccess)
                    await Task.Delay(_IntervalInSeconds * 1000, stoppingToken);
                else
                    await Task.Delay(600 /*10 mins*/ * 1000, stoppingToken);
            }
        }

        private async Task<bool> DoWork()
        {
            var IsSuccess = true;
            try
            {
                /*var ApplicantTuple =
                await SharedUtilities.GetFromAPI(
                    new List<EMS.Workflow.Data.LogActivity.ApplicantLogActivity>(), _getApplicantLogActivityURL);

                if (ApplicantTuple.IsSuccess)
                {
                    foreach (var item in ApplicantTuple.APIResult)
                    {
                        await SendEmail(
                            item.Title,
                            "Applicant",
                            item.Type,
                            item.ApplicantName,
                            item.Description,
                            item.Email
                            );

                        var (PostIsSuccess, Message) = 
                            await SharedUtilities.PutFromAPI(new object(), 
                            string.Concat(_updateApplicantLogActivityURL, "?ID=" + item.ID));
                    }
                    if(ApplicantTuple.APIResult.Count > 0)    
                    _logger.LogInformation("[LogActivity] SENT IDs: {IDs}",
                            string.Join(",", ApplicantTuple.APIResult.Select(x => x.ID).ToArray()));
                }
                else
                {
                    throw new Exception(ApplicantTuple.ErrorMessage);
                }

                var EmployeeTuple =
                await SharedUtilities.GetFromAPI(
                    new List<EMS.Workflow.Data.LogActivity.EmployeeLogActivity>(), _getEmployeeLogActivityURL);

                if (EmployeeTuple.IsSuccess)
                {
                    foreach (var item in EmployeeTuple.APIResult)
                    {
                        await SendEmail(
                            item.Title,
                            "Employee",
                            item.Type,
                            item.EmployeeName,
                            item.Description,
                            item.Email
                            );

                        var (PostIsSuccess, Message) =
                            await SharedUtilities.PutFromAPI(new object(),
                            string.Concat(_updateEmployeeLogActivityURL, "?ID=" + item.ID));

                    }
                }
                else
                {
                    throw new Exception(EmployeeTuple.ErrorMessage);
                }*/



                var EmailLogs =
                await SharedUtilities.GetFromAPI(
                    new List<EMS.Workflow.Data.EmailServerCredential.EmailLogs>(), _getApplicantPendingEmailURL);

                if (EmailLogs.IsSuccess)
                {
                    foreach (var item in EmailLogs.APIResult)
                    {
                        EmailDetails emailFrom = new EmailDetails()
                        {
                            email = item.FromEmailAddress,
                            name = item.SenderName
                        };

                        List<EmailDetails> emailDetailsTo = new List<EmailDetails>();
                        if (!string.IsNullOrEmpty(item.ToEmailAddress)) 
                        {
                            if ((item.ToEmailAddress.Split(',')).Count() > 1)
                            {
                                foreach (var to in item.ToEmailAddress.Split(','))
                                {
                                    EmailDetails emailTo = new EmailDetails()
                                    {
                                        email = to,
                                        name = ""
                                    };
                                    emailDetailsTo.Add(emailTo);
                                }
                            }
                            else
                            {
                                EmailDetails emailTo = new EmailDetails()
                                {
                                    email = item.ToEmailAddress,
                                    name = ""
                                };
                                emailDetailsTo.Add(emailTo);
                            }
                        }

                        List<EmailDetails> emailDetailsCC = new List<EmailDetails>();
                        if (!string.IsNullOrEmpty(item.CCEmailAddress))
                        {
                            if ((item.CCEmailAddress.Split(',')).Count() > 1)
                            {
                                foreach (var to in item.CCEmailAddress.Split(','))
                                {
                                    EmailDetails emailCC = new EmailDetails()
                                    {
                                        email = to,
                                        name = ""
                                    };
                                    emailDetailsCC.Add(emailCC);
                                }
                            }
                            else
                            {
                                EmailDetails emailCC = new EmailDetails()
                                {
                                    email = item.CCEmailAddress,
                                    name = ""
                                };
                                emailDetailsCC.Add(emailCC);
                            }
                        }

                        List<File> fileAttachment = new List<File>();
                        if (!string.IsNullOrEmpty(item.AttachmentName))
                        {
                            if ((item.AttachmentName.Split(',')).Count() > 1)
                            {
                                var name = item.AttachmentName.Split(',');
                                var link = item.AttachmentLink.Split(',');
                                for (var i = 0; i < name.Count(); i++)
                                {
                                    File file = new File()
                                    {
                                        name = name[i],
                                        path = link[i]
                                    };
                                    fileAttachment.Add(file);
                                }
                            }
                            else
                            {
                                File file = new File()
                                {
                                    name = item.AttachmentName,
                                    path = item.AttachmentLink
                                };
                                fileAttachment.Add(file);
                            }
                        }
                        
                        EmailForm emailForm = new EmailForm() {
                            subject = item.Subject,
                            body = item.EmailBody,
                            template = "mail",
                            to = emailDetailsTo,
                            from = new List<EmailDetails>() { emailFrom },
                            cc = emailDetailsCC,
                            bcc = new List<EmailDetails>() { },
                            file = fileAttachment
                        };
 
                        var HeaderName = "tokenizer";
                        var HeaderValue = SharedUtilities.ComputeSHA256Hash("EMS" + DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).ToLower();
                        var URL = "http://192.168.150.16/api-cmc/api/sendEmail";
                        var (IsSuccessSent, Messages) = await SharedUtilities.PostFromAPIWithHeader(emailForm, URL, HeaderName, HeaderValue);

                        if (IsSuccessSent)
                        {
                            var (PostIsSuccess, Message) =
                            await SharedUtilities.PutFromAPI(new object(),
                            string.Concat(_updateApplicantPendingEmailURL, "?ID=" + item.ID));
                        }
                    }
                }
                else
                {
                    throw new Exception(EmailLogs.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                _logger.LogInformation("[LogActivity] ERROR: {error}", 
                    string.Concat(ex.Message, "" + ex.InnerException, "" + ex.StackTrace));
                //throw;
            }

            return IsSuccess;
        }
/*
        private async Task SendEmail(string Title, string ApplicantOrEmployee, string Type, 
            string Name, string Description, string RecipientEmail)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(_LogActivity.SenderDisplayName, _LogActivity.SenderEmail));
                message.To.Add(new MailboxAddress("", RecipientEmail));
                message.Subject = _LogActivity.Subject.Replace("<<{title}>>", Title);
                message.Body = new BodyBuilder
                {
                    HtmlBody = _LogActivity.Body.Replace("<<{applicant_employee}>>", ApplicantOrEmployee)
                    .Replace("<<{type}>>", Type).Replace("<<{name}>>", Name)
                    .Replace("<<{description}>>", Description)

                }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    if (_LogActivity.EnableSSL)
                    {
                        client.Connect(_LogActivity.Host, _LogActivity.Port, _LogActivity.EnableSSL);
                    }
                    else
                    { 
                        client.Connect(_LogActivity.Host, _LogActivity.Port, SecureSocketOptions.StartTls);
                    }
                    client.Authenticate(_LogActivity.SenderUsername, _LogActivity.SenderPassword);
                    //client.Authenticate(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_LogActivity.SenderUsername))
                    //    , Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_LogActivity.SenderPassword)));
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[LogActivity-SendEmail] ERROR: {error}",
                    string.Concat(ex.Message, "" + ex.InnerException, "" + ex.StackTrace));
                throw;
            }
        }*/



        private async Task SendEmailFromEmailLogs(string SenderName,string FromEmail,string ToEmail, string Name, string Subject,
            string Body)
        {
            try
            {
                if (String.IsNullOrEmpty(FromEmail)) {
                    FromEmail = _LogActivity.SenderEmail;
                }
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(SenderName, FromEmail));
                message.To.Add(new MailboxAddress(Name, ToEmail));
                message.Subject = Subject;
                message.Body = new BodyBuilder
                {
                    HtmlBody = Body

                }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    if (_LogActivity.EnableSSL) 
                    {
                        client.Connect(_LogActivity.Host, _LogActivity.Port, _LogActivity.EnableSSL);
                    }
                    else
                    {
                        client.Connect(_LogActivity.Host, _LogActivity.Port, SecureSocketOptions.SslOnConnect);
                    }
                    client.Authenticate(_LogActivity.SenderUsername, _LogActivity.SenderPassword);
                    //client.Authenticate(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_LogActivity.SenderUsername))
                    //    , Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_LogActivity.SenderPassword)));
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[LogActivity-SendEmail] ERROR: {error}",
                    string.Concat(ex.Message, "" + ex.InnerException, "" + ex.StackTrace));
                throw;
            }
        }
    }
}
