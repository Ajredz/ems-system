using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.DBObjects.NPRFSignatories
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("NPRF_signatories")]
    public class NPRFSignatories
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("approver_id")]
        public int ApproverId { get; set; }

        [Column("approver_description")]
        public string ApproverDescription { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("company_id")]
        public int CompanyID { get; set; }

    }
}
