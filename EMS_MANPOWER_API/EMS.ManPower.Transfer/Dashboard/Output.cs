using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Manpower.Transfer.Dashboard
{
    public class MRFDashboard
    {
        public string OpenDesc { get; set; }
        public int OpenVal { get; set; }
        public int OpenCount { get; set; }
        public string ClosedDesc { get; set; }
        public int ClosedVal { get; set; }
        public int ClosedCount { get; set; }
        public string Age { get; set; }

    }

    public class MRFDashboardList
    {
        public string OpenDescription { get; set; }
        public int OpenValue { get; set; }
        public List<int> OpenCountList { get; set; }
        public string ClosedDescription { get; set; }
        public int ClosedValue { get; set; }
        public List<int> ClosedCountList { get; set; }
        public List<string> AgeList { get; set; }

    }
}
