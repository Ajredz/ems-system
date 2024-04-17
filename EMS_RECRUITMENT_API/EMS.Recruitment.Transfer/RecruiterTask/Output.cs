using Utilities.API;

namespace EMS.Recruitment.Transfer.RecruiterTask
{
    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string Recruiter { get; set; }
        public string Applicant { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class GetPendingListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string Applicant { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
