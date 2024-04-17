using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.DBObjects.NPRF
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("nprf_approver")]
    public class NPRFApprover
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("nprf_id")]
        public int NPRFID { get; set; }

        [Column("nprf_signatories_id")]
        public int NPRFSignatoriesID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

    }


}
