using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Plantilla.Data.OrgGroup
{
    [Table("tv_org_group_history_list")]
    public class TableVarOrgGroupHistoryList
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("tdate")]
        public string TDate { get; set; }

        [Column("is_latest")]
        public bool IsLatest { get; set; }

        [Column("is_latest_description")]
        public string IsLatestDescription { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
