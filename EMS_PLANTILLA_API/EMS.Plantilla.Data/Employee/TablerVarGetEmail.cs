using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_get_email")]
    public class TableVarGetEmail
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("system_user_id")]
        public int SystemUserID { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("org_code")]
        public string OrgCode { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("pos_code")]
        public string PosCode { get; set; }

        [Column("title")]
        public string Title { get; set; }

    }
}
