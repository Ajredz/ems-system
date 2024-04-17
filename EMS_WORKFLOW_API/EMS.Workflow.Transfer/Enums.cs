using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Workflow.Transfer
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

        public enum ActivityStatus
        {
            NEW,
            CANCELLED,
            WIP,
            DONE
        }

        public enum ActivityModule
        {
            RECRUITMENT,
            ONBOARDING,
            EXIT_MANAGEMENT,
            MOVEMENT,
            GENERAL
        }

        public enum AccountabilityStatus
        {
            NEW,
            ACCEPTED,
            FOR_CLEARANCE,
            CLEARED,
            CANCELLED
        }

        public enum ReferenceCodes
        {
            ACCOUNTABILITY_TYPE,
            ACTIVITY_TYPE
        }
    }
}
