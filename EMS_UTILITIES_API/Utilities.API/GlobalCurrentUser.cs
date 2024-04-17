using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.API
{
    public class GlobalCurrentUser
    {
        public string Username { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string EmploymentStatus { get; set; }

        public string Company { get; set; }

        public short CompanyID { get; set; }

        public string Branch { get; set; }

        public string BranchCode { get; set; }

        public string Position { get; set; }
        
        public int PositionID { get; set; }

        public int OrgGroupID { get; set; }

        public List<int> OrgGroupRovingDescendants { get; set; }

        public List<int> OrgGroupDescendants { get; set; }

        public string EmployeeUnderAccess { get; set; }

        public List<int> OrgGroupAccess { get; set; }

        public List<GlobalCurrentUserRoving> Roving { get; set; }

        public int UserID { get; set; }

        public string LastLoggedIn { get; set; }

        public string LastLoggedOut { get; set; }

        public string LastPasswordChange { get; set; }
        
        public bool IsActive { get; set; }

        public int EmployeeID { get; set; }

        public string IPAddress { get; set; }
        public string ComputerName { get; set; }

        public bool IsPasswordChanged { get; set; }

    }

    public class GlobalCurrentUserRoving
    {
        public int PositionID { get; set; }
        public int OrgGroupID { get; set; }
    }
}
