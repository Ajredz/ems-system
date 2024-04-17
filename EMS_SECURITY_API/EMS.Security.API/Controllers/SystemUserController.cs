using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS_SecurityService.DBContexts;
using EMS_SecurityService.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS_SecurityServiceModel.SystemUser;
using EMS.Security.Core.SystemUser;
using EMS.Security.Transfer.SystemUser;

namespace EMS_SecurityService.Controllers
{
    [Route("security/[controller]")]
    [ApiController]
    public class SystemUserController : SharedClasses.Utilities
    {
        private readonly ISystemUserService _service;

        public SystemUserController(SystemAccessContext dbContext, IConfiguration iconfiguration,
            ISystemUserService service) : base(dbContext, iconfiguration)
        {
            _service = service;
        }

        [HttpGet]
        [Route("getuserdetails")]
        public async Task<IActionResult> GetSystemUser([FromQuery] string username, [FromQuery] string password, [FromQuery] int userid)
        {
            _userID = userid;
            var result = (await _dbContext.tv_CurrentUser
                .FromSqlRaw("CALL sp_login_authentication({0},{1})", username, password)
                .ToListAsync()).FirstOrDefault();
            if (result != null)
            {
                GlobalCurrentUser currentUser = new GlobalCurrentUser
                {
                    Username = result.Username,
                    LastName = result.LastName,
                    FirstName = result.FirstName,
                    MiddleName = result.MiddleName,
                    CompanyID = result.CompanyID,
                    Company = "<Company Name Here>",
                    Branch = "<Branch Name Here>",
                    BranchCode = "code",
                    Position = "<Position Title Here>",
                    UserID = result.ID,
                    LastLoggedIn = result.LastLoggedIn.HasValue ? result.LastLoggedIn.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastLoggedOut = result.LastLoggedOut.HasValue ? result.LastLoggedOut.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastPasswordChange = result.LastPasswordChange.HasValue ? result.LastPasswordChange.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    IsActive = result.IsActive,
                    IsPasswordChanged = result.IsPasswordChanged
                };

                if (currentUser.IsActive == true)
                    return new OkObjectResult(currentUser);
                else
                    return new BadRequestObjectResult(MessageUtilities.ERRMSG_USER_ACCOUNT_INACTIVE);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }

        }

        [HttpGet]
        [Route("getuserdetailsbyusername")]
        public async Task<IActionResult> GetSystemUserByUsername([FromQuery] string username, [FromQuery] int userid)
        {
            _userID = userid;
            var result = await _dbContext.SystemUser
                .Where(x => x.Username.ToLower().Equals((username ?? "").ToLower()))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            GlobalCurrentUser currentUser = null;

            if (result != null)
            {
                currentUser = new GlobalCurrentUser
                {
                    Username = result.Username,
                    LastName = result.LastName,
                    FirstName = result.FirstName,
                    MiddleName = result.MiddleName,
                    CompanyID = result.CompanyID,
                    Company = "<Company Name Here>",
                    Branch = "<Branch Name Here>",
                    BranchCode = "code",
                    Position = "<Position Title Here>",
                    UserID = result.ID,
                    LastLoggedIn = result.LastLoggedIn.HasValue ? result.LastLoggedIn.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastLoggedOut = result.LastLoggedOut.HasValue ? result.LastLoggedOut.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                    LastPasswordChange = result.LastPasswordChange.HasValue ? result.LastPasswordChange.Value.ToString("MMMM dd, yyyy HH:mm tt") : "",
                };
                return new OkObjectResult(currentUser);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }

        }



        [HttpGet]
        [Route("getuserautocomplete")]
        public async Task<IActionResult> GetSystemUserAutoComplete([FromQuery] string username, [FromQuery] int userid)
        {
            _userID = userid;
            var result = await _dbContext.SystemUser
                .AsNoTracking()
                .Where(x => x.Username.ToLower().Contains(username.ToLower()))
                .ToListAsync();

            List<SystemUser> usernamelist = null;

            if (result != null)
            {
                usernamelist = result.Select(x => new SystemUser { ID = x.ID, Username = x.Username }).ToList();
            }

            return new OkObjectResult(usernamelist);
        }

        //[HttpGet]
        //[Route("getuserdropdown")]
        //public async Task<IActionResult> GetSystemUserDropdown()
        //{
        //    var result = await _dbContext.SystemUser
        //        .AsNoTracking()
        //        .ToListAsync();

        //    List<SystemUser> usernamelist = null;

        //    if (result != null)
        //    {
        //        usernamelist = result.Select(x => new SystemUser { ID = x.ID, Username = x.Username }).ToList();
        //    }

        //    return new OkObjectResult(usernamelist);
        //}

        /// <summary>
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery]APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-by-ids")]
        public async Task<IActionResult> GetByIDs([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.GetByIDs(credentials, IDs).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromQuery] GetByNameInput param)
        {
            return await _service.AddSystemUser(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-system-users")]
        public async Task<IActionResult> AddSystemUsers([FromQuery] APICredentials credentials, [FromBody] List<GetByNameInput> param)
        {
            return await _service.AddSystemUsers(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-system-user-dropdown-by-id")]
        public async Task<IActionResult> GetSystemUserDropDownByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetSystemUserDropDownByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-system-user-dropdown")]
        public async Task<IActionResult> GetSystemUserDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetSystemUserDropDown(credentials).ConfigureAwait(true);
		}
		
        [HttpPost]
        [Route("integrate-with-portal-global")]
        public async Task<IActionResult> IntegrateWithPortalGlobal([FromQuery] APICredentials credentials)
        {
            return await _service.IntegrateWithPortalGlobal(credentials).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromQuery] APICredentials credentials, [FromBody] ChangePasswordInput param)
        {
            return await _service.ChangePassword(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-system-user-role")]
        public async Task<IActionResult> AddSystemUserRole([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.AddSystemUserRole(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-system-user-role")]
        public async Task<IActionResult> UpdateSystemUserRole([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.UpdateSystemUserRole(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-system-user-role-dropdown-by-user-id")]
        public async Task<IActionResult> GetSystemUserRoleDropDownByRoleID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetSystemUserRoleDropDownByUserID(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("batch-reset-password")]
        public async Task<IActionResult> BatchResetPassword([FromQuery] APICredentials credentials, [FromBody] BatchResetPasswordForm param)
        {
            return await _service.BatchResetPassword(credentials, param).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("employee-upload-insert")]
        public async Task<IActionResult> EmployeeUploadInsert([FromQuery] APICredentials credentials, [FromBody] List<EmployeeUploadInsertInput> param)
        {
            return await _service.EmployeeUploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-username")]
        public async Task<IActionResult> UpdateUsername([FromQuery] APICredentials credentials, [FromBody] UpdateUsername param)
        {
            return await _service.UpdateUsername(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> ChangeStatus([FromQuery] APICredentials credentials, [FromBody] ChangeStatusForm param)
        {
            return await _service.ChangeStatus(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("force-change-password")]
        public async Task<IActionResult> ForceChangePassword([FromQuery] APICredentials credentials, [FromBody] ForceChangePasswordInput param)
        {
            return await _service.ForceChangePassword(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("sync-from-h2pay")]
        public async Task<IActionResult> SyncFromH2Pay([FromQuery] APICredentials credentials
            , [FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.SyncFromH2Pay(unit, value).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("disable-employee-account")]
        public async Task<IActionResult> DisableEmployeeAccount([FromQuery] APICredentials credentials, [FromBody] UpdateSystemUser param)
        {
            return await _service.DisableEmployeeAccount(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] APICredentials credentials, [FromBody] ResetPassword param)
        {
            return await _service.ResetPassword(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-user-by-role-ids")]
        public async Task<IActionResult> GetUserByRoleIDs([FromQuery] APICredentials credentials, [FromBody] List<int> ID)
        {
            return await _service.GetUserByRoleIDs(credentials, ID).ConfigureAwait(true);
        }
    }
}
