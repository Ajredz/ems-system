using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.

    //API FOR GET ONLINE MRF
    public class TableVarMRFOnline
    {
        public int ID { get; set; }
        public int PositionID { get; set; }
        public string OnlinePosition { get; set; }
        public string OnlineLocation { get; set; }
        public string OnlineJobDescription { get; set; }
        public string OnlineJobQualification { get; set; }
        public string ClosedDate { get; set; }
        public int ApplicantCount { get; set; }
        public string MrfCreatedDate { get; set; }

    }
}