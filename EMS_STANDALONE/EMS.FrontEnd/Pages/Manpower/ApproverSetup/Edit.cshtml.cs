using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.Text;
using EMS.Manpower.Transfer.ApproverSetup;
using EMS.Plantilla.Transfer.OrgGroup;

namespace EMS.FrontEnd.Pages.Manpower.ApproverSetup
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Manpower.Transfer.ApproverSetup.Form ApproverSetup { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGetAsync(int ID)
        {

            if (_globalCurrentUser != null)
            {
                ApproverSetup = new EMS.Manpower.Transfer.ApproverSetup.Form { ID = ID };
            }
        }

        public async Task<JsonResult> OnGetPositionList(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("ApproverSetup").GetSection("GetByID").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EMS.Manpower.Transfer.ApproverSetup.Form(), URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostAsync()
        {
                var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("ApproverSetup").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                //var oldValue = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(ApproverSetup.ID);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(ApproverSetup, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                //if (IsSuccess)
                //{
                //    StringBuilder Remarks = new StringBuilder();
                    
                //    if (ApproverSetup.Code != oldValue.Code)
                //    {
                //        Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", ApproverSetup.Code, ". "));
                //    }

                //    if (ApproverSetup.PositionLevelID != oldValue.PositionLevelID)
                //    {
                //        Remarks.Append(string.Concat("PositionLevelID changed from ", oldValue.PositionLevelID, " to ", ApproverSetup.PositionLevelID, ". "));
                //    }

                //    if (ApproverSetup.Title != oldValue.Title)
                //    {
                //        Remarks.Append(string.Concat("Title changed from ", oldValue.Title, " to ", ApproverSetup.Title, ". "));
                //    }

                //    if (ApproverSetup.ParentPositionID != oldValue.ParentPositionID)
                //    {
                //        Remarks.Append(string.Concat("ParentPositionID changed from ", oldValue.ParentPositionID, " to ", ApproverSetup.ParentPositionID, ". "));
                //    }

                //    if (Remarks.Length > 0)
                //    {
                //        /*Add AuditLog*/
                //        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                //            .AddAuditLog(new Security.Transfer.AuditLog.Form
                //            {
                //                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                //                TableName = "ApproverSetup",
                //                TableID = ApproverSetup.ID,
                //                Remarks = Remarks.ToString(),
                //                IsSuccess = true,
                //                CreatedBy = _globalCurrentUser.UserID
                //            }); ; 
                //    }
                //}
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoComplete(EMS.Plantilla.Transfer.Position.GetAutoCompleteInput param)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetPositionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupAutoComplete(string Term, int TopResults)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}