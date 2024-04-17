using Utilities.API;

namespace EMS.Plantilla.Transfer.Region
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}