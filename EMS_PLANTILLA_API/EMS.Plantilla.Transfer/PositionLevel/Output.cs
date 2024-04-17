using Utilities.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Plantilla.Transfer.PositionLevel
{
    public class GetListOutput : JQGridResult
    {
        public int UserID { get; set; }
        public int? ID { get; set; }
        public string Description { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}
