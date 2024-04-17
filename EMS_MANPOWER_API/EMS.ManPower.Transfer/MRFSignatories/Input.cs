namespace EMS.Manpower.Transfer.MRFSignatories
{
    public class Form
    {
        public int ID { get; set; }

        public int WorkflowID { get; set; }

        public int WorkflowStepID { get; set; }

        public int WorkflowStepApproverID { get; set; }

        public int RequesterID { get; set; }

        public int PositionID { get; set; }

        public int PositionLevelID { get; set; }

        public int ApproverRoleID { get; set; }

        public string ApproverRoleName { get; set; }

        public string ApproverDescription { get; set; }

        public string WorkflowStepCode { get; set; }

        public int TATDays { get; set; }

        public int Order { get; set; }

    }

    public class GetByUserPositionInput
    {
        public int UserID { get; set; }
        public int PositionID { get; set; }
    }

    public class GetMRFSignatoriesAddInput
    {
        public int RecordID { get; set; }
        public int RequesterID { get; set; }
        public int PositionID { get; set; }
    }
}