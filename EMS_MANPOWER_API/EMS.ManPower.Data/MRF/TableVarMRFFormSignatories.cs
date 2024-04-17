using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_form_signatories")]
    public class TableVarMRFFormSignatories
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("approver_description")]
        public string ApproverDescription { get; set; }

        [Column("approver_role_id")]
        public int ApproverRoleID { get; set; }

        [Column("approval_actual_tat")]
        public int ApprovalActualTAT { get; set; }

        [Column("approval_status")]
        public string ApprovalStatus { get; set; }
    }
}