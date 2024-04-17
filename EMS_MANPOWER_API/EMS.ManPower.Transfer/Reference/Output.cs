using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Manpower.Transfer.Reference
{
    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetIsMaintainableOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
