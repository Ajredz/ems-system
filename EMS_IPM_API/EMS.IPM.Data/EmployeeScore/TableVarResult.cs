using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.IPM.Data.EmployeeScore
{
    [Table("tv_result")]
    public class TableVarResult
    {
        [Key]
        [Column("result")]
        public string Result { get; set; }
    }
}
