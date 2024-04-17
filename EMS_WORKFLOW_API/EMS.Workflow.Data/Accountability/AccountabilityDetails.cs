using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("accountability_details")]
    public class AccountabilityDetails
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("accountability_id")]
        public int AccountabilityID { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("org_group_id")]
        public int? OrgGroupID { get; set; }

        [Column("position_id")]
        public int? PositionID { get; set; }

        [Column("employee_id")]
        public int? EmployeeID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
