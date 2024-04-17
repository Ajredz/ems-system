using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class RemoveApplicantModel : MRF.RemoveApplicantModel
    {
        public RemoveApplicantModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }

    }
}