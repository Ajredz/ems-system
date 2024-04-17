using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.EmailServerCredential
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("email_server_credential")]
    public class EmailServerCredential
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("template_code")]
        public string TemplateCode { get; set; }
        
        [Column("subject")]
        public string Subject { get; set; }
        
        [Column("body")]
        public string Body { get; set; }
        
        [Column("sender_display_name")]
        public string SenderDisplayName { get; set; }
        
        [Column("sender_email")]
        public string SenderEmail { get; set; }
        
        [Column("sender_username")]
        public string SenderUsername { get; set; }
        
        [Column("sender_password")]
        public string SenderPassword { get; set; }
        
        [Column("host")]
        public string Host { get; set; }
        
        [Column("port")]
        public int Port { get; set; }

        [Column("enable_ssl")]
        public bool EnableSSL { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; }
        
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

    }
}
