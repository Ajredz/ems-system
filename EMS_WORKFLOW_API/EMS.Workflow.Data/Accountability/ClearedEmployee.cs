using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("cleared_employee")]
    public class ClearedEmployee
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("accountability")]
        public string Accountability { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_updated_by")]
        public int StatusUpdatedBy { get; set; }
        [Column("status_updated_date")]
        public DateTime? StatusUpdatedDate { get; set; }
        [Column("status_remarks")]
        public string StatusRemarks { get; set; }
        [Column("computation")]
        public string Computation { get; set; }
        [Column("agreed")]
        public bool Agreed { get; set; }
        [Column("agreed_date")]
        public DateTime? AgreedDate { get; set; }
        [Column("last_comment")]
        public string LastComment { get; set; }
        [Column("last_comment_date")]
        public DateTime? LastCommentDate { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }

    [Table("cleared_employee_attachment")]
    public class ClearedEmployeeAttachment
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("cleared_employee_id")]
        public int ClearedEmployeeID { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("path")]
        public string Path { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }

    [Table("cleared_employee_comments")]
    public class ClearedEmployeeComments
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("cleared_employee_id")]
        public int ClearedEmployeeID { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }

    [Table("cleared_employee_status_history")]
    public class ClearedEmployeeStatusHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("cleared_employee_id")]
        public int ClearedEmployeeID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }

    public class tvAddClearedEmployee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("accountability")]
        public string Accountability { get; set; }
        [Column("is_cleared")]
        public bool IsCleared { get; set; }
    }

    public class tvClearedEmployeeList
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("fullname")]
        public string FullName { get; set; }
        [Column("org_group")]
        public string OrgGroup { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("accountability")]
        public string Accountability { get; set; }
        [Column("status_code")]
        public string StatusCode { get; set; }
        [Column("status_description")]
        public string StatusDescription { get; set; }
        [Column("status_updated_by")]
        public string StatusUpdatedBy { get; set; }
        [Column("status_updated_date")]
        public string StatusUpdatedDate { get; set; }
        [Column("status_remarks")]
        public string StatusRemarks { get; set; }
        [Column("Computation")]
        public string Computation { get; set; }
        [Column("agreed")]
        public string Agreed { get; set; }
        [Column("agreed_date")]
        public string AgreedDate { get; set; }
        [Column("last_comment")]
        public string LastComment { get; set; }
        [Column("last_comment_date")]
        public string LastCommentDate { get; set; }
        [Column("status_color")]
        public string StatusColor { get; set; }
        [Column("total_num")]
        public int TotalNoOfRecord { get; set; }
        [Column("num_of_pages")]
        public int NoOfPages { get; set; }
    }
    public class tvClearedEmployeeByID
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("fullname")]
        public string FullName { get; set; }
        [Column("org_group")]
        public string OrgGroup { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("accountability")]
        public string Accountability { get; set; }
        [Column("status_code")]
        public string StatusCode { get; set; }
        [Column("status_description")]
        public string StatusDescription { get; set; }
        [Column("status_updated_by")]
        public string StatusUpdatedBy { get; set; }
        [Column("status_updated_date")]
        public string StatusUpdatedDate { get; set; }
        [Column("computation")]
        public string Computation { get; set; }
        [Column("agreed")]
        public string Agreed { get; set; }
        [Column("agreed_date")]
        public string AgreedDate { get; set; }
        [Column("date_hired")]
        public string DateHired { get; set; }
    }
    public class tvClearedEmployeeComments
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("created_date")]
        public string CreatedDate { get; set; }
    }
    public class tvClearedEmployeeStatusHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("updated_date")]
        public string UpdatedDate { get; set; }
        [Column("updated_by")]
        public string UpdatedBy { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
    }
    public class tvEmployeeAccountability
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("org_group")]
        public string OrgGroup { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_updated_date")]
        public string StatusUpdatedDate { get; set; }
        [Column("status_updated_by")]
        public string StatusUpdatedBy { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
    }
}
