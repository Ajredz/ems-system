using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_skills")]
    public class TableVarEmployeeSkills
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("skills_code")]
        public string SkillsCode { get; set; }

        [Column("skills_description")]
        public string SkillsDescription { get; set; }

        [Column("rate")]
        public string Rate { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

    }
}
