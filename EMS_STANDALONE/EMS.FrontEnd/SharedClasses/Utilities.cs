using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses
{

    public static class UtilitiesStatic
    {
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int batchSize)
        {
            var batch = new List<TSource>();
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<TSource>();
                }
            }

            if (batch.Any()) yield return batch;
        }
    }

    public class Utilities : Security
    {
        public IWebHostEnvironment _env;
        public bool _IsAdminAccess = false;

        public Utilities(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration)
        {
            new ErrorLog(
            "FrontEnd"
            , _iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            _env = env;
        }

        public enum ReferenceCodes_SystemAccess
        {
            ON_MENU_LEVEL,
            URL_FUNCTION_TYPE,
            EMPLOYMENT_NATURE,
            PURPOSE,
            RESULT_TYPE,
            PASS_FAIL,
            APPROVE_REJECT,
            PASS_FAIL_SKIP,
            APPROVE_REJECT_SKIP,
            ORG_REGROUP_INPUT
        }

        public enum ReferenceCodes_PlantillaReference
        {
            ORG_GROUP,
            BRANCH_TRANSFER,
            BOMD_INPUT_TAGS
        }

        //public enum ReferenceCodes_Plantilla
        //{
        //    COMPANY_TAG,
        //    ORG_BCH_TAGS,
        //    ORGGROUPTYPE,
        //    REGION_TAG,
        //    ORGLIST_FILTER,
        //    EMPLOYMENT_STATUS,
        //    FAMILY_RELATIONSHIP,
        //    REPORT_TYPE,
        //    COMPANY_CODE_NUMBER,
        //    EMP_CODE_COUNTER,
        //    MOVEMENT_TYPE,
        //    EMP_NATIONALITY,
        //    EMP_CITIZENSHIP,
        //    EMP_CIVIL_STATUS,
        //    EMP_RELIGION,
        //    EMP_SCHOOL_LEVEL,
        //    EMP_ED_ATT_DEG,
        //    EMP_ED_ATT_STAT,
        //    JOB_CLASS,
        //    EMP_SSS_STAT,
        //    EMP_EXEMPT_STAT,
        //    EMP_GENDER,
        //    WAGE_DAILY_DIVISOR,
        //    WAGE_HOURLY_DIVISOR
        //}

        public enum ReferenceCodes_Manpower
        {
            SYSTEM_MODULE,
            APPROVE_REJECT,
            APPROVE_REJECT_SKIP,
            PASS_FAIL,
            PASS_FAIL_SKIP,
            RESULT_TYPE,
            NATURE_OF_EMPLOYMENT,
            MRF_PURPOSE,
            REQUEST_TYPE,
            MRF_STATUS,
            MRF_TRANSID_COUNTER,
            MRF_TRANSID_PREFIX,
            ORGLIST_FILTER,
            COMPANY_TAG
        }

        public enum ReferenceCodes_Recruitment
        {
            SYSTEM_MODULE,
            APPROVE_REJECT,
            APPROVE_REJECT_SKIP,
            PASS_FAIL,
            PASS_FAIL_SKIP,
            RESULT_TYPE,
            NATURE_OF_EMPLOYMENT,
            APPLICATION_SOURCE,
            GEOGRAPHICAL_REGION,
            REQUEST_TYPE,
            ATTACHMENT_TYPE,
            TASK_STATUS,
            ORGLIST_FILTER,
            COURSE,
            LEGAL_PROFILE
        }

        public enum ReferenceCodes_Workflow
        {
            ACTIVITY_MODULE,
            ACTIVITY_STATUS,
            ACTIVITY_TYPE,
            TASK_TYPE,
            APPR_ACTIVITY_STATUS,
            ACTIVITY_STAT_FILTER,
            APPROVE_REJECT,
            APPROVE_REJECT_SKIP,
            DONE_PENDING,
            NEGATIVE_RESULT_TYPE,
            PASS_FAIL,
            PASS_FAIL_SKIP,
            RESULT_TYPE,
            YES_NO,
            ACCOUNTABILITY_TYPE,
            ACCNTABILITY_STATUS,
            EMAIL
        }

        public enum ReferenceCodes_SystemRole
        {
            SYSTEM_ROLE,
            CLEARANCE_APPROVER
        }

        public enum ReferenceCodes_Email
        {
            BODY_MRF_SEND_EMAIL,
            CAREER_SENDER_NAME,
            CAREER_SENDER_EMAIL,
            CAREER_CONTACT_NUMBER,
            SENDER_EMAIL,
            CHANGE_STATUS,
            CLEARANCE_BODY,
            CLEARANCE_FORM,
            BIRTHDAY_BODY,
            EVALUATION_BODY
        }

        public enum ReferenceCodes_IPM
        {
            KPI_TYPE,
            KPI_SOURCE_TYPE
        }


        public enum MRF_STATUS
        {
            OPEN,
            CLOSED,
            CANCELLED,
            AUTO_CANCELLED,
            REJECTED,
            HR_CANCELLED,
            FOR_APPROVAL
        }

        public enum MRF_APPROVER_STATUS
        {
            FOR_APPROVAL,
            APPROVED,
            REJECTED,
            CANCELLED
        }

        public enum ReferenceCodes_ReportType
        {
            PRO_EMP_PER_GROUP,
            PRO_EMP_PER_POSITION,
            PRO_STATUS_BEYOND_6,
            PRO_STATUS_EXPIRING,
            BIRTHDAY_PER_MONTH,
            ACT_EMP_PER_GROUP,
            ACT_EMP_PER_POSITION,
            INACTIVE_EMPLOYEE
        }

        public enum FormUtil
        {
            OrgGroupForm = 1
        }

        public async Task<(bool, string)> TestConnectionPlantilla(int userID)
        {
            try
            {
                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new object(),
                        string.Concat(_plantillaBaseURL,
                        _iconfiguration.GetSection("Plantilla_TestConnection").Value, "?",
                        "userid=", userID));
                return (IsSuccess, IsSuccess ? Message : MessageUtilities.ERRMSG_PLANTILLA_API_STATUS);
            }
            catch
            {
                return (false, MessageUtilities.ERRMSG_PLANTILLA_API_STATUS);
            }
        }

        public async Task<(bool, string)> TestConnectionManpower(int userID)
        {
            try
            {
                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new object(),
                       string.Concat(_manpowerBaseURL,
                       _iconfiguration.GetSection("Manpower_TestConnection").Value, "?",
                       "userid=", userID));
                return (IsSuccess, IsSuccess ? Message : MessageUtilities.ERRMSG_MANPOWER_API_STATUS);
            }
            catch
            {
                return (false, MessageUtilities.ERRMSG_MANPOWER_API_STATUS);
            }
        }

        public async Task<(bool, string)> TestConnectionRecruitment(int userID)
        {
            try
            {
                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new object(),
                        string.Concat(_recruitmentBaseURL,
                        _iconfiguration.GetSection("Recruitment_TestConnection").Value, "?",
                        "userid=", userID));
                return (IsSuccess, IsSuccess ? Message : MessageUtilities.ERRMSG_RECRUITMENT_API_STATUS);
            }
            catch
            {
                return (false, MessageUtilities.ERRMSG_RECRUITMENT_API_STATUS);
            }
        }

        public async Task<(bool, string)> TestConnectionWorkflow(int userID)
        {
            try
            {
                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new object(),
                        string.Concat(_workflowBaseURL,
                        _iconfiguration.GetSection("Workflow_TestConnection").Value, "?",
                        "userid=", userID));
                return (IsSuccess, IsSuccess ? Message : MessageUtilities.ERRMSG_WORKFLOW_API_STATUS);
            }
            catch
            {
                return (false, MessageUtilities.ERRMSG_WORKFLOW_API_STATUS);
            }
        }

        public async Task<(bool, string)> TestConnectionIPM(int userID)
        {
            try
            {
                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new object(),
                        string.Concat(_ipmBaseURL,
                        _iconfiguration.GetSection("IPM_TestConnection").Value, "?",
                        "userid=", userID));
                return (IsSuccess, IsSuccess ? Message : MessageUtilities.ERRMSG_IPM_API_STATUS);
            }
            catch
            {
                return (false, MessageUtilities.ERRMSG_IPM_API_STATUS);
            }
        }

        public static async Task CopyToServerPath(string path, IFormFile file, string ServerFileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using var fileStream = new FileStream(Path.Combine(path, ServerFileName), FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        public static void DeleteFileFromServer(string path, string serverFileName)
        {
            var filePath = Path.Combine(path, serverFileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        public IActionResult OnGetCheckFileIfExists(string repoPath, string ServerFile)
        {
            var filePath = Path.Combine(_env.WebRootPath, _iconfiguration.GetSection(repoPath).Value, ServerFile);
            var serverPath = Path.Combine(_iconfiguration.GetSection(repoPath).Value, ServerFile);
            if (System.IO.File.Exists(filePath))
            {
                _resultView.IsSuccess = true;
            }
            else if (System.IO.File.Exists(serverPath))
            {
                _resultView.IsSuccess = true;
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = MessageUtilities.ERRMSG_FILE_NOTFOUND;
            }

            return new JsonResult(_resultView);
        }

        public IActionResult OnGetDownloadFile(string repoPath, string serverFile, string sourceFile)
        {
            string filePath = Path.Combine(_env.WebRootPath, _iconfiguration.GetSection(repoPath).Value, serverFile);
            byte[] data = System.IO.File.ReadAllBytes(filePath);
            _resultView.IsSuccess = true;
            if (_resultView.IsSuccess)
                return File(data, "application/force-download", sourceFile);
            else
            {
                _resultView.Result = MessageUtilities.ERRMSG_FILE_NOTFOUND;
                return new JsonResult(_resultView);
            }
        }

        public IActionResult OnGetDownloadTemplate(int ID)
        {
            string filename = GetFormFilename(ID);
            string filePath = Path.Combine(GetFormFilePath(ID), filename);
            byte[] data = System.IO.File.ReadAllBytes(filePath);

            return File(data, "application/force-download", filename);
        }

        public string GetFormFilePath(int form)
        {
            // Path where contains all forms
            string result = "";
            FormUtil uploadPathUtil = (FormUtil)form;

            switch (uploadPathUtil)
            {
                case FormUtil.OrgGroupForm:
                    result = string.Concat(_env.WebRootPath, "/Recruitment/Forms");
                    break;
                default:
                    break;
            }

            return result;
        }

        public static string GetFormFilename(int form)
        {
            // Filename of form
            string result = "";
            FormUtil uploadPathUtil = (FormUtil)form;

            switch (uploadPathUtil)
            {
                case FormUtil.OrgGroupForm:
                    result = "Organizational_Group.xlsx";
                    break;
                default:
                    break;

            }

            return result;
        }

        public static string GetFormUploadFilename(FormUtil formFilename, string filename)
        {
            string result = "";
            string guid = "";

            result = DateTime.Now.ToString("yyyyMMddHHmmss");
            guid = Guid.NewGuid().ToString("N").Substring(0, 4);

            switch (formFilename)
            {
                case FormUtil.OrgGroupForm:
                    result = "Organizational_Group" + result + guid;
                    break;
                default:
                    break;
            }

            result = result.ToUpper() + "_" + filename;

            return result;
        }

        public static string GetComputerNameByIP(string IP)
        {
            string computerName;
            try
            {
                computerName = System.Net.Dns.GetHostEntry(IP).HostName;
            }
            catch
            {
                computerName = "N/A";
            }
            return computerName;
        }
    }
}