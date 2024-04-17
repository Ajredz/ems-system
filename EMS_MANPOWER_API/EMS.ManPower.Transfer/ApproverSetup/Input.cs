using System.Collections.Generic;
using Utilities.API;

namespace EMS.Manpower.Transfer.ApproverSetup
{
    public class Form
    {
        public int ID { get; set; }
        public string OrgGroup { get; set; }
       
        public List<Approvers> Approvers { get; set; }
    }

    public class Approvers
    {
        public int PositionID { get; set; }
        public string Position { get; set; }
        public int HierarchyLevel { get; set; }
        public int ApproverPositionID { get; set; }
        public string ApproverPosition { get; set; }
        public int ApproverOrgGroupID { get; set; }
        public string ApproverOrgGroup { get; set; }
        public int AltApproverPositionID { get; set; }
        public string AltApproverPosition { get; set; }
        public int AltApproverOrgGroupID { get; set; }
        public string AltApproverOrgGroup { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public string OrgGroup { get; set; }
        public string HasApprover { get; set; }
        public string ModifiedDateFrom { get; set; }
        public string ModifiedDateTo { get; set; }
    }

}