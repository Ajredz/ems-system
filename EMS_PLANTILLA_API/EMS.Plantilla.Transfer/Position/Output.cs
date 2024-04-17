using Utilities.API;

namespace EMS.Plantilla.Transfer.Position
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string PositionLevelDescription { get; set; }
        
        public string Code { get; set; }

        public string Title { get; set; }

        public string ParentPositionDescription { get; set; }
    }

    public class GetDropDownOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}