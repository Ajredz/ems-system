using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Plantilla.Data.Dashboard
{
    [Table("tv_plantilla_dashboard")]
    public class TableVarPlantillaDashboard
    {
        [Key]
        [Column("value")]
        public string Value { get; set; }

        [Column("count")]
        public int Count { get; set; }
    }
}
