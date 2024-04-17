using System;
using Utilities.API;

namespace EMS.IPM.Transfer.KPIPosition
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string Position { get; set; }

        public string Weight { get; set; }

        public string EffectiveDate { get; set; }
    }

    public class GetAllKPIOutput
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class GetAllKPIPositionOutput
    {
        public int ID { get; set; }

        public DateTime TDate { get; set; }

        public int Position { get; set; }

        public int KPI { get; set; }

        public decimal Weight { get; set; }

    }

    public class GetAllKPIPositionDetailsOutput
    {
        public int ID { get; set; }

        public string KRAGroup { get; set; }

        public string KPICode { get; set; }
        
        public string KPIName { get; set; }

        public string KPIDescription { get; set; }

        public decimal Weight { get; set; }

        public decimal PositionID { get; set; }
    }

    public class GetExportListOutput : JQGridResult
    {
        public int ID { get; set; }

        public string Position { get; set; }

        public string EffectiveDate { get; set; }

        public string KPI { get; set; }

        public string Weight { get; set; }

    }
    public class GetCopyPositionOutput
    {
        public string ID { get; set; }

        public string Description { get; set; }
    }
}