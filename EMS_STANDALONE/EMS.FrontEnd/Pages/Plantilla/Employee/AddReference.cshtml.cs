using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.Recruitment.Transfer.Reference;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class AddReferenceModel : SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.ReferenceMaintenance.ReferenceValue ReferenceValue { get; set; }

        public AddReferenceModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(string refcode)
        {
            ReferenceValue.UserID = _globalCurrentUser.UserID;
            ReferenceValue.RefCode = refcode;

            //var URL = string.Concat(_plantillaBaseURL,
            //    _iconfiguration.GetSection("Reference").GetSection("AddReferenceValue").Value, "?",
            //    "userid=", _globalCurrentUser.UserID);

            var URL = string.Concat(_workflowBaseURL,
                _iconfiguration.GetSection("Reference").GetSection("AddReferenceValue").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(ReferenceValue, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValueList(string RefCode)
        {

            //List<GetReferenceValueListOutput> result =
            //       await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
            //       .GetReferenceValueList(RefCode);

            List<GetReferenceValueListOutput> result =
                   await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                   .GetReferenceValueList(RefCode);

            var jsonData = new
            {
                total = result.Count,
                pageNumber = 1,
                sort = "Description",
                records = result.Count,
                rows = result
            };
            return new JsonResult(jsonData);
        }
    }
}