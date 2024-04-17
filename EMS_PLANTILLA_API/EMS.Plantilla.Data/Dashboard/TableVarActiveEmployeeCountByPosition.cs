﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.Dashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_active_employee_count_per_position")]
    public class TableVarActiveEmployeeCountByPosition
    {
        [Key]
        [Column("position")]
        public string Position { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
