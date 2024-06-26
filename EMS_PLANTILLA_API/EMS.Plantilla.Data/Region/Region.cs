﻿using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.Region
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("region")]
    public class Region
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

    }
}
