﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Security.Data.SystemRole
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_system_role")]
    public class TableVarSystemRole
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }

        [Column("date_created")]
        public string DateCreated { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
