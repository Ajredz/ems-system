using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Workflow.Data.Case
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("case")]
    public class Case
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_updated_by")]
        public int StatusUpdateBy { get; set; }
        [Column("status_updated_date")]
        public DateTime? StatusUpdatedDate { get; set; }
        [Column("status_remarks")]
        public string StatusRemarks { get; set; }
        [Column("is_active")]
        public bool isActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
    [Table("case_details")]
    public class CaseDetails
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("case_id")]
        public int CaseID { get; set; }
        [Column("case_type")]
        public int CaseType { get; set; }
        [Column("where")]
        public  string Where{ get; set; }
        [Column("when")]
        public DateTime? When { get; set; }
        [Column("article")]
        public int Article { get; set; }
        [Column("cat_code")]
        public int CatCode { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("case_type")]
        public string Details { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }

    [Table("case_attachment")]
    public class CaseAttachment
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("case_id")]
        public int CaseID { get; set; }
        [Column("source_file")]
        public string SourceFile { get; set; }
        [Column("server_file")]
        public string ServerFile { get; set; }
        [Column("in_report")]
        public bool InReport { get; set; }
        [Column("in_nte")]
        public bool InNte { get; set; }
        [Column("in_atd")]
        public bool InATD { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
        [Column("modified_by")]
        public int ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }

    [Table("case_comments")]
    public class CaseComments
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("case_id")]
        public int CaseID { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("in_nte")]
        public bool InNTE { get; set; }
        [Column("in_noa")]
        public bool InNoa { get; set; }
        [Column("in_report")]
        public bool InReport { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate  { get; set; }
    }

    [Table("case_noa")]
    public class CaseNoa
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("case_id")]
        public int CaseID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("article")]
        public int Article { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("frequency")]
        public int Frequency { get; set; }
        [Column("action_type")]
        public int ActionType { get; set; }
        [Column("effective_date")]
        public string EffectiveDate { get; set; }
        [Column("hrd_comments")]
        public string HrdComments { get; set; }
        [Column("is_issued")]
        public bool IsIssued { get; set; }
        [Column("is_employee_seen")]
        public bool IsEmployeeSeen { get; set; }
        [Column("issued_date")]
        public DateTime? IssuedDate { get; set; }
        [Column("employee_seen_date")]
        public DateTime?  EmployeeSeenDate { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
        [Column("modified_by")]
        public int ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }

    [Table("case_nte")]
    public class CaseNTE
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("case_id")]
        public int CaseID { get; set; }
        [Column("frequency")]
        public int Frequency { get; set; }
        [Column("explanation")]
        public string Explanation { get; set; }
        [Column("is_issued")]
        public bool IsIssued { get; set; }
        [Column("is_employee_seen")]
        public bool IsEmployeeSeen { get; set; }
        [Column("issued_date")]
        public DateTime? IssuedDate { get; set; }
        [Column("employee_seen_date")]
        public DateTime? EmployeeSeenDate { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
        [Column("modified_by")]
        public int ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }

    [Table("case_status_history")]
    public class CaseStatusHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("case_id")]
        public int CaseID { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public int CreatedDate { get; set; }
    }

}
