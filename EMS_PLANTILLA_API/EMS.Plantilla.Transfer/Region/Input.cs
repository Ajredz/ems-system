using Utilities.API;

namespace EMS.Plantilla.Transfer.Region
{
    public class Form
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
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
}