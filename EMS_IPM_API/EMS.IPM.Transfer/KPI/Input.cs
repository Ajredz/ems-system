using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.KPI
{
    public class Form
    {
        public int ID { get; set; }

        public int KRAGroup { get; set; }

        public int? KRASubGroup { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TargetGuidelines { get; set; }

        public string OldKPICode { get; set; }

        public string Type { get; set; }

        public string SourceType { get; set; }

        public int ModifiedBy { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string KRATypeDelimited { get; set; }

        public string KRAGroup { get; set; }

        public string KRASubGroup { get; set; }

        public string Name { get; set; }

        public string OldKPICode { get; set; }

        public string KPITypeDelimited { get; set; }

        public string SourceTypeDelimited { get; set; }

        public bool IsExport { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }
    }
    public class GetDropDownInput
    {
        public int ID { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }
}