using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.IPM.Data.EmployeeScore
{
    public class TransEmployeeScoreStagingTest
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("trans_summary_id")]
        public int TransSummaryID { get; set; }

    }
}
