using System.Collections.Generic;

namespace Utilities.API
{
    public class APICredentials
    {
        public int UserID { get; set; }
    }

    public class EmailForm
    {
        public string subject { get; set; }
        public string body { get; set; }
        public string template { get; set; }
        public List<File> file { get; set; }
        public List<EmailDetails> from { get; set; }
        public List<EmailDetails> to { get; set; }
        public List<EmailDetails> cc { get; set; }
        public List<EmailDetails> bcc { get; set; }
    }
    public class EmailDetails
    {
        public string email { get; set; }
        public string name { get; set; }
    }
    public class File
    {
        public string path { get; set; }
        public string name { get; set; }
    }

    public class ChangeStatus
    {
        public List<long> ID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}