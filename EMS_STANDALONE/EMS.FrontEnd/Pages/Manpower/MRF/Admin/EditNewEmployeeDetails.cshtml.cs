using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.Employee;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class EditNewEmployeeDetailsModel : EMS.FrontEnd.SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.NewEmployeeForm Employee { get; set; }

        public EditNewEmployeeDetailsModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ApplicantID, int EmployeeID)
        {
            Employee = new NewEmployeeForm();

            Employee.ID = EmployeeID;

            var Applicant = (await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                .GetApplicant(ApplicantID));

            ViewData["title"] = string.Concat("Applicant ID# ", Applicant.ID.ToString().PadLeft(7,'0')," | ",Applicant.PersonalInformation.LastName,", ",Applicant.PersonalInformation.FirstName," ",Applicant.PersonalInformation.MiddleName," | DRAFT");


            var companyTagList = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                    EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString());

            ViewData["CompanyTagSelectList"] = companyTagList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();
        }

        public async Task<JsonResult> OnPostAsync() 
        {
            var (IsSuccess,Message) = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .ConvertNewEmployee(Employee));
            if (IsSuccess)
            {
                var EmployeeDetails = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployee(Employee.ID));

                var (SystemUserInfo, IsSystemUserSuccess, SystemUserMessage) = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .AddSystemUser(new EMS.Security.Transfer.SystemUser.GetByNameInput
                    {
                        EmployeeCode = EmployeeDetails.Code,
                        FirstName = EmployeeDetails.PersonalInformation.FirstName,
                        MiddleName = EmployeeDetails.PersonalInformation.MiddleName,
                        LastName = EmployeeDetails.PersonalInformation.LastName,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (IsSystemUserSuccess)
                {
                    await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .UpdateSystemUser(new EMS.Plantilla.Transfer.Employee.UpdateSystemUserInput
                        { 
                            EmployeeID = EmployeeDetails.ID,
                            SystemUserID = SystemUserInfo.ID
                        });
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "Employee Converted",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(EmployeeDetails.Code, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
    }
}
