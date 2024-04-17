using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Recruitment.Transfer
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

        public enum ApproverResponseEnum
        {
            PASS,
            FAIL,
            APPROVE,
            REJECT,
            SKIP,
            ACCEPT,
            DECLINE
        }

        public enum TaskStatus
        {
            OPEN,
            CLOSED
        }
    }
}
