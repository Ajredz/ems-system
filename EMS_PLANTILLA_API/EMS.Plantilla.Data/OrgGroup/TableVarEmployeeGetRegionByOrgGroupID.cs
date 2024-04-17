﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_get_region_by_org_group_id")]
    public class TableVarGetRegionByOrgGroupID
    {
        [Key]
        [Column("region_code")]
        public string RegionCode { get; set; }

        [Column("region")]
        public string Region { get; set; }

        [Column("monthly_rate")]
        public decimal MonthlyRate { get; set; }
    }
}