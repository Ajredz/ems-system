using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.Plantilla.PositionLevel
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.PositionLevel.Form PositionLevel { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITIONLEVEL/DELETE")).Count() > 0 ? "true" : "false";
            }

            PositionLevel = await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevel(ID);
        }

        public async Task<JsonResult> OnPostAsync()
        {
            PositionLevel.CreatedBy = _globalCurrentUser.UserID;

            if (ModelState.IsValid)
            {
                var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var oldValue = await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevel(PositionLevel.ID);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(PositionLevel, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    StringBuilder Remarks = new StringBuilder();

                    if (PositionLevel.Description != oldValue.Description)
                    {
                        Remarks.Append(string.Concat("Description changed from ", oldValue.Description, " to ", PositionLevel.Description, ". "));
                    }

                    if (Remarks.Length > 0)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                                TableName = "PositionLevel",
                                TableID = PositionLevel.ID,
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