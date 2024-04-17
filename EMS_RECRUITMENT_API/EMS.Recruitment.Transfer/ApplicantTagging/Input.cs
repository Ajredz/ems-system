using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Recruitment.Transfer.ApplicantTagging
{
    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicationSourceDelimited { get; set; }

        public string PositionRemarks { get; set; }

        public string ReferredBy { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }
        public string ApplicantName { get; set; }
        public string PositionRemarks { get; set; }
        public string ReferredBy { get; set; }
    }
}
