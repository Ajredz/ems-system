using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Utilities.API;
using EMS.Plantilla.Transfer.Dashboard;

namespace EMS.FrontEnd.SharedClasses.Common_Plantilla
{
    public class Common_Dashboard : Utilities
    {
        public Common_Dashboard(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<(List<GetPlantillaDashboardOutput>, bool, string)> GetPlantillaDashboard(PlantillaDashboardInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetPlantillaDashboard").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetPlantillaDashboardOutput>(), param, URL);
        }
    }
}
