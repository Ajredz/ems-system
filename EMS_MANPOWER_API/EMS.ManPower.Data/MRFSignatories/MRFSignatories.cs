using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRFSignatories
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("mrf_signatories")]
    public class MRFSignatories
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("requester_id")]
        public int RequesterID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("workflow_code")]
        public string WorkflowCode { get; set; }
    }
}