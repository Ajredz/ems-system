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

namespace EMS.FrontEnd.Pages.Plantilla.Position
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Position.Form Position { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITION/DELETE")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                Position = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(ID);

                ViewData["PositionLevelSelectList"] = 
                    await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdown(Position.PositionLevelID);

                //ViewData["ParentPositionSelectList"] =
                //    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(Position.ParentPositionID ?? 0);

                var jobClass = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.JOB_CLASS.ToString());
                ViewData["JobClassSelectList"] =
                    jobClass.Select(x => new SelectListItem
                    { Value = x.Value, Text = x.Description, Selected = x.Value == Position.JobClassCode }
                    ).ToList();

            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Position.CreatedBy = _globalCurrentUser.UserID;

            if (ModelState.IsValid)
            {
                var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Position").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var oldValue = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(Position.ID);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Position, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    StringBuilder Remarks = new StringBuilder();
                    
                    if (Position.Code != oldValue.Code)
                    {
                        Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", Position.Code, ". "));
                    }

                    if (Position.PositionLevelID != oldValue.PositionLevelID)
                    {
                        Remarks.Append(string.Concat("PositionLevelID changed from ", oldValue.PositionLevelID, " to ", Position.PositionLevelID, ". "));
                    }

                    if (Position.Title != oldValue.Title)
                    {
                        Remarks.Append(string.Concat("Title changed from ", oldValue.Title, " to ", Position.Title, ". "));
                    }

                    if (Position.ParentPositionID != oldValue.ParentPositionID)
                    {
                        Remarks.Append(string.Concat("ParentPositionID changed from ", oldValue.ParentPositionID, " to ", Position.ParentPositionID, ". "));
                    }

                    if (Remarks.Length > 0)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                                TableName = "Position",
                                TableID = Position.ID,
                                Remarks = Remarks.ToString(),
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            }); ; 
                    }
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