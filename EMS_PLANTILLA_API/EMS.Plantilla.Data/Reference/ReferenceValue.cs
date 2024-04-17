using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.Reference
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("reference_value")]
    public class ReferenceValue
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("ref_code")]
        public string RefCode { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}