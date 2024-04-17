using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Common.Transfer.SystemUser
{
    public class Form
    {
          public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
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

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
