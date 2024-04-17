using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf")]
    public class TableVarMRF
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("mrf_transaction_id")]
        public string MRFTransactionID { get; set; }

        [Column("org_group_description")]
        public string OrgGroupDescription { get; set; }
        
        [Column("scope_org_group")]
        public string ScopeOrgGroup { get; set; }

        [Column("position_level_description")]
        public string PositionLevelDescription { get; set; }

        [Column("position_description")]
        public string PositionDescription { get; set; }

        [Column("nature_of_employment")]
        public string NatureOfEmployment { get; set; }
        [Column("purpose")]
        public string Purpose { get; set; }

        [Column("no_of_applicant")]
        public int NoOfApplicant { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("approved_date")]
        public string ApprovedDate { get; set; }

        [Column("hired_date")]
        public string HiredDate { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("is_approved")]
        public bool IsApproved { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public string CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public string ModifiedDate { get; set; }
    }
}