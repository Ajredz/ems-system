using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Common.Data.CompanyDatabase
{
    [Table("vwcompanydatabase")]
    public class vwCompanyDatabase
    {
        [Key]
        [Column("company_id")]
        public int CompanyID { get; set; }
        [Column("company_code")]
        public string CompanyCode { get; set; }
        [Column("company_name")]
        public string CompanyName { get; set; }
        [Column("module_code")]
        public string ModuleCode { get; set; }
        [Column("ConnectionString")]
        public string ConnectionString { get; set; }
    }
}