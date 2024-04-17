using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee")]
    public class TableVarEmployee
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("employment_status")]
        public string EmploymentStatus { get; set; }

        [Column("date_status")]
        public string DateStatus { get; set; }

        [Column("movement_date")]
        public string MovementDate { get; set; }

        [Column("date_hired")]
        public string DateHired { get; set; }
        
        [Column("birth_date")]
        public string BirthDate { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("current_step")]
        public string CurrentStep { get; set; }

        [Column("date_scheduled")]
        public string DateScheduled { get; set; }

        [Column("date_completed")]
        public string DateCompleted { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }

        [Column("home_branch")]
        public string HomeBranch { get; set; }

        [Column("ref_value_company_tag")]
        public string Company { get; set; }

        [Column("cluster")]
        public string Cluster { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("region")]
        public string Region { get; set; }

        [Column("zone")]
        public string Zone { get; set; }

        [Column("percent")]
        public string Percent { get; set; }

    }
}
