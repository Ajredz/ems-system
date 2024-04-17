using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class AddEmployeeFieldModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.EmployeeMovement.EmployeeFieldForm MovementEmployeeField { get; set; }

        public AddEmployeeFieldModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            var employeeFields = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.MOVEMENT_EMP_FIELD.ToString())).ToList();

            ViewData["EmployeeFieldSelectList"] = employeeFields.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();
        }

        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("AddMovementEmployeeField").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(MovementEmployeeField, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "EmployeeMovementMapping",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(MovementEmployeeField.EmployeeField, " added to ", MovementEmployeeField.MovementType),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        }); 
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = string.Concat(MessageUtilities.PRE_ERRMSG_REC_SAVE,
                string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray()));
            }

            return new JsonResult(_resultView);
        }
    }
}