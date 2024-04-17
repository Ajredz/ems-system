using Utilities.API;

namespace EMS.Plantilla.Transfer.PositionLevel
{
    public class Form
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int CompanyID { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Description { get; set; }
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

    public class GetByPositionLevelIDInput
    {
        public int OrgGroupID { get; set; }
        public int SelectedValue { get; set; }
    }
}