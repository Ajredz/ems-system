using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Security.Transfer.SystemRole
{

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
        public short CompanyID { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string RoleName { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public bool IsExport { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string RoleName { get; set; }
        public int CreatedBy { get; set; }

        //public List<SystemUserRoleForm> SystemUserRoleList { get; set; }
        //public List<SystemRolePageForm> SystemRolePageList { get; set; }
        
        public List<SystemRoleAccess> SystemRoleAccessList { get; set; }
    }

    //public class SystemUserRoleForm
    //{
    //    public int UserID { get; set; }
    //    public int RoleID { get; set; }
    //    public int CreatedBy { get; set; }
    //}

    public class SystemRoleAccess
    {
        public int ID { get; set; }
        public string ParentCode { get; set; }
        public int ParentPageID { get; set; }
        public int PageID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FunctionType { get; set; }
        public bool HasAccess { get; set; }
    }

    public class SystemRolePageForm
    {
        public int RoleID { get; set; }
        public int PageID { get; set; }
        public string FunctionType { get; set; }
        public bool IsHidden { get; set; }
        public int CreatedBy { get; set; }
    }
}
