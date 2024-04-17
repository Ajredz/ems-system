using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_SecurityServiceModel.SystemRole
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_role")]
    public class SystemRole
    {
        [Key]
        [Column("id")]
        public short ID { get; set; }

        [Column("company_id")]
        public short CompanyID { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }

        [Column("integration_key")]
        public string IntegrationKey { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public int ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }
}
