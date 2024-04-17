using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.DataDuplication.Region
{
    [Table("region")]
    public class Region
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("sync_id")]
        public int SyncID { get; set; }

        [Column("sync_date")]
        public DateTime? SyncDate { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}