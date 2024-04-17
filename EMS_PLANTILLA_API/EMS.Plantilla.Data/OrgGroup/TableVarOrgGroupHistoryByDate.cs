using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Plantilla.Data.OrgGroup
{
    [Table("tv_org_group_history_by_date")]
    public class TableVarOrgGroupHistoryByDate
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("tdate")]
        public string TDate { get; set; }
        [Column("is_latest")]
        public bool IsLatest { get; set; }

        [Column("org_type")]
        public string OrgType { get; set; }

        [Column("is_latest_description")]
        public string IsLatestDescription { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("parent_org_id")]
        public int ParentOrgId { get; set; }
        [Column("parent_description")]
        public string ParentDescription { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
