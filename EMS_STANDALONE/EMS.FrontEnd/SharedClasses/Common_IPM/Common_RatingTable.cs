using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.IPM.Transfer.RatingTable;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_IPM
{
    public class Common_RatingTable : Utilities
    {
        public Common_RatingTable(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }
        
        public async Task<List<GetAllOutput>> GetAll()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("RatingTable").GetSection("GetAll").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

    }
}
