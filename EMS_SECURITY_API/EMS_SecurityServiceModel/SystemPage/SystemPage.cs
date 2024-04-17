using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_SecurityServiceModel.SystemPage
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_page")]
    public class SystemPage
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("url")]
        public string URL { get; set; }

        [Column("parent_page_id")]
        public int ParentPageID { get; set; }

        [Column("on_menu_level")]
        public string OnMenuLevel { get; set; }

        [Column("is_hidden")]
        public bool IsHidden { get; set; }

        [Column("child_order")]
        public short ChildOrder { get; set; }
    }
}