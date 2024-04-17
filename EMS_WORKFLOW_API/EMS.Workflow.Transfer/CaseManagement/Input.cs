using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utilities.API;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;

namespace EMS.Workflow.Transfer.CaseManagement
{
    #region Case 
    public class CaseForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Status { get; set; }
        public int StatusUpdateBy { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string StatusRemarks { get; set; }
        public bool isActive { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }

    public class CaseDetailsForm
    {
        public int CaseID { get; set; }
        public int CaseType { get; set; }
        public string Where { get; set; }
        public string When { get; set; }
        public int Article { get; set; }
        public int CatCode { get; set; }
        public string Reason { get; set; }
        public List<AttachmentForm> AddAttachmentForm { get; set; }
        public List<AttachmentForm> DeleteAttachmentForm { get; set; }
        public string Details { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }

    public class AttachmentForm
    {
        public bool InReport { get; set; }
        public bool InNte { get; set; }
        public int InAtd { get; set; }
        [IgnoreDataMember]
        public IFormFile File { get; set; }
        public string SourceFile { get; set; }
        public string ServerFile { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }

    public class CaseMinorAuditCommentsForm
    {
        public int CaseID { get; set; }
        public string Comments { get; set; }
        public int InNte { get; set; }
        public int InNoa { get; set; }
        public int CreatedBy { get; set; }
    }

    #endregion
}
