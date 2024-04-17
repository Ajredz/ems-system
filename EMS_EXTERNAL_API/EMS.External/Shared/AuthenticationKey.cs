using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.External.API.Shared
{
    [Table("tblAPIToken")]
    public class AuthenticationKey
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("token")]
        public string token { get; set; }
    }
}
