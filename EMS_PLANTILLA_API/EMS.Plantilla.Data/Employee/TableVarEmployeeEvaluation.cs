using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_evaluation")]
    public class TableVarEmployeeEvaluation
    {

        [Key]

        [Column("code")]
        public string Code { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("fullname")]
        public string FullName { get; set; }

        [Column("date_hired")]
        public string DateHired { get; set; }

        [Column("fivemonths")]
        public string Fivemonths { get; set; }

        [Column("dismisseddate")]
        public string DismissedDate { get; set; }

        [Column("regularization")]
        public string Regularization { get; set; }

        [Column("corporate_email")]
        public string CorporateEmail { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("hrbp")]
        public string HRBP { get; set; }

        [Column("dept")]
        public string DEPT { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("supervisor_name")]
        public string SupervisorName { get; set; }

        [Column("supervisor")]
        public string Supervisor { get; set; }

        [Column("cc_email")]
        public string CCEmail { get; set; }

    }
}
