using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Security.Transfer.AuditLog
{
    public class Form
    {
        public string EventType { get; set; }
        public string TableName { get; set; }
        public long TableID { get; set; }
        public string Remarks { get; set; }
        public string IPAddress { get; set; }
        public bool IsSuccess { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string EventTypeDelimited { get; set; }
        public string TableNameDelimited { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
        public string Filter { get; set; }
    }
}
