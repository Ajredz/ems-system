using System.Collections.Generic;
using Utilities.API;

namespace EMS.Manpower.Transfer.MRF
{
    public class Form
    {
        public int ID { get; set; }

        public string Status { get; set; }
        public string StatusCode { get; set; }

        public string MRFTransactionID { get; set; }

        public string OldMRFID { get; set; }

        public int OrgGroupID { get; set; }

        public int PositionLevelID { get; set; }
        public bool IsConfidential { get; set; }

        public string NatureOfEmploymentValue { get; set; }

        public int PositionID { get; set; }

        public string PurposeValue { get; set; }

        public int Vacancy { get; set; }

        public int TurnaroundTime { get; set; }

        public string Remarks { get; set; }

        public string Requester { get; set; }

        public int CreatedBy { get; set; }

        public string DateCreated { get; set; }
        
        public string DateModified { get; set; }
        public string DateApproved { get; set; }

        public int ApproverPositionID { get; set; }

        public int ApproverOrgGroupID { get; set; }

        public int AltApproverPositionID { get; set; }

        public int AltApproverOrgGroupID { get; set; }

        public List<MRFSignatoriesForm> Signatories { get; set; }

        public string ReasonForCancellation { get; set; }
        public string OnlinePosition { get; set; }
        public string OnlineLocation { get; set; }
        public string OnlineJobDescription { get; set; }
        public string OnlineJobQualification { get; set; }
        public bool IsAvailableOnline { get; set; }
    }

    public class MRFApprovalHistoryForm
    {
        public int RequestingPositionID { get; set; }
        public int RequestingOrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int MRFID { get; set; }
    }

    public class MRFApplicantForm
    {
        public int ID { get; set; }
        public string MRFTransactionID { get; set; }
        public string OrganizationalGroup { get; set; }
        public string Position { get; set; }
        public string PositionLevel { get; set; }
        public string NatureOfEmployment { get; set; }
        public int Count { get; set; }
        public string Purpose { get; set; }
        public string DateCreated { get; set; }
        public string DateApproved { get; set; }
        public string DateModified { get; set; }
        public string Remarks { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string IsConfidential { get; set; }
        public string DatePrinted { get; set; }
        public string SubmittedBy { get; set; }
        public int SubmittedByID { get; set; }
        public string HiredRemarks { get; set; }
        public string CurrentStepCode { get; set; }
        public string PrintDateCreated { get; set; }

        public List<ApplicantIDsAndForHiring> ApplicantIDs { get; set; }
    }

    public class MRFCommentsForm
    {
        public int MRFID { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
    }
    
    public class MRFApplicantCommentsForm
    {
        public int MRFID { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ApplicantIDsAndForHiring
    {
        public long MRFApplicantID { get; set; }
        public bool ForHiring { get; set; }
        public int ApplicantID { get; set; }

    }

    public class MRFPickApplicantForm
    {
        public string MRFTransactionID { get; set; }
        public int ID { get; set; }
        public int WorkflowID { get; set; }
        public List<MRFApplicantDetails> Applicants { get; set; }
    }

    public class MRFApplicantDetails
    {
        public int ApplicantID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public string CurrentStepCode { get; set; }
        public string CurrentStepDescription { get; set; }
        public string CurrentStepApproverRoleIDs { get; set; }
        public string ResultType { get; set; }
    }

    public class MRFApplicantWorkflowForm
    {
        public int WorkflowID { get; set; }

        public string CurrentStepCode { get; set; }

        public string CurrentStepDescription { get; set; }

        public string CurrentStepApproverRoleIDs { get; set; }
    }

    public class MRFSignatoriesForm
    {
        public string ApproverDescription { get; set; }

        public int ApproverRoleID { get; set; }

        public int ApprovalActualTAT { get; set; }

        public string ApprovalStatus { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string MRFTransactionID { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string ScopeOrgType { get; set; }
        public string ScopeOrgGroupDelimited { get; set; }
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
        public string DateHiredFrom { get; set; }
        public string DateHiredTo { get; set; }
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsExport { get; set; }
        public string OrgDescendant { get; set; }
    }

    public class RemoveApplicantInput
    {
        public int MRFID { get; set; }
        public int MRFApplicantID { get; set; }
    }

    public class UpdateForHiringApplicantInput
    {
        public int MRFID { get; set; }
        public int MRFApplicantID { get; set; }
        public bool ForHiring { get; set; }
    }

     public class UpdateStatusInput
    {
        public enum MRF_STATUS
        {
            OPEN,
            CLOSED,
            REJECTED,
            CANCELLED,
            CLOSED_INTERNAL
        }
        public int ID { get; set; }
        public MRF_STATUS Status { get; set; }
    }

    public class ValidateMRFExistingActualInput
    {
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
    }

    public class MRFCancelForm
    {
        public int MRFID { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateCurrentWorkflowStepInput
    {
        public long ID { get; set; }
        public string CurrentStepCode { get; set; }
        public string CurrentStepDescription { get; set; }
        public string WorkflowStatus { get; set; }
        public string CurrentStepApproverRoleIDs { get; set; }
        public string ResultType { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string ApproverRemarks { get; set; }

    }

    public class GetMRFExistingApplicantListInput : JQGridFilter
    {
        public int MRFID { get; set; }

        public string IDDelimited { get; set; }

        public int? ID { get; set; }


        public int ForHiringID { get; set; }

        public string ApplicantName { get; set; }

        public string CurrentStepDelimited { get; set; }

        public string StatusDelimited { get; set; }

        public string DateScheduledFrom { get; set; }
        public string DateScheduledTo { get; set; }
        public string DateCompletedFrom { get; set; }
        public string DateCompletedTo { get; set; }

        public string ApproverRemarks { get; set; }
    }

    public class GetApplicantByMRFIDAndIDInput
    {
        public int MRFID { get; set; }
        public int ApplicantID { get; set; }
    }
    public class ApplicantKickoutQuestionInput
    {
        public int ApplicantID { get; set; }
        public List<AddApplicantKickoutQuestionInput> addApplicantKickoutQuestionInputs { get; set; }
    }
    public class AddApplicantKickoutQuestionInput
    {
        public int Question { get; set; }
        public string Answer { get; set; }
    }
    public class AddKickoutQuestionInput
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string QuestionType { get; set; }
    }
    public class AddKickoutQuestionToMRFInput
    {
        public int ID { get; set; }
        public int MRFID { get; set; }
        public int KickoutQuestionID { get; set; }
        public int Order { get; set; }
    }

    public class GetByKickoutQuestionAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }
    public class MRFChangeStatusInput
    {
        public int MrfID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
