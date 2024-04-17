using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.H2Pay.Data.SystemUser
{
    public class spSystemUserGetAllSync_Result
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int FailedAttempt { get; set; }
        public bool IsPasswordChanged { get; set; }
        public bool IsActive { get; set; }
    }

}