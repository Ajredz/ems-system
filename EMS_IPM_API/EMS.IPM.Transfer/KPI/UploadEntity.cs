using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.IPM.Transfer.KPI
{

    public class UploadFileColumn
    {
        public static readonly byte RowNum = 0;
        public static readonly byte KRAGroup = 0;
        public static readonly byte OldKPICode = 1;
        public static readonly byte KPIName = 2;
        public static readonly byte Description = 3;
        public static readonly byte TargetGuidelines = 4;
        public static readonly byte KPIType = 5;
        public static readonly byte SourceType = 6;

    }

    public class UploadFileEntity
    {
        public string RowNum { get; set; }
        public string KRAGroup { get; set; }
        public string KPICode { get; set; }
        public string OldKPICode { get; set; }
        public string KPIName { get; set; }
        public string Description { get; set; }
        public string TargetGuidelines { get; set; }
        public string KPIType { get; set; }
        public string SourceType { get; set; }
    }
}
