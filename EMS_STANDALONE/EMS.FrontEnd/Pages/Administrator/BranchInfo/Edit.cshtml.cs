using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.FrontEnd.Pages.Administrator.BranchInfo
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.OrgGroup.Form OrgGroup { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasEditFeatureBomdAm"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITBOMDAM")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureHRBP"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITHRBP")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureRRT"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITRRT")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureCategory"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITCATEGORY")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureEmail"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITEMAIL")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureBranchNumber"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITBRANCHNUMBER")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeaturePsgc"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITPSGC")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureAddress"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITADDRESS")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureBranchSize"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITBRANCHSIZE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureParkingSize"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITPARKINGSIZE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureSign"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITSIGN")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeaturePage"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITPAGE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureBranchActive"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITBRANCHACTIVE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureServiceCount"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDITSERVICECOUNT")).Count() > 0 ? "true" : "false";
            }

            OrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(ID);

            ViewData["PsgcAddress"] = (OrgGroup.Psgc == null? "":(await new Common_PSGCPantillla(_iconfiguration, _globalCurrentUser, _env).GetPSGCAutoComplete(OrgGroup.Psgc, 1)).Select(x => x.Description).FirstOrDefault());

            var result = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPCATEGORY.ToString());
            ViewData["CategorySelectList"] = result.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            if (OrgGroup.CSODAM > 0)
            {
                var OrgCSODAM = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.CSODAM));
                ViewData["CSODAM"] = OrgCSODAM.Code + " - " + OrgCSODAM.Description;
            }
            else
                ViewData["CSODAM"] = "";
            if (OrgGroup.HRBP > 0)
            {
                var OrgHRBP = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.HRBP));
                ViewData["HRBP"] = OrgHRBP.Code + " - " + OrgHRBP.Description;
            }
            else
                ViewData["HRBP"] = "";
            if (OrgGroup.RRT > 0)
            {
                var OrgRRT = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.RRT));
                ViewData["RRT"] = OrgRRT.Code + " - " + OrgRRT.Description;
            }
            else
                ViewData["RRT"] = "";
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(OrgGroup.ID);

            oldValue.CSODAM = OrgGroup.CSODAM;
            oldValue.HRBP = OrgGroup.HRBP;
            oldValue.RRT = OrgGroup.RRT;
            oldValue.Category = (string.IsNullOrEmpty(OrgGroup.Category) ? oldValue.Category : OrgGroup.Category);
            oldValue.Email = (string.IsNullOrEmpty(OrgGroup.Email) ? oldValue.Email : OrgGroup.Email);
            oldValue.Number = (string.IsNullOrEmpty(OrgGroup.Number) ? oldValue.Number : OrgGroup.Number);
            oldValue.Psgc = (string.IsNullOrEmpty(OrgGroup.Psgc) ? oldValue.Psgc : OrgGroup.Psgc);
            oldValue.Address = (string.IsNullOrEmpty(OrgGroup.Address) ? oldValue.Address : OrgGroup.Address);
            oldValue.BranchSize = (string.IsNullOrEmpty(OrgGroup.BranchSize) ? oldValue.BranchSize : OrgGroup.BranchSize);
            oldValue.ParkingSize = (string.IsNullOrEmpty(OrgGroup.ParkingSize) ? oldValue.ParkingSize : OrgGroup.ParkingSize);
            oldValue.Sign = (string.IsNullOrEmpty(OrgGroup.Sign) ? oldValue.Sign : OrgGroup.Sign);
            oldValue.Page = (string.IsNullOrEmpty(OrgGroup.Page) ? oldValue.Page : OrgGroup.Page);
            oldValue.IsBranchActive = OrgGroup.IsBranchActive;
            oldValue.ServiceBayCount = (string.IsNullOrEmpty(OrgGroup.ServiceBayCount.ToString()) ? oldValue.ServiceBayCount : OrgGroup.ServiceBayCount);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(oldValue, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}