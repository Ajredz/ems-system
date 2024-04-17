using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Shared.Reference
{
    public class _ReferenceMaintenance : SharedClasses.Utilities
    {
        [BindProperty]
        public List<Utilities.API.ReferenceMaintenance.ReferenceValue> ReferenceValue { get; set; }

        private string[] _refCodeList = null;
        private readonly string _baseURL;

        public _ReferenceMaintenance(IConfiguration iconfiguration, string[] refCodeList, string baseURL, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _refCodeList = refCodeList;
            _baseURL = baseURL;
        }

        public async Task OnGetAsync()
        {
            if (_globalCurrentUser != null)
            {
                ReferenceValue = new List<Utilities.API.ReferenceMaintenance.ReferenceValue>();
                //var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _baseURL).GetReferenceByCode(_refCodeList);
                var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _baseURL, _env).GetReferenceByIsMaintainable();
                ViewData["ReferenceSelectList"] = result.Select(
                    x => new SelectListItem
                    {
                        Value = x.Code,
                        Text = x.Description
                    }).ToList();
            }
        }

        public async Task<JsonResult> OnGetReferenceValuesAsync(string RefCode)
        {
            return new JsonResult(await new Common_Reference(_iconfiguration, _globalCurrentUser, _baseURL, _env).GetReferenceValueByRefCode(RefCode));
        }

        public async Task<JsonResult> OnPostAsync(string refcode)
        {
            ReferenceValue.Select(x =>
            {
                x.UserID = _globalCurrentUser.UserID;
                x.RefCode = refcode;
                return x;
            }).ToList();

            var URL = string.Concat(_baseURL,
                _iconfiguration.GetSection("Reference").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(ReferenceValue, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}