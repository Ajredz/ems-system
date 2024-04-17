using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.IPM.Transfer.KPIPosition
{

    public class UploadFileColumn
    {
        public static readonly byte RowNum = 0;
        public static readonly byte EffectiveDate = 0;
        public static readonly byte KPICode = 1;
        public static readonly byte PositionCode = 2;
        public static readonly byte Position = 3;
        public static readonly byte Weight = 4;
    }

    public class UploadFileEntity
    {
        public string RowNum { get; set; }
        public string EffectiveDate { get; set; }
        public DateTime EffectiveDateConverted { get; set; }
        public string KPICode { get; set; }
        public string PositionCode { get; set; }
        public string Position { get; set; }
        public string Weight { get; set; }
    }

    public class KPIEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class KRAGroupEntity
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
