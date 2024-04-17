using System.Collections.Generic;
using Utilities.API;

namespace EMS.Plantilla.Transfer.Position
{
    public class Form
    {
        public int ID { get; set; }

        public int PositionLevelID { get; set; }

        public string PositionLevelDescription { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public int? ParentPositionID { get; set; }

        public string ParentPositionDescription { get; set; }

        public string JobClassCode { get; set; }
        public string JobClassDescription { get; set; }

        public int CreatedBy { get; set; }
        public string OnlinePosition { get; set; }
        public string OnlineLocation { get; set; }
        public string OnlineJobDescription { get; set; }
        public string OnlineJobQualification { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string PositionLevelIDs { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string ParentPositionDelimited { get; set; }

        public bool IsExport { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetDropdownByOrgGroupInput
    {
        public int OrgGroupID { get; set; }
        public int SelectedValue { get; set; }

    }
}