using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("mrf_status_history")]
    public class MRFStatusHistory
    {
        [Key]

        [Column("id")]
        public long ID { get; set; }

        [Column("mrf_id")]
        public int MRFID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}