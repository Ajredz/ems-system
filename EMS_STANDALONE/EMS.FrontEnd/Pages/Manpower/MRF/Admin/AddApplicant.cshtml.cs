using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class AddApplicantModel : MRF.AddApplicantModel
    {
        public AddApplicantModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }
    }
}