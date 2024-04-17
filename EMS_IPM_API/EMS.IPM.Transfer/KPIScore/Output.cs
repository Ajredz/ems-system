using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.KPIScore
{
    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }

        public string Employee { get; set; }

        public string ParentOrgGroup { get; set; }

        public string OrgGroup { get; set; }

        public string KPI { get; set; }

        public string Target { get; set; }

        public string Actual { get; set; }

        public string Rate { get; set; }

        public string Period { get; set; }
    }

    public class UploadScoresOutput
    {
        public string Message { get; set; }
    }

    public class GetAllKPIScoreOutput
    {
        public int ID { get; set; }

        public int OrgGroup { get; set; }

        public int KPIPosition { get; set; }

        public decimal? Target { get; set; }

        public decimal? Actual { get; set; }

        public decimal Rate { get; set; }

        public string Formula { get; set; }
    }
}