using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.EmployeeMovement
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_movement")]
    // For movement checker, 25-27
    public class TableVarEmployeeMovement
    {

        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("employee_name")]
        public string EmployeeName { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }
        
        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("employee_field")]
        public string EmployeeField { get; set; }

        [Column("movement_type")]
        public string MovementType { get; set; }

        [Column("from")]
        public string From { get; set; }

        [Column("to")]
        public string To { get; set; }

        [Column("date_effective_from")]
        public string DateEffectiveFrom { get; set; }

        [Column("date_effective_to")]
        public string DateEffectiveTo { get; set; }
        
        [Column("created_date")]
        public string CreatedDate { get; set; }

        [Column("created_by")]
        public int  CreatedBy { get; set; }

        [Column("created_by_name")]
        public string CreatedByName { get; set; }

        [Column("reason")]
        public string Reason { get; set; }
        
        [Column("hrd_comments")]
        public string HRDComments { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("new_employee_id")]
        public string NewEmployeeID { get; set; }

        [Column("dept_section")]
        public string DeptSection { get; set; }

        [Column("date_hired")]
        public string DateHired { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        [Column("employment_status")]
        public string EmploymentStatus { get; set; }

        [Column("region")]
        public string Region { get; set; }
    }
}
