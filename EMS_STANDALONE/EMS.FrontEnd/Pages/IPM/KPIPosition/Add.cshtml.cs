using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.IPM.KPIPosition
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPIPosition.Form KPIPosition { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                KPIPosition = new EMS.IPM.Transfer.KPIPosition.Form();

                // Get all Positions with KPI
                var existingKPIPosList =
                    await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetAllKPIPosition();
                var existingKPIPosID = existingKPIPosList.Select(x => x.Position.ToString()).Distinct().ToList();

                // Exclude Positions with KPI from Select List
                var positionList =
                    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown();
                var filteredPositionList = positionList.Where(x => !existingKPIPosID.Contains(x.Value)).ToList();

                ViewData["PositionSelectList"] = filteredPositionList;

                var AllKPIPositionList =
                    await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetAllKPIPosition();
                AllKPIPositionList = AllKPIPositionList.OrderBy(x => x.Position).OrderBy(x => x.TDate).ToList();
                var EffectiveDatesList = AllKPIPositionList.Select(x => new
                {
                    Position = x.Position,
                    TDate = x.TDate,
                    EffectiveDate = x.TDate.ToString("MM/dd/yyyy")
                })
                .Distinct()
                .OrderBy(x => x.Position)
                .ThenByDescending(x => x.TDate).ToList();

                ViewData["EffectiveDatesList"] = EffectiveDatesList;

                ViewData["AllKPIList"] = 
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetAllKPIDetails();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            KPIPosition.ModifiedBy = _globalCurrentUser.UserID;

            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("Add").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(KPIPosition, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "KPIPosition",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(KPIPosition.Position, " KPI added"),
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