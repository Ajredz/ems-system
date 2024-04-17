using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Security.Transfer.SystemUser
{
    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetByUserIDOutput
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }

    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string DateModified { get; set; }
        public string DateCreated { get; set; }
    }

    public class EmployeeUploadInsertOutput
    {
        public int SystemUserID { get; set; }
        public string NewEmployeeCode { get; set; }
    }
}
