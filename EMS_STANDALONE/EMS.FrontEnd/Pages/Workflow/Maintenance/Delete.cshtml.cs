﻿using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Workflow.Maintenance
{
    public class DeleteModel : EMS.FrontEnd.SharedClasses.Utilities
    {
        public DeleteModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                   _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("Delete").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.DELETE.ToString(),
                        TableName = "Workflow",
                        TableID = ID,
                        Remarks = string.Concat(ID, " deleted"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
    }
}