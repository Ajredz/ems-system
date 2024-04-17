using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.PSGC
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("psgc_barangay")]
    public class PSGCBarangay
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column("region_prefix")]
        public string RegionPrefix { get; set; }

        [Column("province_prefix")]
        public string ProvincePrefix { get; set; }

        [Column("city_mun_prefix")]
        public string CityMunicipalityPrefix { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}