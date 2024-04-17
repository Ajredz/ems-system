namespace EMS.Manpower.Transfer.DataDuplication.SystemRole
{
    public class Form
    {
        public int ID { get; set; }
        public string RoleName { get; set; }

        public short CompanyID { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
        public short CompanyID { get; set; }
    }
}