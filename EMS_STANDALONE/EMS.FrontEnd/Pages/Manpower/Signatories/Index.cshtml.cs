using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Manpower.Transfer.DataDuplication.Position;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.Signatories
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public List<EMS.Manpower.Transfer.MRFSignatories.Form> MRFSignatory { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnGetSignatories(int UserID, int PositionID)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRFSignatories").GetSection("List").Value, "?",
                "UserID=", UserID,
                "&PositionID=", PositionID);
            List<EMS.Manpower.Transfer.MRFSignatories.Form> result = new List<EMS.Manpower.Transfer.MRFSignatories.Form>();

            var (APIResult, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(result, URL);

            if (IsSuccess)
            {
                if (APIResult != null)
                {
                    _resultView.Result = APIResult;
                    _resultView.IsSuccess = true;
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = ErrorMessage;
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetUserNameAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSystemRoleDropDown()
        {
            _resultView.Result = await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleDropDown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionLevelDropDown()
        {
            _resultView.Result = await new Common_Synced_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDown(int PositionLevelID)
        {
            _resultView.Result = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetDropdownByPositionLevel(new GetDropDownInput
            {
                PositionLevelID = PositionLevelID,
                SelectedValue = 0
            });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSubmit()
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRFSignatories").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(MRFSignatory, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "MRFSignatories",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "MRF Signatories added",
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
    }
}