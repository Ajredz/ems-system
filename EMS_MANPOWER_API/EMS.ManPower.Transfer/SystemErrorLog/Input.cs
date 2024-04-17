using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Manpower.Transfer.SystemErrorLog
{
    public class Form
    {
        public long ID { get; set; }
        public string Layer { get; set; }
        public string Method { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerException { get; set; }
        public string User { get; set; }
        public int UserID { get; set; }
        public string CreatedDate { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Method { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
        public string UserIDDelimited { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public bool IsExport { get; set; }
        public string ReportType { get; set; }
    }
}
