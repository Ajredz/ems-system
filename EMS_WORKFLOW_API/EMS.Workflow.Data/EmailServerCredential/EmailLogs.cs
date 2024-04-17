using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.EmailServerCredential
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("email_logs")]
    public class EmailLogs
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }
        
        [Column("CreatedBy")]
        public string CreatedBy { get; set; }

        [Column("SenderName")]
        public string SenderName { get; set; }

        [Column("FromEmailAddress")]
        public string FromEmailAddress { get; set; }

        [Column("ToEmailAddress")]
        public string ToEmailAddress { get; set; }
        
        [Column("CCEmailAddress")]
        public string CCEmailAddress { get; set; }
        
        [Column("PositionTitle")]
        public string PositionTitle { get; set; }
        
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Status")]
        public string Status { get; set; }
        
        [Column("isSent")]
        public int isSent { get; set; }
        
        [Column("SentStatus")]
        public string SentStatus { get; set; }

        [Column("SentDate")]
        public DateTime? SentDate { get; set; }
        
        [Column("ErrorLog")]
        public string ErrorLog { get; set; }
        [Column("ErrorDate")]
        public DateTime? ErrorDate { get; set; }
        [Column("SystemCode")]
        public string SystemCode { get; set; }
        [Column("Subject")]
        public string Subject { get; set; }
        [Column("EmailBody")]
        public string EmailBody { get; set; }
        [Column("AttachmentName")]
        public string AttachmentName { get; set; }
        [Column("AttachmentLink")]
        public string AttachmentLink { get; set; }

    }
}
