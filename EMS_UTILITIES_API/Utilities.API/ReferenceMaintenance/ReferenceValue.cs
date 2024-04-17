namespace Utilities.API.ReferenceMaintenance
{
    public class ReferenceValue
    {
        public int ID { get; set; }

        public string RefCode { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public int UserID { get; set; }
    }
    public class MultiSelectedFilter
    {
        public string ID { get; set; }

        public string Description { get; set; }
    }
}