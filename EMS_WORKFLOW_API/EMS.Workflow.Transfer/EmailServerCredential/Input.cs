using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Workflow.Transfer.EmailServerCredential
{
    public class EmailLogsInput
    {
        public int ID { get; set; }
        public string CreatedBy { get; set; }
        public string SenderName { get; set; }
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string CCEmailAddress { get; set; }
        public string PositionTitle { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string SystemCode { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentLink { get; set; }
    }

    public class CronLogsInput
    { 
        public string CronName { get; set; }
        public string CronLink { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class SmsInput
    {
        public string phone_number { get; set; }
        public string system { get; set; }
        public string module { get; set; }
        public string content { get; set; }
    }
}
