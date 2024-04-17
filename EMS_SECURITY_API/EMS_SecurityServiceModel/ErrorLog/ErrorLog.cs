using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_SecurityServiceModel
{
    [Table("error_log")]
    public class ErrorLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("layer")]
        public string Layer { get; set; }

        [Column("class")]
        public string Class { get; set; }

        [Column("method")]
        public string Method { get; set; }

        [Column("error_message")]
        public string ErrorMessage { get; set; }

        [Column("inner_exception")]
        public string InnerException { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

    }
}
