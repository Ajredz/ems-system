namespace EMS.IPM.Transfer.DataDuplication.PSGCRegion
{
    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int EmployeeLevelID { get; set; }
        public int ParentEmployeedID { get; set; }
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