using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Plantilla.Transfer
{
    public static class Enums
    {
        public enum ReferenceCodes
        {
            ORGGROUPTYPE,
            ORGGROUPCATEGORY,
            COMPANY_TAG,
            REGION_TAG,
            ORG_BRN_TAGS,
            ORG_DEPT_TAGS,
            OPER_GROUP_TAG,
            ORGLIST_FILTER,
            EMPLOYMENT_STATUS,
            FAMILY_RELATIONSHIP,
            REPORT_TYPE,
            COMPANY_CODE_NUMBER,
            EMP_CODE_COUNTER,
            MOVEMENT_EMP_FIELD,
            MOVEMENT_TYPE,
            EMP_NATIONALITY,
            EMP_CITIZENSHIP,
            EMP_CIVIL_STATUS,
            EMP_RELIGION,
            EMP_SCHOOL_LEVEL,
            EMP_ED_ATT_DEG,
            EMP_ED_ATT_STAT,
            JOB_CLASS,
            EMP_SSS_STAT,
            EMP_EXEMPT_STAT,
            EMP_GENDER,
            WAGE_DAILY_DIVISOR,
            WAGE_HOURLY_DIVISOR,
            UPDATEPROFILE,
            UPDATEPROFILETAGS
        }

        public enum EMPLOYMENT_STATUS
        {

            PROBATIONARY,
            PROBATIONARY_PROM,
            REGULAR,
            RESIGNED,
            OUTGOING,
            AWOL,
            TERMINATED,
            DECEASED,
            BACKOUT
        }

        public enum FAMILY_RELATIONSHIP
        {
            AUNT,
            BROTHER,
            COUSIN,
            DAUGTHER,
            FATHER,
            GRANDFATHER,
            GRANDMOTHER,
            MOTHER,
            SISTER,
            SON,
            SPOUSE,
            UNCLE
        }

        public enum MOVEMENT_EMP_FIELD
        {
            POSITION,
            ORG_GROUP,
            EMPLOYMENT_STATUS,
            COMPANY,
            COMPENSATION,
            SECONDARY_DESIG,
            OTHERS,
            CIVIL_STATUS,
            EXEMPTION_STATUS
        }

    }
}
