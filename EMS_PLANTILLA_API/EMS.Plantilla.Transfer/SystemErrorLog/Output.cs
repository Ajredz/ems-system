using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Plantilla.Transfer.SystemErrorLog
{
    public class GetListOutput : JQGridResult
    {
        public long? ID { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
        public string User { get; set; }
        public int UserID { get; set; }
        public string CreatedDate { get; set; }
    }
}
