using System.Collections.Generic;
using Utilities.API;

namespace EMS.Recruitment.Transfer.RecruiterTask
{
    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Recruiter { get; set; }
        public string Applicant { get; set; }
        public string Description { get; set; }
        public string StatusDelimited { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public string DateModifiedFrom { get; set; }
        public string DateModifiedTo { get; set; }
    }

    public class GetPendingListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Applicant { get; set; }
        public string Description { get; set; }
        public string StatusDelimited { get; set; }
        public string DateCreatedFrom { get; set; }
        public string DateCreatedTo { get; set; }
        public string DateModifiedFrom { get; set; }
        public string DateModifiedTo { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }
        public int RecruiterID { get; set; }
        public int ApplicantID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
    }

    public class BatchForm
    {
        public List<int> IDs { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
