using EMS.IPM.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.DataDuplication.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("org_group")]
    public class OrgGroup : SyncFields
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("parent_org_id")]
        public int ParentOrgID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }
        
        [Column("org_type")]
        public string OrgType { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}