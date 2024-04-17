using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.DBObjects.NPRF
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    public class NPRF
    {
        public NPRFHeader NPRFHeader { get; set; }

        public List<NPRFPosition> NPRFPositionList { get; set; }

        public List<NPRFApprover> NPRFApproverList { get; set; }

    }


}
