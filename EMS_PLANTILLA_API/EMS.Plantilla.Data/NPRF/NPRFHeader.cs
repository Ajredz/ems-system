using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.DBObjects.NPRF
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("nprf")]
    public class NPRFHeader
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("date_required")]
        public DateTime DateRequired { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("reason")]
        public string Reason { get; set; }

        [Column("nature_of_employment")]
        public string NatureOfEmployment { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("company_id")]
        public int CompanyID { get; set; }

    }


}
