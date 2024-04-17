using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_trans_id")]
    public class TableVarMRFTransID
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("trans_id")]
        public string TransID { get; set; }
    }
}