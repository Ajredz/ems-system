using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.PositionLevel
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.PositionLevel.Form PositionLevel { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            PositionLevel = new EMS.Plantilla.Transfer.PositionLevel.Form();
        }

        public async Task<JsonResult> OnPostAsync()
        {
            PositionLevel.CreatedBy = _globalCurrentUser.UserID;

            //if (ModelState.IsValid)
            //{
                var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").GetSection("Add").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(PositionLevel, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "PositionLevel",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(PositionLevel.Description, " added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            //}
            //else
            //{
            //    _resultView.IsSuccess = false;
            //    _resultView.Result = string.Concat(MessageUtilities.PRE_ERRMSG_REC_SAVE,
            //    string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray()));
            //}

            return new JsonResult(_resultView);
        }
    }
}