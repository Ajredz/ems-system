using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.IPM.Transfer.Reference
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

    public class GetReferenceValueListOutput
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class GetReferenceValueOutput
    {
        public string Value { get; set; }
    }
}
