using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Recruitment.Transfer.Reference
{
    public class GetAutoCompleteInput
    {
        public string RefCode { get; set; }
        public string Term { get; set; }
        public int TopResults { get; set; }
    }
}
