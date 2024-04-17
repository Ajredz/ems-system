using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class EditReferenceModel : SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.ReferenceMaintenance.ReferenceValue ReferenceValue { get; set; }

        public EditReferenceModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {

            //ReferenceValue = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByID(ID);
            ReferenceValue = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env).GetReferenceValueByID(ID);
        }

        public async Task<JsonResult> OnPostAsync(string refcode)
        {
            ReferenceValue.UserID = _globalCurrentUser.UserID;
            ReferenceValue.RefCode = refcode;

            //var URL = string.Concat(_plantillaBaseURL,
            //                   _iconfiguration.GetSection("Reference").GetSection("UpdateReferenceValue").Value, "?",
            //                   "userid=", _globalCurrentUser.UserID);

            var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("Reference").GetSection("UpdateReferenceValue").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(ReferenceValue, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}