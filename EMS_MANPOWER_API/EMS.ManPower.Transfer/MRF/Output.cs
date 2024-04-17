using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Manpower.Transfer.MRF
{
    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string MRFID { get; set; }
        public string OrgGroupDescription { get; set; }
        public string ScopeOrgGroup { get; set; }
        public string PositionLevelDescription { get; set; }
        public string PositionDescription { get; set; }
        public string NatureOfEmployment { get; set; }
        public string Purpose { get; set; }
        public int NoOfApplicant { get; set; }
        public string Status { get; set; }
        public string ApprovedDate { get; set; }
        public string HiredDate { get; set; }
        public int Age { get; set; }

        public bool IsApproved { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class MRFApprovalHistoryOutput
    {
        public int HierarchyLevel { get; set; }
        public int PositionID { get; set; }
        public string PositionCode { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupCode { get; set; }
        public int AltPositionID { get; set; }
        public int AltOrgGroupID { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusCode { get; set; }
        public string ApprovedDate { get; set; }
        public int ApproverID { get; set; }
        public string ApproverName { get; set; }
        public string ApprovalRemarks { get; set; }
    }

    public class MRFGetCommentsOutput
    {
        public string Timestamp { get; set; }
        
        public string Sender { get; set; }

        public int CreatedBy { get; set; }

        public string Comments { get; set; }
    }

    public class GetMRFExistingApplicantListOutput : JQGridResult
    {
        public int ID { get; set; }
        public long MRFApplicantID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicationSource { get; set; }
        public string CurrentStep { get; set; }
        public string Status { get; set; }
        public int WorkflowID { get; set; }
        public string CurrentStepCode { get; set; }
        public string CurrentResult { get; set; }
        public string ResultType { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string ApproverRemarks { get; set; }
        public int Points { get; set; }
        public int TotalPoints { get; set; }
        public int Flag { get; set; }
        public string LastUpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdateByName { get; set; }
    }


    //API FOR GET ONLINE MRF
    public class GetListOutputMrfOnline
    {
        public string Position { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string JobQualification { get; set; }
        public int MrfId { get; set; }
        public string ClosedDate { get; set; }
        public int ApplicantCount { get; set; }
        public string MrfCreatedDate { get; set; }
        public int PositionID { get; set; }
    }
    // GET THE LAST INSERTED DATA
    public class LastId
    {
        public long Id { get; set; }
    }
    public class KickoutQuestionOutput
    { 
        public int ID { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string QuestionType { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class MRFKickoutQuestionListOutput
    { 
        public int ID { get; set; }
        public int MRFID { get; set; }
        public int KickoutQuestionID { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string QuestionType { get; set; }
        public int Order { get; set; }
    }
    public class kickoutQuestionAutoComplete
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}
