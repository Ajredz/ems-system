using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Common.Data.CompanyDatabase
{
    public class CompanyDatabase
    {
        [Key]
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ModuleID { get; set; }
        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}