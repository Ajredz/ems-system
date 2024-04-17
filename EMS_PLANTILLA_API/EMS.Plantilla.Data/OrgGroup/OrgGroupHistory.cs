using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("org_group_history")]
    public class OrgGroupHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("number")]
        public string Number { get; set; }

        [Column("org_type")]
        public string OrgType { get; set; }

        [Column("psgc")]
        public string Psgc { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("branch_size")]
        public string BranchSize { get; set; }

        [Column("parking_size")]
        public string Parkingsize { get; set; }

        [Column("sign")]
        public string Sign { get; set; }

        [Column("Page")]
        public string Page { get; set; }

        [Column("is_branch_active")]
        public bool IsBranchActive { get; set; }

        [Column("parent_org_id")]
        public int ParentOrgID { get; set; }

        [Column("service_bay_count")]
        public int ServiceBayCount { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("tdate")]
        public DateTime? TDate { get; set; }

        [Column("is_latest")]
        public bool IsLatest { get; set; }

        [Column("cso_am")]
        public int CSODAM { get; set; }

        [Column("hrbp")]
        public int HRBP { get; set; }

        [Column("rrt")]
        public int RRT { get; set; }

    }
}