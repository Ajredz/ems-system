using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Manpower.Transfer
{
    public static class Enums
    {
        public enum ResultType
        {
            PASS_FAIL,
            APPROVE_REJECT,
            PASS_FAIL_SKIP,
            APPROVE_REJECT_SKIP,
        }

        public enum MRF_APPROVER_STATUS
        {
            FOR_APPROVAL,
            APPROVED,
            REJECTED,
            CANCELLED
        }

        public enum MRFReference
        {
            APPROVE_REJECT,
            APPROVE_REJECT_SKIP,
            PASS_FAIL,
            PASS_FAIL_SKIP,
            RESULT_TYPE,
            NATURE_OF_EMPLOYMENT,
            MRF_PURPOSE,
            REQUEST_TYPE,
            MRF_STATUS,
            SETUP_MRF_APP_LEVEL
        }

        public enum MRF_STATUS
        {
            OPEN,
            CLOSED,
            CANCELLED,
            REJECTED,
            HR_CANCELLED,
            FOR_APPROVAL
        }

        public enum WorkflowStatus
        {
            COMPLETED,
            IN_PROGRESS,
            FAILED
        }
    }
}
