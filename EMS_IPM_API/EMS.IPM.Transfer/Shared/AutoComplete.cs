using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.IPM.Transfer.Shared
{       
    public class GetAutoCompleteInput
    {
        public string Term { get; set; }

        public int TopResults { get; set; }
    }

    public class GetAutoCompleteOutput
    {
        public string ID { get; set; }

        public string Description { get; set; }
    }
}
