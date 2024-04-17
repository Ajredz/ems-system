using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Manpower.Transfer.MRFApproval
{
    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string MRFID { get; set; }
        public string OrgGroupDescription { get; set; }
        public string PositionLevelDescription { get; set; }
        public string PositionDescription { get; set; }
        public string NatureOfEmployment { get; set; }
        public int NoOfApplicant { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string ApprovedDate { get; set; }
        public int Age { get; set; }
    }
}
