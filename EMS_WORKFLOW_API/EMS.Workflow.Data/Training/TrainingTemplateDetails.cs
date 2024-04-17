using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EMS.Workflow.Data.Training
{
    [Table("training_template_details")]
    public class TrainingTemplateDetails
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("training_template_id")]
        public int TrainingTemplateID { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
        [Column("modified_by")]
        public int? ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
