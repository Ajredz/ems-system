using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.Common.Transfer.CompanyDatabase
{

  
    public class GetByCompanyCodeOutput
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ModuleCode { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public class GetByModuleCodeOutput
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ModuleCode { get; set; }
        public string ConnectionString { get; set; }
    } 
    
    public class GetAllOutput
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ModuleCode { get; set; }
        public string EncryptedConnectionString { get; set; }
    }

}