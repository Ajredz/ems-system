using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.KPIPosition
{
    public class Form
    {
        public int PositionID { get; set; }

        public string Position { get; set; }

        public string EffectiveDate { get; set; }

        public int ModifiedBy { get; set; }

        public List<KPIPositionForm> KPIPositionList { get; set; }
    }

    public class KPIPositionForm
    {
        public int ID { get; set; }

        public string KRAGroup { get; set; }

        public string KRASubGroup { get; set; }

        public string KPICode { get; set; }

        public string KPIName { get; set; }

        public string KPIDescription { get; set; }

        public string KPI { get; set; }

        public int KPIID { get; set; }
    
        public decimal Weight { get; set; }
        public decimal WeightNoServiceBay { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string PositionDelimited { get; set; }

        public decimal? Weight{ get; set; }

        public string DateEffectiveFrom { get; set; }

        public string DateEffectiveTo { get; set; }

        public bool IsShowRecentOnly { get; set; }

        public bool IsExport { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }

        public string EffectiveDate { get; set; }
    }

    public class CopyKpiPositionInput
    {
        public int OldPosition { get; set; }
        public int NewPosition { get; set; }

        public string OldEffectiveDate { get; set; }
        public string NewEffectiveDate { get; set; }

    }
}