using EMS.IPM.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.DataDuplication.PSGCCity
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("psgc_city")]
    public class PSGCCity : SyncFields
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("psgc_region_id")]
        public int PSGCRegionID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}