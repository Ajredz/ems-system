using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Workflow.Transfer.Reference
{
    public class GetAutoCompleteInput
    {
        public string RefCode { get; set; }
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetByRefCodeAndValuePrefixInput
    {
        public string RefCode { get; set; }
        public string ValuePrefix { get; set; }
    }

    public class GetByRefCodeValueInput
    {
        public string RefCode { get; set; }
        public string Value { get; set; }
    }
}
