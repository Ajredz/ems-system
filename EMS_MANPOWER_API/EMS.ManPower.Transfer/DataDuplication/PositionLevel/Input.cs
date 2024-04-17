namespace EMS.Manpower.Transfer.DataDuplication.PositionLevel
{
    public class Form
    {
        public int ID { get; set; }

        public string Description { get; set; }

    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetByOrgGroupIDInput
    {
        public int OrgGroupID { get; set; }
        public int SelectedValue { get; set; }
    }
}