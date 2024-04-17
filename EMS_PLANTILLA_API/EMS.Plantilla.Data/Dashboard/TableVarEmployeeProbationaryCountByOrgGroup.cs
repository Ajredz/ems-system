using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Plantilla.Data.Dashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_probationary_count_by_org_group")]
    public class TableVarEmployeeProbationaryCountByOrgGroup
    {
        [Key]
        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
