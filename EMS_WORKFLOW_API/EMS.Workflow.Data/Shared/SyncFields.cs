using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Shared
{
    public class SyncFields
    {
        [Column("sync_id")]
        public int SyncID { get; set; }

        [Column("sync_date")]
        public DateTime SyncDate { get; set; }
    }
}