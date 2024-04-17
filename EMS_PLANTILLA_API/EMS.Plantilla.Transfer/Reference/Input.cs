using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Plantilla.Transfer.Reference
{
    public class GetAutoCompleteInput
    {
        public string RefCode { get; set; }
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetByRefCodeValueInput
    {
        public string RefCode { get; set; }
        public string Value { get; set; }
    }
}
