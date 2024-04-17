using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Security.Transfer.AuditLog
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string DateCreated { get; set; }
    }

    public class GetEventTypeByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetTableNameByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}
