using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Recruitment.Transfer.ApplicantTagging
{
    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicationSource { get; set; }

        public string PositionRemarks { get; set; }

        public string ReferredBy { get; set; }
    }
}
