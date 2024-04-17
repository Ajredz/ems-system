namespace EMS.IPM.Transfer.DataDuplication.Position
{
    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int PositionLevelID { get; set; }
        public int ParentPositiondID { get; set; }
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
}