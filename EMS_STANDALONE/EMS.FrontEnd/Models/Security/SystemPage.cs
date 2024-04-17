using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_FrontEnd.Models.Security
{
    public class SystemPage
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public int ParentPageID { get; set; }

        public string OnMenuLevel { get; set; }

        public bool IsHidden { get; set; }
        public short ChildOrder { get; set; }
    }
}
