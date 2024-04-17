using Utilities.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.IPM.Transfer.KRAGroup
{
    public class GetListOutput : JQGridResult
    {
        public int UserID { get; set; }
        public int? ID { get; set; }
        public string Description { get; set; }
    }

    public class GetAllKRAGroupOutput
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
