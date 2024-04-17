using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Security.Data.SystemRole
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_role_page")]
    public class SystemRolePage
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("role_id")]
        public int RoleID { get; set; }
        
        [Column("page_id")]
        public int PageID { get; set; }
        
        [Column("function_type")]
        public string FunctionType { get; set; }
        
        [Column("is_hidden")]
        public bool IsHidden { get; set; }
        
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}