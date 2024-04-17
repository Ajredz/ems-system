using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.Manpower.Transfer.MRF;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class ApplicantPickerModel : MRF.ApplicantPickerModel
    {
        public ApplicantPickerModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }

    }
}