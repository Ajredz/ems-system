using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("org_group_nprf")]
    public class OrgGroupNPRF
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("nprf_number")]
        public string NPRFNumber { get; set; }

        [Column("approved_date")]
        public DateTime ApprovedDate { get; set; }

        [Column("source_file")]
        public string SourceFile { get; set; }

        [Column("server_file")]
        public string ServerFile { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

    }
}
