using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Workflow.Transfer.EmployeeScore
{
    public class Form
    {
        public int TID { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
    }

    public class BatchEmployeeScoreForm
    {
        public List<int> IDs { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

}
