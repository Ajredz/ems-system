using System.Collections.Generic;
using Utilities.API;

namespace EMS.Manpower.Transfer.MRFApproval
{
    public class Form
    {
        public int ID { get; set; }

        public int AgeOfRequest { get; set; }

        public string DateOfRequest { get; set; }

        public string Requester { get; set; }

        public string Status { get; set; }

        public string MRFTransactionID { get; set; }

        public string OldMRFID { get; set; }

        public int OrgGroupID { get; set; }

        public int PositionLevelID { get; set; }

        public string NatureOfEmploymentValue { get; set; }

        public int PositionID { get; set; }

        public string PurposeValue { get; set; }

        public int Vacancy { get; set; }

        public int TurnaroundTime { get; set; }

        public string Remarks { get; set; }

        public int CreatedBy { get; set; }

        public List<MRFSignatoriesForm> Signatories { get; set; }

        public int ApproverPositionID { get; set; }

        public int ApproverOrgGroupID { get; set; }
        
        public int AltApproverPositionID { get; set; }

        public int AltApproverOrgGroupID { get; set; }

        public int LevelOfApproval { get; set; }
    }

    public class MRFSignatoriesForm
    {
        public string ApproverDescription { get; set; }

        public int ApproverRoleID { get; set; }

        public int ApprovalActualTAT { get; set; }

        public string ApprovalStatus { get; set; }
    }

    public class GetApprovalListInput : JQGridFilter
    {
        public int ApproverPositionID { get; set; }
        public int ApproverOrgGroupID { get; set; }
        public string RovingPositionDelimited { get; set; }
        public string RovingOrgGroupDelimited { get; set; }
        public int ApproverID { get; set; }
        public int? ID { get; set; }
        public string MRFTransactionID { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string PositionLevelDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public string NatureOfEmploymentDelimited { get; set; }
        public int? NoOfApplicantMin { get; set; }
        public int? NoOfApplicantMax { get; set; }
        public string StatusDelimited { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public string DateApprovedFrom { get; set; }
        public string DateApprovedTo { get; set; }
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
    }

    public class ApproverResponse
    {
        public int RecordID { get; set; }
        public int LevelOfApproval { get; set; }
        public Enums.MRF_APPROVER_STATUS Result { get; set; }
        public int NextApproverPositionID { get; set; }
        public int NextApproverOrgGroupID { get; set; }
        public string Remarks { get; set; }
    }
}