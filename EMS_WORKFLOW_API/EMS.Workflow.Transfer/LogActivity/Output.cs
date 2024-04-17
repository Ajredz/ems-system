using System;
using Utilities.API;

namespace EMS.Workflow.Transfer.LogActivity
{
    public class GetApplicantLogActivityByApplicantIDOutput
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentTimestamp { get; set; }
        public string AssignedUser { get; set; }
        public int AssignedUserID { get; set; }
        public string AssignedOrgGroup { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public string IsPass { get; set; }
        public int CreatedBy { get; set; }
        public string AssignedBy { get; set; }
    }

    public class GetApplicantLogActivityStatusHistoryOutput
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public int UserID { get; set; }
        public string User { get; set; }
        public string Remarks { get; set; }
        public string IsPass { get; set; }
    }

     public class GetEmployeeLogActivityByEmployeeIDOutput
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentTimestamp { get; set; }
        public string AssignedUser { get; set; }
        public int AssignedUserID { get; set; }
        public string AssignedOrgGroup { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public string IsPass { get; set; }
        public int CreatedBy { get; set; }
        public string AssignedBy { get; set; }
    }

    public class GetEmployeeLogActivityStatusHistoryOutput
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public int UserID { get; set; }
        public string User { get; set; }
        public string Remarks { get; set; }
        public string IsPass { get; set; }
    }

    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }

        public string Module { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IsPassFail { get; set; }

        public string IsAssignment { get; set; }
    }

    public class GetAssignedActivitiesListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentTimestamp { get; set; }
        public string DueDate { get; set; }
        public string IsPass { get; set; }
        public int CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public int ApplicantID { get; set; }
        public string ApplicantName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupName { get; set; }
        public string CurrentStatusCode { get; set; }
        public string Remarks { get; set; }
    }


    public class ApplicantLogActivityGetCommentsOutput
    {
        public string Timestamp { get; set; }

        public string Sender { get; set; }

        public int CreatedBy { get; set; }

        public string Comments { get; set; }
    }
    
    public class EmployeeLogActivityGetCommentsOutput
    {
        public string Timestamp { get; set; }

        public string Sender { get; set; }

        public int CreatedBy { get; set; }

        public string Comments { get; set; }
    }

    public class GetLogActivityByPreloadedIDOutput
    {
        public string Module { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPassFail { get; set; }
        public bool IsAssignment { get; set; }
        public bool IsVisible { get; set; }
        public int AssignedUserID { get; set; }
        public string AssignedUserName { get; set; }
    }

    public class GetPreLoadedListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string PreloadName { get; set; }

        public string DateCreated { get; set; }
    }

    public class GetChecklistListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentTimestamp { get; set; }
        public string IsPass { get; set; }
        public int CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public int AssignedUserID { get; set; }
        public string AssignedTo { get; set; }
        public int EmployeeID { get; set; }
        public string Remarks { get; set; }
        public bool IsAssignment { get; set; }
        public string DueDate { get; set; }
    }

    public class GetApplicantLogActivityListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentTimestamp { get; set; }
        public string IsPass { get; set; }
        public int CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public int AssignedUserID { get; set; }
        public string AssignedTo { get; set; }
        public int ApplicantID { get; set; }
        public string Remarks { get; set; }
        public bool IsAssignment { get; set; }
        public string DueDate { get; set; }
    }
}
