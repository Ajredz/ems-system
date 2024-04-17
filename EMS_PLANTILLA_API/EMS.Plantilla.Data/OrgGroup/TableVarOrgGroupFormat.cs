using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Plantilla.Data.OrgGroup
{
    [Table("tv_org_group_format")]
    public class TableVarOrgGroupFormat
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("result")]
        public string Result { get; set; }
    }

    [Table("tv_org_group_somd")]
    public class TableVarOrgGroupSOMD
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("clus")]
        public string Clus { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("reg")]
        public string Reg { get; set; }

        [Column("zone")]
        public string Zone { get; set; }
    }
}
