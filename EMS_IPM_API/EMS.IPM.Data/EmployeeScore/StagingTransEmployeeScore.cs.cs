using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.IPM.Data.EmployeeScore
{


    [Table("staging_trans_employee_score")]
    public class StagingTransEmployeeScore
    {
        [Key]
        [Column("temp_pk")]
        public string temp_pk { get; set; }
        [Column("trans_summary_id")]
        public int TransSummaryID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("score")]
        public decimal Score { get; set; }
        [Column("pdate_from")]
        public DateTime PDateFrom { get; set; }
        [Column("pdate_to")]
        public DateTime PDateTo { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("level_of_approval")]
        public int? LevelOfApproval { get; set; }
        [Column("requestor_id")]
        public int? RequestorID { get; set; }
        [Column("approver_ids")]
        public string ApproverIDs { get; set; }
        [Column("approver_position_id")]
        public int? ApproverPositionID { get; set; }
        [Column("approver_org_group_id")]
        public int? ApproverOrgGroupID { get; set; }
        [Column("approver_role_ids")]
        public string ApproverRoleIDs { get; set; }

        [Column("employee_code")]
        public string EmployeeCode { get; set; }

        [Column("lastname")]
        public string Lastname { get; set; }

        [Column("firstname")]
        public string Firstname { get; set; }

        [Column("middlename")]
        public string Middlename { get; set; }

        [Column("suffix")]
        public string Suffix { get; set; }

        [Column("org_group_code")]
        public string OrgGroupCode { get; set; }

        [Column("org_group_description")]
        public string OrgGroupDescription { get; set; }

        [Column("position_code")]
        public string PositionCode { get; set; }

        [Column("position_title")]
        public string PositionTitle { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("region_department")]
        public string RegionDepartment { get; set; }

        [Column("is_active")]
        public bool isActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

    }
}
