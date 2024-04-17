using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Workflow.Transfer.LogActivity
{

    public class GetListInput : JQGridFilter
    {
        public int ID { get; set; }

        public string ModuleDelimited { get; set; }

        public string TypeDelimited { get; set; }

        public string SubTypeDelimited { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IsPassFail { get; set; }
        
        public string IsAssignment { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPassFail { get; set; }
        public bool IsAssignment { get; set; }
        public int LogActivityPreloadedID { get; set; }
        public bool IsVisible { get; set; }
        public int AssignedUserID { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class LogActivityForm
    {
        public int ID { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsWithPassFail { get; set; }
        public bool IsWithAssignment { get; set; }
        //public bool IsPreloaded { get; set; }
    }


    public class ApplicantLogActivityForm
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string AssignedUser { get; set; }
        public int AssignedUserID { get; set; }
        public bool IsPass { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsWithAssignment { get; set; }
        public bool IsWithPassFail { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsAssignToSelf { get; set; }
        public int ApplicantID { get; set; }
        public string OrgGroupDescription { get; set; }
    }

    public class EmployeeLogActivityForm
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string AssignedUser { get; set; }
        public int AssignedUserID { get; set; }
        public bool IsPass { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsWithAssignment { get; set; }
        public bool IsWithPassFail { get; set; }
        public bool IsVisible { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsAssignToSelf { get; set; }
        public int EmployeeID { get; set; }
        public string OrgGroupDescription { get; set; }
    }

    public class TagToApplicantForm
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsWithPassFail { get; set; }
        public bool IsWithAssignment { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int AssignedUserID { get; set; }
        public string Email { get; set; }
        public string ApplicantName { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsAssignToSelf { get; set; }
    }
    public class TagToEmployeeForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsWithPassFail { get; set; }
        public bool IsWithAssignment { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int AssignedUserID { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsVisible { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsAssignToSelf { get; set; }
    }

    public class GetLogActivityByModuleTypeInput
    {
        public List<Enums.ActivityModule> Modules { get; set; }
        public string Type { get; set; }
        public int SelectedID { get; set; }
    }

    public class GetAssignedActivitiesListInput : JQGridFilter
    {
        public int ID { get; set; }
        public string TypeDelimited { get; set; }
        public string SubTypeDelimited { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatusDelimited { get; set; }
        public string CurrentTimstampFrom { get; set; }
        public string CurrentTimstampTo { get; set; }
        public string DueDateFrom { get; set; }
        public string DueDateTo { get; set; }
        public string AssignedByDelimited { get; set; }
        public bool IsAdminAccess { get; set; }
        public string EmployeeDelimited { get; set; }
        public string ApplicantDelimited { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string Remarks { get; set; }
        public bool IsExport { get; set; }
    }

    public class ApplicantLogActivityCommentsForm
    {
        public int ApplicantLogActivityID { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ApplicantLogActivityAttachmentForm
    {
        public int ApplicantLogActivityID { get; set; }
        public List<AttachmentForm> AddAttachmentForm { get; set; }
        public List<AttachmentForm> DeleteAttachmentForm { get; set; }
    }

     public class EmployeeLogActivityCommentsForm
    {
        public int EmployeeLogActivityID { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
    }

    public class EmployeeLogActivityAttachmentForm
    {
        public int EmployeeLogActivityID { get; set; }
        public List<AttachmentForm> AddAttachmentForm { get; set; }
        public List<AttachmentForm> DeleteAttachmentForm { get; set; }
    }

    public class AttachmentForm
    {
        public string AttachmentType { get; set; }

        public string Remarks { get; set; }

        [IgnoreDataMember]
        public IFormFile File { get; set; }

        public string SourceFile { get; set; }

        public string ServerFile { get; set; }

        public int CreatedBy { get; set; }

        public string UploadedBy { get; set; }

        public string Timestamp { get; set; }
    }

    public class AddApplicantPreLoadedActivitiesInput
    {
        public List<int> LogActivityPreloadedIDs { get; set; }
        public int ApplicantID { get; set; }
    }

    public class GetPreLoadedListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string PreloadName { get; set; }

        public string DateCreatedFrom { get; set; }

        public string DateCreatedTo { get; set; }

        public bool IsExport { get; set; }
    }

    public class LogActivityPreloadedForm
    {
        public int ID { get; set; }
        public string PreloadedName { get; set; }
        public int CreatedBy { get; set; }
        public List<Form> LogActivityList { get; set; }
	}
	
    public class AddEmployeePreLoadedActivitiesInput
    {
        public List<int> LogActivityPreloadedIDs { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetChecklistListInput : JQGridFilter
    {
        public int EmployeeID { get; set; }
        public string TypeDelimited { get; set; }
        public string SubTypeDelimited { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatusDelimited { get; set; }
        public string CurrentTimestampFrom { get; set; }
        public string CurrentTimestampTo { get; set; }
        public string AssignedByDelimited { get; set; }
        public string AssignedToDelimited { get; set; }
        public string Remarks { get; set; }
        public string DueDateFrom { get; set; }
        public string DueDateTo { get; set; }
        public bool IsExport { get; set; }
    }

    public class UpdateEmployeeLogActivityAssignedUserForm
    {
        public List<int> LogActivityIDs { get; set; }
        public int AssignedUserID { get; set; }
        public DateTime? DueDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsAssignToSelf { get; set; }
    }

    public class UpdateApplicantLogActivityAssignedUserForm
    {
        public List<int> LogActivityIDs { get; set; }
        public int AssignedUserID { get; set; }
        public DateTime? DueDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsAssignToSelf { get; set; }
    }

    public class GetApplicantLogActivityListInput : JQGridFilter
    {
        public int ApplicantID { get; set; }
        public string TypeDelimited { get; set; }
        public string SubTypeDelimited { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatusDelimited { get; set; }
        public string CurrentTimestampFrom { get; set; }
        public string CurrentTimestampTo { get; set; }
        public string AssignedByDelimited { get; set; }
        public string AssignedToDelimited { get; set; }
        public string Remarks { get; set; }
        public string DueDateFrom { get; set; }
        public string DueDateTo { get; set; }
    }

    public class BatchTaskForm
    {
        public List<int> ApplicantIDs { get; set; }
        public List<int> EmployeeIDs { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class UploadLogActivityFile
    {
        public string RowNum { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string CurrentStatus { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public int AssignedOrgGroupID { get; set; }
        public int AssignedUserId { get; set; }
        public string OrgGroupCode { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }

    }
}
