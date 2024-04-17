using Utilities.API;

namespace EMS.IPM.Transfer.KPI
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string KRAType { get; set; }

        public string KRAGroup { get; set; }

        public string KRASubGroup { get; set; }

        public string Name { get; set; }

        public string OldKPICode { get; set; }

        public string KPIType { get; set; }

        public string SourceType { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetAllKPIDetailsOutput
    {
        public int ID { get; set; }

        public string KRAGroup { get; set; }

        public string KRASubGroup { get; set; }

        public string KPICode { get; set; }

        public string OldKPICode { get; set; }

        public string KPIName { get; set; }

        public string KPIDescription { get; set; }

    }
}