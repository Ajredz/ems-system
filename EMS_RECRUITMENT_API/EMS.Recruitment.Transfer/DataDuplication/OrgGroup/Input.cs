using System.Collections.Generic;
using Utilities.API;

namespace EMS.Recruitment.Transfer.DataDuplication.OrgGroup
{
    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int RegionID { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetDropDownInput
    {
        public int ID { get; set; }

    }

    public class GetByOrgTypeAutoCompleteInput
    {
        public string Term { get; set; }
        public string OrgType { get; set; }
        public int TopResults { get; set; }
    }
}