using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Utilities.API
{
    public class JQGridResult
    {
        [NotMapped]
        public long TotalNoOfRecord { get; set; }

        [NotMapped]
        public long Row { get; set; }

        [NotMapped]
        public int NoOfPages { get; set; }

    }
}
