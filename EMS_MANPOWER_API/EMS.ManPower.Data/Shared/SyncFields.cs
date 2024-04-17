using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Manpower.Data.Shared
{
    public class SyncFields
    {
        [Column("sync_id")]
        public int SyncID { get; set; }

        [Column("sync_date")]
        public DateTime SyncDate { get; set; }
    }
}
