﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class CancelMRFModel : SharedClasses.Utilities
    {
        public CancelMRFModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(EMS.Manpower.Transfer.MRF.MRFCancelForm param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("HRCancelMRF").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}