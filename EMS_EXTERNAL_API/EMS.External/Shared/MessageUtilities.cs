namespace EMS.External.API.Shared
{
    public static class MessageUtilities
    {
        // Success Messages
        public static readonly string SCSSMSG_REC_DELETE = "Records successfully deleted.";
        public static readonly string SCSSMSG_REC_CANCEL = "Successfully cancelled record(s).";
        public static readonly string SCSSMSG_REC_DISABLE = "Successfully disabled record(s).";
        public static readonly string SCSSMSG_REC_PROCESS = "Successfully processed record(s).";
        public static readonly string SCSSMSG_REC_FILE_UPLOAD = "Successfully uploaded.";
        public static readonly string SCSSMSG_REC_REJECT = "Successfully rejected record(s).";
        public static readonly string SCSSMSG_REC_RECALL = "Successfully recalled record(s).";
        public static readonly string SCSSMSG_REC_APPROVE = "Successfully approved record(s).";
        public static readonly string SCSSMSG_REC_DISAPPROVE = "Successfully disapproved record(s).";
        public static readonly string SCSSMSG_SECURITY_API_STATUS = "Security API is running successfully.";
        public static readonly string SCSSMSG_PLANTILLA_API_STATUS = "Plantilla API is running successfully.";
        public static readonly string SCSSMSG_MANPOWER_API_STATUS = "Manpower API is running successfully.";
        public static readonly string SCSSMSG_RECRUITMENT_API_STATUS = "Recruitment API is running successfully.";
        public static readonly string SCSSMSG_ONBOARDING_API_STATUS = "Onboarding API is running successfully.";
        public static readonly string SCSSMSG_WORKFLOW_API_STATUS = "Workflow API is running successfully.";
        public static readonly string SCSSMSG_IPM_API_STATUS = "Individual Performance Management API is running successfully.";
        public static readonly string SCSSMSG_REC_FILE_UPDATE = "Successfully updated.";

        // Single record success messages
        public static readonly string SCSSMSG_REC_SAVE = "Record successfully saved.";
        public static readonly string SCSSMSG_REC_COMPLETE = "Action completed successfully.";
        public static readonly string SCSSMSG_REC_UPDATE = "Record successfully updated.";
        public static readonly string SSCSSMSG_REC_DELETE = "Record successfully deleted.";
        public static readonly string SSCSSMSG_REC_CANCEL = "Successfully cancelled record.";
        public static readonly string SSCSSMSG_REC_APPROVE = "Successfully approved record.";
        public static readonly string SSCSSMSG_REC_REJECT = "Successfully rejected record.";
        public static readonly string SSCSSMSG_REC_RECALL = "Successfully recalled record.";
        public static readonly string SSCSSMSG_REC_PROCESS = "Successfully processed record.";
        public static readonly string SSCSMSG_RESET_PASS = "Successfully reset password.";
        public static readonly string SSCSMSG_CHANGED_PASS = "Password successfully changed.Please Login again using your new password.";
        
        // Success Messages Prefix
        public static readonly string PRE_SCSSMSG_REC_SAVE = "Record successfully saved.";
        public static readonly string PRE_SCSSMSG_REC_UPDATE = "Record successfully saved.";
        public static readonly string PRE_SCSSMSG_REC_DELETE = "Record(s) successfully deleted.";
        public static readonly string PRE_SCSSMSG_REC_FILE_UPLOAD = "Successfully uploaded.";
        public static readonly string PRE_SCSSMSG_REC_LOCK = "Successfully locked record(s).";
        public static readonly string PRE_SCSSMSG_REC_UNLOCK = "Successfully unlocked record.";
        public static readonly string PRE_SCSSMSG_REC_ADDED = "Record(s) successfully added.";

        // Success Messages Suffix
        public static readonly string SUFF_SCSSMSG_REC_SAVE = "successfully added.";
        public static readonly string SUFF_SCSSMSG_REC_UPDATE = "successfully updated.";
        public static readonly string SUFF_SCSSMSG_REC_CLEAR = "successfully cleared.";
        public static readonly string SUFF_SCSSMSG_REC_DELETE = "successfully deleted.";
        public static readonly string SUFF_SCSSMSG_REC_FILE_UPLOAD = "successfully uploaded.";
        public static readonly string SUFF_SCSSMSG_REC_SYNCED = "successfully synced.";
        public static readonly string SUFF_SCSSMSG_REC_SYNCED_UPDATED = "records are up to date.";

        // Error Messages
        public static readonly string ERRMSG_EXCEPTION = "Oops something went wrong. Please contact your system administrator.";
        public static readonly string ERRMSG_EXCEPTION_WITH_REFNO = "Oops something went wrong. Please contact your system administrator. Reference no. ";
        public static readonly string ERRMSG_EXCEPTION_WITH_REFNO_PREFIX = "A problem has occurred on this website. Please try again. if this continues, please contact support. Reference no. ";
        public static readonly string ERRMSG_NOACCESS = "You have no permission to access this page.";
        public static readonly string ERRMSG_PASSWORD_VALIDATION = "Password must contain at least one digit, one lower case letter, one uppercase letter, and length is between 8 and 15.";
        public static readonly string ERRMSG_NO_PRODUCT_KEY = "No product key. Please contact your system administrator.";
        public static readonly string ERRMSG_INVALID_FILE = "File format is invalid.";
        public static readonly string ERRMSG_REC_SAVE = "Failed to add new record(s).";
        public static readonly string ERRMSG_REC_UPDATE = "Failed to update record(s).";
        public static readonly string ERRMSG_REC_DELETE = "Failed to delete record(s).";
        public static readonly string ERRMSG_REC_PROCESS = "An error occur. Failed to process record(s).";
        public static readonly string ERRMSG_REC_NOT_EXIST = "Record does not exist.";
        public static readonly string ERRMSG_LOGIN_SESSION = "Your session has expired. Please login again.";
        public static readonly string ERRMSG_FILE_NOTFOUND = "File not found.";
        public static readonly string ERRMSG_INCORRECT_LOGIN = "Username and/or Password is incorrect.";
        public static readonly string ERRMSG_INCORRECT_SSO_KEY = "Incorrect SSO Key.";
        public static readonly string ERRMSG_NO_RECORDS = "No records found.";
        public static readonly string ERRMSG_NO_CHANGES = "No change has been made.";
        public static readonly string ERRMSG_SECURITY_API_STATUS = "Website is under maintenance.";
        public static readonly string ERRMSG_PLANTILLA_API_STATUS = "Onboarding API is under maintenance.";
        public static readonly string ERRMSG_MANPOWER_API_STATUS = "Manpower API is under maintenance.";
        public static readonly string ERRMSG_RECRUITMENT_API_STATUS = "Workflow API is under maintenance.";
        public static readonly string ERRMSG_ONBOARDING_API_STATUS = "Onboarding API is under maintenance.";
        public static readonly string ERRMSG_WORKFLOW_API_STATUS = "Workflow API is under maintenance.";
        public static readonly string ERRMSG_IPM_API_STATUS = "Individual Performance Management API is under maintenance.";
        public static readonly string ERRMSG_MRF_REQUEST_EXCEED = "MRF request exceeded the existing budget for this position.";
        public static readonly string ERRMSG_NO_REC_EXPORT = "No records to Export.";
        public static readonly string ERRMSG_ALLOWED_APP_MRF = "Only 1 Applicant is allowed to be hired for this Request.";
        public static readonly string ERRMSG_CURRENT_NOT_MATCHED = "Incorrect Current Password.";
        public static readonly string ERRMSG_NEW_CONFIRM_NOT_MATCHED = "New Password and Confirm Password does not match.";
        public static readonly string ERRMSG_NEW_PASS_NOT_EQUAL_DEFAULT = "New Password must not be equal to the default password.";
        public static readonly string ERRMSG_NEW_PASS_MIN = "New Password must be at least 8 characters.";
        public static readonly string ERRMSG_DUPLICATE_APPLICANT = "Duplicate Applicants:";
        public static readonly string ERRMSG_DUPLICATE_EMPLOYEE = "Duplicate Employees:";
        public static readonly string ERRMSG_KPI_SCORES = "Duplicate Scores:";
        public static readonly string ERRMSG_KPI_RUN_NO_EMP = "No employee score generated";

        // Error Message Prefix        
        public static readonly string PRE_ERRMSG_REC_VALID = "Please provide valid";
        public static readonly string PRE_ERRMSG_REC_SAVE = "Failed to add record.";
        public static readonly string PRE_ERRMSG_REC_UPDATE = "Failed to update record.";
        public static readonly string PRE_ERRMSG_REC_DELETE = "Failed to delete record.";
        public static readonly string PRE_ERRMSG_REC_CANCEL = "Failed to cancel record.";
        public static readonly string PRE_ERRMSG_NUMERIC = "Please provide numeric ";
        // Error Messages Suffix
        public static readonly string SUFF_ERRMSG_REC_EXISTS = "already exists.";
        public static readonly string SUFF_ERRMSG_REC_INUSE = "is being used.";
        public static readonly string SUFF_ERRMSG_REC_PROC = "is being processed.";
        public static readonly string SUFF_ERRMSG_REC_NOT_EXISTS = "does not exists.";
        public static readonly string SUFF_ERRMSG_INPUT_REQUIRED = "is required.";
        public static readonly string SUFF_ERRMSG_INPUT_NOSPACE = "must have no space.";
        public static readonly string SUFF_ERRMSG_INPUT_JS_INJECTION = "contains the symbol '<' with a succeeding character. Please add space after the '<' symbol.";
        public static readonly string SUFF_ERRMSG_INCORRECT = "is incorrect.";
        public static readonly string SUFF_ERRMSG_INACTIVE = "is inactive.";
        public static readonly string SUFF_ERRMSG_MAX_FILE_SIZE = "cannot be more than 1 MB.";
        public static readonly string SUFF_ERRMSG_MAX_FILE_SIZE_IMAGE = "total size cannot be more than 10 MB.";
        public static readonly string SUFF_ERRMSG_INVALID = "is invalid.";
        // Comparisons
        public static readonly string COMPARE_GREATER_EQUAL = " must be greater than or equal to ";
        public static readonly string COMPARE_GREATER = " must be greater than ";
        public static readonly string COMPARE_BETWEEN = " must be between ";
        public static readonly string COMPARE_EQUAL_LESS = " must be less than or equal to ";
        public static readonly string COMPARE_NOT_EXCEED = " must not exceed ";
        public static readonly string COMPARE_ALREADY_EXIST = " already exists in Row No. ";
        public static readonly string COMPARE_INVALID_DATE = " is an invalid date. ";
        public static readonly string COMPARE_INVALID_AMOUNT = " is an invalid amount. ";
        public static readonly string COMPARE_INVALID = " is invalid.";

        // Regex Error Messages
        public static readonly string ERRMSG_REGEX_CODE = "Code must be Alphanumeric only and can have underscore '_'.";

        //Organizational Group Messages
        public static readonly string ERRMSG_TOP_LEVEL_EXIST = "There can be only one top level organizational group";

        //
        public static readonly string ERRMSG_APPLICANT_IS_USED = "Failed to delete record, Applicant is tagged on an MRF Application";

        //Login Message
        public static readonly string ERRMSG_USER_ACCOUNT_INACTIVE = "Login failed. This user account is disabled.";

        public static readonly string ERRMSG_INVALID_TOKEN = "Unable to access API.";

    }
}
