using EMS.Common.Data.DBContexts;
using EMS.Common.Data.SystemUser;
using EMS.Common.Transfer.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Common.Core.SystemUser
{
    public interface ISystemUserService
    {
        Task<IActionResult> GetSystemUserByUsername(string username, int param);
        Task<IActionResult> GetSystemUser(string username, string password ,int userid);
    }

    public class SystemUserService : EMS.Common.Core.Shared.Utilities, ISystemUserService
    {
        private readonly ISystemUserDBAccess _dbAccess;

        public SystemUserService(SystemAccessContext dbContext, IConfiguration iconfiguration,
            ISystemUserDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetSystemUserByUsername(string username, int param)
        {
            var result = await _dbAccess.GetSystemUserByUsername(username, param);

            GlobalCurrentUser currentUser = null;

            if (result != null)
            {
                currentUser = result.Select(x => new GlobalCurrentUser
                {
                    Username = x.Username,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    CompanyID = 1,
                    Company = "<Company Name Here>",
                    Branch = "<Branch Name Here>",
                    BranchCode = "code",
                    Position = "<Position Title Here>",
                    UserID = x.ID,
                    LastLoggedIn = x.LastLoggedIn.HasValue ? x.LastLoggedIn.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastLoggedOut = x.LastLoggedOut.HasValue ? x.LastLoggedOut.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastPasswordChange = x.LastPasswordChange.HasValue ? x.LastPasswordChange.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                }).FirstOrDefault();

                return new OkObjectResult(currentUser);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }
            //return new OkObjectResult(
            //   (await _dbAccess.GetSystemUserByUsername( username, param)).Select(x => new GlobalCurrentUser
            //   {
            //       Username = x.Username,
            //       LastName = x.LastName,
            //       FirstName = x.FirstName,
            //       MiddleName = x.MiddleName,
            //       //CompanyID = result.CompanyID,
            //       Company = "<Company Name Here>",
            //       Branch = "<Branch Name Here>",
            //       BranchCode = "code",
            //       Position = "<Position Title Here>",
            //       UserID = x.ID,
            //       LastLoggedIn = x.LastLoggedIn.HasValue ? x.LastLoggedIn.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
            //       LastLoggedOut = x.LastLoggedOut.HasValue ? x.LastLoggedOut.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
            //       LastPasswordChange = x.LastPasswordChange.HasValue ? x.LastPasswordChange.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
            //   }));
        }

        public async Task<IActionResult> GetSystemUser(string username, string password ,  int userid)
        {
            var result = await _dbAccess.GetSystemUser(username, password);

            GlobalCurrentUser currentUser = null;

            if (result != null)
            {
                currentUser = result.Select(x => new GlobalCurrentUser
                {
                    //Username = x.Username,
                    //LastName = x.LastName,
                    //FirstName = x.FirstName,
                    //MiddleName = x.MiddleName,
                    //CompanyID = 1,
                    //Company = "<Company Name Here>",
                    //Branch = "<Branch Name Here>",
                    //BranchCode = "code",
                    //Position = "<Position Title Here>",
                    //UserID = x.ID,
                    //LastLoggedIn = x.LastLoggedIn.HasValue ? x.LastLoggedIn.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    //LastLoggedOut = x.LastLoggedOut.HasValue ? x.LastLoggedOut.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    //LastPasswordChange = x.LastPasswordChange.HasValue ? x.LastPasswordChange.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    Username = x.Username,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    CompanyID = x.CompanyID,
                    Company = "<Company Name Here>",
                    Branch = "<Branch Name Here>",
                    BranchCode = "code",
                    Position = "<Position Title Here>",
                    UserID = x.ID,
                    LastLoggedIn = x.LastLoggedIn.HasValue ? x.LastLoggedIn.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastLoggedOut = x.LastLoggedOut.HasValue ? x.LastLoggedOut.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastPasswordChange = x.LastPasswordChange.HasValue ? x.LastPasswordChange.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    IsActive = x.IsActive,
                    IsPasswordChanged = x.IsPasswordChanged
                }).FirstOrDefault();

                return new OkObjectResult(currentUser);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }
        }

    }
}
