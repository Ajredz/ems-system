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

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class EditReferenceModel : SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.ReferenceMaintenance.ReferenceValue ReferenceValue { get; set; }

        public EditReferenceModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {

            ReferenceValue = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env).GetReferenceValueByID(ID);
        }

        public async Task<JsonResult> OnPostAsync(string refcode)
        {
            ReferenceValue.UserID = _globalCurrentUser.UserID;
            ReferenceValue.RefCode = refcode;

            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("Reference").GetSection("UpdateReferenceValue").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(ReferenceValue, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}