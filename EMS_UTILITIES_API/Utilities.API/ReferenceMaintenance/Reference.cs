namespace Utilities.API.ReferenceMaintenance
{
    public class Reference
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsMaintainable { get; set; }

        public int UserID { get; set; }
    }
}