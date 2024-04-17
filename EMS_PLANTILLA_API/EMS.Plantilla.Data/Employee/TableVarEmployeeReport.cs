using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_report")]
    public class TableVarEmployeeReport
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("tdate")]
        public DateTime TDate { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("status_updated_by")]
        public int StatusUpdatedBy { get; set; }

        [Column("status_updated_date")]
        public DateTime? StatusUpdatedDate { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("home_branch_id")]
        public int HomeBranchID { get; set; }

        [Column("region_id")]
        public int? RegionID { get; set; }

        [Column("company_tag")]
        public string CompanyTag { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

    }
}
