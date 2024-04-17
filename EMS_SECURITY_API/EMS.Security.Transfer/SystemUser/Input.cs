using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Security.Transfer.SystemUser
{
    public class Form {
        public int ID { get; set; }

        public short CompanyID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public short FailedAttempt { get; set; }

        public bool IsPasswordChanged { get; set; }

        public bool IsLoggedIn { get; set; }

        public string IntegrationKey { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLoggedIn { get; set; }

        public DateTime? LastLoggedOut { get; set; }

        public DateTime? LastPasswordChange { get; set; }

        public List<int> RoleIDs { get; set; }

    }


    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
        public short CompanyID { get; set; }
    }

    public class GetByNameInput
    {
        public string EmployeeCode { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int CreatedBy { get; set; }
    }

    public class ChangePasswordInput
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
    
    public class GetListInput : JQGridFilter
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string DateModifiedFrom { get; set; }
        public string DateModifiedTo { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public bool IsExport { get; set; }
    }

    public class BatchResetPasswordForm
    {
        public int ModifiedBy { get; set; }
        public List<int> SystemUserIDs { get; set; }
    }

    public class EmployeeUploadInsertInput
    {
        public string NewEmployeeCode { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int CreatedBy { get; set; }
    }

    public class UpdateUsername
    {
        public int ID { get; set; }
        public string Username { get; set; }
    }

    public class ChangeStatusForm
    {
        public int ModifiedBy { get; set; }
        public List<int> SystemUserIDs { get; set; }
    }

    public class ForceChangePasswordInput
    {
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

    public class UpdateSystemUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }

    public class ResetPassword
    {
        public int ID { get; set; }
        public string NewPassword { get; set; }
        public bool ForceChangePassword { get; set; }
    }
}
