using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Training
{
    [Table("tv_training_template")]
    public class TableVarTableTemplate
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("template_name")]
        public string TemplateName { get; set; }

        [Column("created_date")]
        public string CreatedDate { get; set; }


        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
