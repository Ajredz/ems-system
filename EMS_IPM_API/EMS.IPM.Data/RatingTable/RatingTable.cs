using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.RatingTable
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("rating_table")]
    public class RatingTable
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column("min_score")]
        public decimal? MinScore { get; set; }
        
        [Column("max_score")]
        public decimal? MaxScore { get; set; }
        
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}