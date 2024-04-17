using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Security.Data.SystemRole
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_system_role_access")]
    public class TableVarSystemRoleAccess
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("parent_code")]
        public string ParentCode { get; set; }
        
        [Column("parent_page_id")]
        public int ParentPageID { get; set; }

        [Column("page_id")]
        public int PageID { get; set; }

        [Column("title")]
        public string Title { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column("url")]
        public string URL { get; set; }
        
        [Column("function_type")]
        public string FunctionType { get; set; }
        
        [Column("has_access")]
        public bool HasAccess { get; set; }
        
        [Column("on_menu_level")]
        public string OnMenuLevel { get; set; }
    }
}
