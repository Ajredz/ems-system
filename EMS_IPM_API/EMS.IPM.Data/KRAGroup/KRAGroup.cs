using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.KRAGroup
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("kra_group")]
    public class KRAGroup
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public string Type { get; set; }

    }
}
