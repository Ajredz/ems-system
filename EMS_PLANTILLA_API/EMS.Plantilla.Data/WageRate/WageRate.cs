using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.WageRate
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("wage_rate")]
    public class WageRate
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("ref_val_region_tag")]
        public string RefValRegionTag { get; set; }
        
        [Column("monthly_salary")]
        public decimal MonthlySalary { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; }
        
        [Column("created_by")]
        public int Createdby { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        
        [Column("modified_by")]
        public int ModifiedBy { get; set; }
        
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}