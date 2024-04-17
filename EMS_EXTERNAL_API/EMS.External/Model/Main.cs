using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.External.API.Model
{

    public class GetUserDetailsInput
    { 
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class GetUserDetailsOutput
    {
        public string Username { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Company { get; set; }

        public short CompanyID { get; set; }

        public int OrgGroupID { get; set; }

        public string OrgGroupCode { get; set; }

        public string OrgGroupName { get; set; }

        public int PositionID { get; set; }

        public string PositionCode { get; set; }

        public string PositionName { get; set; }

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


    public class GetEmployeeByUsernameOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupCode { get; set; }
        public string OrgGroupDescription { get; set; }
        public string OrgGroupConcatenated { get; set; }
        public string OrgType { get; set; }
        public int PositionID { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public string PositionConcatenated { get; set; }
        public string Company { get; set; }
        public string RefValueCompanyTag { get; set; }
        public string Email { get; set; }
    }
}
