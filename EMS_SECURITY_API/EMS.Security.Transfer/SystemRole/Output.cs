using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Security.Transfer.SystemRole
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
        public string RoleName { get; set; }
        public string DateCreated { get; set; }
    }
}
