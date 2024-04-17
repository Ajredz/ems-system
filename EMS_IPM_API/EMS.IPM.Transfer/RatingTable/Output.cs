using System;
using Utilities.API;

namespace EMS.IPM.Transfer.RatingTable
{
    public class GetAllOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? MinScore { get; set; }
        public decimal? MaxScore { get; set; }
    }
}