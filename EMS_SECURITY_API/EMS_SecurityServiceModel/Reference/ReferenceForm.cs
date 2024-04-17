using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS_SecurityServiceModel.Reference
{
    public class ReferenceForm
    {
        public string ReferenceCode { get; set; }

        public List<ReferenceValue> ReferenceValues { get; set; }

    }
}
