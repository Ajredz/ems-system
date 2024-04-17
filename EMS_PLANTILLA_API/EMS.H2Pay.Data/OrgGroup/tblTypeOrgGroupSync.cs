using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.H2Pay.Data.OrgGroup
{
    public class tblTypeOrgGroupSync
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrgGroupParentCode { get; set; }
        public string OrgGroupLevel { get; set; }
        public int Plantilla { get; set; }
        public bool IsActive { get; set; }
    }

}