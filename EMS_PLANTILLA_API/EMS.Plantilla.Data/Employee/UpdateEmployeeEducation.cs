using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("update_employee_education")]
    public class UpdateEmployeeEducation

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("udpdate_status")]
        public string UpdateStatus { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }
        
        [Column("school")]
        public string School { get; set; }
        
        [Column("school_address")]
        public string SchoolAddress { get; set; }
        
        [Column("school_level_code")]
        public string SchoolLevelCode { get; set; }
        
        [Column("course")]
        public string Course { get; set; }
        
        [Column("year_from")]
        public int YearFrom { get; set; }
        
        [Column("year_to")]
        public int YearTo { get; set; }
        
        [Column("educational_attainment_degree_code")]
        public string EducationalAttainmentDegreeCode { get; set; }
        
        [Column("educational_attainment_status_code")]
        public string EducationalAttainmentStatusCode { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}