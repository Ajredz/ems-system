using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS_FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class DeleteModel : OrgGroup.DeleteModel
    {
        public DeleteModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }

    }
}