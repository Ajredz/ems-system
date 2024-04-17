using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Manpower.Transfer.ApproverSetup
{
    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string OrgGroup { get; set; }
        public string HasApprover { get; set; }
        public string LastModifiedDate { get; set; }
    }

    public class MRFDefinedApproverOutput
    {
        public int ID { get; set; }
        public int HierarchyLevel { get; set; }
        public int RequestingPositionID { get; set; }
        public int RequestingOrgGroupID { get; set; }
        public int ApproverPositionID { get; set; }
        public int ApproverOrgGroupID { get; set; }
        public int? AltApproverPositionID { get; set; }
        public int? AltApproverOrgGroupID { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
