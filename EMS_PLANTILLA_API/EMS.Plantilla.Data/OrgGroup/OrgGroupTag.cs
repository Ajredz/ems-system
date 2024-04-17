using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("org_group_tag")]
    public class OrgGroupTag
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("tag_ref_code")]
        public string TagRefCode { get; set; }

        [Column("tag_value")]
        public string TagValue { get; set; }

    }
}