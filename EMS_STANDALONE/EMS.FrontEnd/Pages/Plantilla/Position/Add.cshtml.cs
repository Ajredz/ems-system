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

namespace EMS.FrontEnd.Pages.Plantilla.Position
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Position.Form Position { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                Position = new EMS.Plantilla.Transfer.Position.Form();
                
                ViewData["PositionLevelSelectList"] = 
                    await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdown();

                //ViewData["ParentPositionSelectList"] =
                //    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(Position.ParentPositionID ?? 0);

                var jobClass = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
               .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.JOB_CLASS.ToString());
                ViewData["JobClassSelectList"] =
                    jobClass.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Position.CreatedBy = _globalCurrentUser.UserID;

            if (ModelState.IsValid)
            {
                var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Position").GetSection("Add").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Position, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "Position",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Position.Code, " added"),
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

        public async Task<JsonResult> OnGetPositionLevelDropdown()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").GetSection("GetPositionLevelDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID);
            List<EMS.Plantilla.Transfer.PositionLevel.Form> result = new List<EMS.Plantilla.Transfer.PositionLevel.Form>();

            var PositionLevelDropdownTuple = await SharedUtilities.GetFromAPI(result, URL);

            if (PositionLevelDropdownTuple.IsSuccess)
            {
                if (PositionLevelDropdownTuple.APIResult != null)
                {
                    _resultView.Result = PositionLevelDropdownTuple.APIResult;
                    _resultView.IsSuccess = true;
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = PositionLevelDropdownTuple.ErrorMessage;
            }

            return new JsonResult(_resultView);
        }

        //public async Task<JsonResult> OnGetOrgGroupDropdown()
        //{
        //    var URL = string.Concat(_plantillaBaseURL,
        //       _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrganizationalGroup").GetSection("GetOrganizationalGroupDropdown").Value, "?",
        //        "userid=", _globalCurrentUser.UserId);
        //    List<EMS.Plantilla.Transfer.OrganizationalGroup.OrganizationalGroup> result = new List<EMS.Plantilla.Transfer.OrganizationalGroup.OrganizationalGroup>();

        //    var OrgGroupDropdownTuple = await SharedUtilities.GetFromAPI(result, URL);

        //    if (OrgGroupDropdownTuple.IsSuccess)
        //    {
        //        if (OrgGroupDropdownTuple.APIResult != null)
        //        {
        //            _resultView.Result = OrgGroupDropdownTuple.APIResult;
        //            _resultView.IsSuccess = true;
        //        }
        //    }
        //    else
        //    {
        //        _resultView.IsSuccess = false;
        //        _resultView.Result = OrgGroupDropdownTuple.ErrorMessage;
        //    }

        //    return new JsonResult(_resultView);
        //}
    }
}