using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.KPIScore
{
    public class Form
    {
        public int ID { get; set; }

        public int OrgGroup { get; set; }
        public int EmployeeID { get; set; }

        public int Position { get; set; }

        public List<KPIScoreForm> KPIScoreList { get; set; }
    }

    public class KPIScoreForm
    {
        public int ID { get; set; }

        public int KPIID { get; set; }

        public string KRAGroup { get; set; }

        public string KPICode { get; set; }

        public string KPIName { get; set; }
            
        public string KPIDescription { get; set; }

        public decimal? Target { get; set; }

        public decimal? Actual { get; set; }

        public decimal Rate { get; set; }
        
        public string Period { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public string ID { get; set; }

        public string ParentOrgGroup { get; set; }

        public string EmployeeDelimited { get; set; }
        public string OrgGroupDelimited { get; set; }

        public string KPIDelimited { get; set; }

        public decimal? Target { get; set; }

        public decimal? Actual { get; set; }

        public decimal? Rate { get; set; }
        
        public string PeriodFrom { get; set; }
        
        public string PeriodTo { get; set; }

        public bool IsExport { get; set; }
        public string KPIType { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }

        public string KPIType { get; set; }
    }

    public class UploadScoresFile
    {
        public string RowNum { get; set; }

        public string KPICode { get; set; }
        public int KPIID { get; set; }

        public string EmployeeCode { get; set; }
        public int EmployeeID { get; set; }
        
        public string OrgGroupCode { get; set; }
        public int OrgGroupID { get; set; }

        public string Target { get; set; }

        public string Actual { get; set; }

        public string Rate { get; set; }
        public string Period { get; set; }
        public DateTime PeriodDateConverted { get; set; }

        public string KPIType { get; set; }
    }
}