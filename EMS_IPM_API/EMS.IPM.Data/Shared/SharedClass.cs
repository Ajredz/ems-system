using EMS.IPM.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.Shared
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_autocomplete")]
    public class TableVariableAutoComplete
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}