using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EMS.Plantilla.Data.EmployeeMovement
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_movement_get_print")]
    public class TableVarEmployeeMovementGetPrint
    {

        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("car_number")]
        public string CarNumber { get; set; }
        [Column("id_number")]
        public string IDNumber { get; set; }
        [Column("new_id_number")]
        public string NewIDNumber { get; set; }
        [Column("company_name")]
        public string CompanyName { get; set; }
        [Column("company_code")]
        public string CompanyCode { get; set; }
        [Column("company_president")]
        public string CompanyPresident { get; set; }
        [Column("hrd_manager")]
        public string HRDManager { get; set; }
        [Column("regional_manager")]
        public string RegionalManager { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("date_hired")]
        public string DateHired { get; set; }
        [Column("org_group")]
        public string OrgGroup { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("employment_status")]
        public string EmploymentStatus { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("hrd_comments")]
        public string HRDComments { get; set; }
        [Column("date_effective_from")]
        public string DateEffectiveFrom { get; set; }
        [Column("date_effective_to")]
        public string DateEffectiveTo { get; set; }
        [Column("date_effective")]
        public string DateEffective { get; set; }

        [Column("movement_type")]
        public string MovementType { get; set; }
        [Column("details_label")]
        public string DetailsLabel { get; set; }
        [Column("from")]
        public string From { get; set; }
        [Column("to")]
        public string To { get; set; }
        [Column("date_generated")]
        public string DateGenerated { get; set; }
        [Column("details")]
        public string Details { get; set; }

        [Column("special_cases")]
        public string SpecialCases { get; set; }

    }
}
