using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_PlantillaServiceModel.OrganizationalGroup
{
  // Table and Column names are all lower case to be mapped with MySQL 
  // objects which are case sensitive on Linux and case insensitive on Windows.
  [Table("organizational_group")]
  public class OrganizationalGroup

  {
    [Key]
    [Column("id")]
    public short ID { get; set; }
    [Column("code")]
    public string Code { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("region_id")]
    public short RegionID { get; set; }
    [Column("organizational_level")]
    public string OrganizationalLevel { get; set; }
    [Column("created_by")]
    public short CreatedBy { get; set; }
    [Column("created_date")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; set; }
    //public DateTime CreatedDate { get; set; }
    [Column("modified_by")]
    public Nullable<short> ModifiedBy { get; set; }
    [Column("modified_date")]
    public Nullable<DateTime> ModifiedDate { get; set; }
    [Column("company_id")]
    public short CompanyID { get; set; }
  }
}
