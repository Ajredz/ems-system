using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class EditModel : MRF.EditModel
    {
        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }
    }
}