using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_FrontEnd.Models.Security
{
    public class SystemUserLogs
    {
        public string LastLoggedIn { get; set; }
        public string LastLoggedOut { get; set; }
        public string LastPasswordChage { get; set; }

    }
}
