using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.IPM.Transfer
{
    public static class Enums
    {
        public enum ReferenceCodes
        {
            KPI_TYPE,
            KRA_TYPE,
            DASHBOARD,
            KPI_SOURCE_TYPE
        }

        public enum DashboardType
        {
            POSITION,
            BRANCH_WITH_POSITION,
            REGION_WITH_POSITION,
            FOR_APPROVAL,
            FOR_EVALUATION,
        }

        public enum EmployeeScore_ApproverStatus
        {
            FOR_APPROVAL,
            APPROVED,
            REJECTED,
            CANCELLED,
            FOR_REVISION
        }
    }
}
