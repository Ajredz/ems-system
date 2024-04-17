using EMS.Manpower.Transfer.Shared;

namespace EMS.Manpower.Transfer.DataDuplication.Position
{
    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int PositionLevelID { get; set; }
        public int? ParentPositionID { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetDropDownInput
    {
        public int PositionLevelID { get; set; }
        public int SelectedValue { get; set; }

    }

    public class GetDropDownByParentPositionIDInput
    {
        public int ParentPositionID { get; set; }
        public int SelectedValue { get; set; }

    }

    public class GetDropdownByOrgGroupInput
    {
        public int OrgGroupID { get; set; }
        public int SelectedValue { get; set; }

    }
}