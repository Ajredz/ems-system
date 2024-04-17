using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Plantilla.Transfer.Employee;
using EMS.Workflow.Transfer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using System.Text;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class AddSkillsModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.EmployeeSkillsForm EmployeeSkillsForm { get; set; }

        public AddSkillsModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                EmployeeSkillsForm = new EMS.Plantilla.Transfer.Employee.EmployeeSkillsForm();

                var skillsCode = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode("SKILLS");
                ViewData["SkillsSelectList"] = skillsCode.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

            }
        }
        public async Task<JsonResult> OnPostAsync()
        {
            EmployeeSkillsForm.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_plantillaBaseURL,
                    _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("AddSkills").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var GetSkill = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode("SKILLS");

            var GetSkills = GetSkill.Where(x => x.Value.Equals(EmployeeSkillsForm.Skills)).FirstOrDefault();
            EmployeeSkillsForm.SkillsCode = GetSkills.Value;
            EmployeeSkillsForm.SkillsDescription = GetSkills.Description;

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(EmployeeSkillsForm, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}