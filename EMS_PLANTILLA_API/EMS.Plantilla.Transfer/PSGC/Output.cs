using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Plantilla.Transfer.PSGC
{
    public class GetRegionAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetPSGCValueOutput
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class GetAllPSGCValueOutput
    {
        public string Code { get; set; }
        public string ParentPrefix { get; set; }
        public string Prefix { get; set; }
        public string Description { get; set; }
    }
}
