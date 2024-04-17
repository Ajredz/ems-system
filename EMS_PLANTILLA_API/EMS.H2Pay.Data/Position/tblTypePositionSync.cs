using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.H2Pay.Data.Position
{
    public class tblTypePositionSync
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

}