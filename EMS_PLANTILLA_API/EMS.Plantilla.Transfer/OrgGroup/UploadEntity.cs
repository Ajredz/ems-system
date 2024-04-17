using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Plantilla.Transfer.OrgGroup
{

    public class UploadFileColumn
    {
        public static readonly byte RowNum = 0;
        public static readonly byte ParentOrgCode = 0;
        public static readonly byte OrgGroupCode = 1;
        public static readonly byte OrgGroupDescription = 2;
        public static readonly byte OrgType = 3;
        public static readonly byte CompanyTag = 4;
        public static readonly byte PositionCode = 5;
        public static readonly byte ReportingPositionCode = 6;
        public static readonly byte PlannedCount = 7;
        //public static readonly byte ActiveCount = 7;
        //public static readonly byte InactiveCount = 8;
        public static readonly byte Address = 8;
        public static readonly byte IsBranchActive = 9;
        public static readonly byte ServiceBayCount = 10;
        //public static readonly byte IsHead = 8;
    }

    public class UploadFileEntity
    {
        public string RowNum { get; set; }
        public string ParentOrgCode { get; set; }
        public string OrgGroupCode { get; set; }
        public string OrgGroupDescription { get; set; }
        public string OrgType { get; set; }
        public string Address { get; set; }
        public string IsBranchActive { get; set; }
        public string ServiceBayCount { get; set; }
        public string CompanyTag { get; set; }
        public string PositionCode { get; set; }
        public string ReportingPositionCode { get; set; }
        public string PlannedCount { get; set; }
        //public string ActiveCount { get; set; }
        //public string InactiveCount { get; set; }
        public string IsHead { get; set; }
    }

    public class OrgGroupEntity
    {
        public int ParentOrgID { get; set; }
        public string ParentOrgCode { get; set; }
        public string OrgGroupCode { get; set; }
        public string OrgGroupDescription { get; set; }
        public string OrgType { get; set; }
        public string Address { get; set; }
        public bool IsBranchActive { get; set; }
        public int ServiceBayCount { get; set; }
        public int UploadedBy { get; set; }
        public List<OrgGroupPositionEntity> OrgGroupPositionList { get; set; }
        public List<OrgGroupTagEntity> OrgGroupTagList { get; set; }
    }

    public class OrgGroupPositionEntity
    {
        public int PositionID { get; set; }
        public string PositionCode { get; set; }
        public int ReportingPositionID { get; set; }
        public int PlannedCount { get; set; }
        public int InactiveCount { get; set; }
        public int ActiveCount { get; set; }
        public bool IsHead { get; set; }
    }

    public class OrgGroupTagEntity
    {
        public string RefCode { get; set; }
        public string Value { get; set; }
    }
}
