using EMS.Security.Data._IntegrationModels;
using EMS.Security.Data.DBContexts;
using EMS.Security.Data.SystemRole;
using EMS.Security.Data.SystemUser;
using EMS.Security.Transfer.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Security.Core.SystemUser
{
    public interface ISystemUserService
    {
        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);
        
        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> GetByIDs(APICredentials credentials, List<int> IDs);

        Task<IActionResult> AddSystemUser(APICredentials credentials, GetByNameInput param);
        Task<IActionResult> AddSystemUsers(APICredentials credentials, List<GetByNameInput> param);

        Task<IActionResult> GetSystemUserDropDownByID(APICredentials credentials, int ID);

        Task<IActionResult> GetSystemUserDropDown(APICredentials credentials);

        Task<IActionResult> IntegrateWithPortalGlobal(APICredentials credentials);

        Task<IActionResult> ChangePassword(APICredentials credentials, ChangePasswordInput param);

        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> UpdateSystemUserRole(APICredentials credentials, Form param);

        Task<IActionResult> AddSystemUserRole(APICredentials credentials, Form param);

        Task<IActionResult> GetSystemUserRoleDropDownByUserID(APICredentials credentials, int ID);

        Task<IActionResult> BatchResetPassword(APICredentials credentials, BatchResetPasswordForm param);

        Task<IActionResult> EmployeeUploadInsert(APICredentials credentials, List<EmployeeUploadInsertInput> param);

        Task<IActionResult> UpdateUsername(APICredentials credentials, UpdateUsername param);

        Task<IActionResult> ChangeStatus(APICredentials credentials, ChangeStatusForm param);

        Task<IActionResult> ForceChangePassword(APICredentials credentials, ForceChangePasswordInput param);

        Task<IActionResult> SyncFromH2Pay(SharedUtilities.UNIT_OF_TIME unit, int value);
        Task<IActionResult> DisableEmployeeAccount(APICredentials credentials, UpdateSystemUser param);
        Task<IActionResult> ResetPassword(APICredentials credentials, ResetPassword param);
        Task<IActionResult> GetUserByRoleIDs(APICredentials credentials, List<int> ID);

    }
    public class SystemUserService : EMS.Security.Core.Shared.Utilities, ISystemUserService
    {
        private readonly ISystemUserDBAccess _dbAccess;
        private readonly Itbl_usersDBAccess _tbl_usersDBAccess;
        private readonly H2Pay.Data.SystemUser.IDBAccess _H2PaySystemUserDBAccess;

        public SystemUserService(SystemAccessContext dbContext, IConfiguration iconfiguration,
            ISystemUserDBAccess dbAccess, Itbl_usersDBAccess tbl_usersDBAccess, H2Pay.Data.SystemUser.IDBAccess H2PaySystemUserDBAccess) :base (dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _tbl_usersDBAccess = tbl_usersDBAccess;
            _H2PaySystemUserDBAccess = H2PaySystemUserDBAccess;
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials ,GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Username
                })
            );
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            EMS.Security.Data.SystemUser.SystemUser result = (await _dbAccess.GetByID(ID));
            List<SystemUserRole> roles = (await _dbAccess.GetRoles(ID)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(new Form { 
                    ID = result.ID,
                    CompanyID = result.CompanyID,
                    Username = result.Username,
                    Password = result.Password,
                    FirstName = result.FirstName,
                    MiddleName = result.MiddleName,
                    LastName = result.LastName,
                    FailedAttempt = result.FailedAttempt,
                    IsPasswordChanged = result.IsPasswordChanged,
                    IsLoggedIn = result.IsLoggedIn,
                    IntegrationKey = result.IntegrationKey,
                    IsActive = result.IsActive,
                    LastLoggedIn = result.LastLoggedIn,
                    LastLoggedOut = result.LastLoggedOut,
                    LastPasswordChange = result.LastPasswordChange,
                    RoleIDs = roles.Select(x => x.RoleID).ToList()
                });
            }
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));
        }

        public async Task<IActionResult> GetByIDs(APICredentials credentials, List<int> IDs)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByIDs(IDs)).Select(x => new Form
                {
                    ID = x.ID,
                    CompanyID = x.CompanyID,
                    Username = x.Username,
                    Password = x.Password,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    FailedAttempt = x.FailedAttempt,
                    IsPasswordChanged = x.IsPasswordChanged,
                    IsLoggedIn = x.IsLoggedIn,
                    IntegrationKey = x.IntegrationKey,
                    IsActive = x.IsActive,
                    LastLoggedIn = x.LastLoggedIn,
                    LastLoggedOut = x.LastLoggedOut,
                    LastPasswordChange = x.LastPasswordChange
                })
                );
        }

        public async Task<IActionResult> AddSystemUser(APICredentials credentials, GetByNameInput param)
        {

            param.EmployeeCode = (param.EmployeeCode ?? "").Trim();
            param.FirstName = (param.FirstName ?? "").Trim();
            param.MiddleName = (param.MiddleName ?? "").Trim();
            param.LastName = (param.LastName ?? "").Trim();

            if (string.IsNullOrEmpty(param.EmployeeCode))
                ErrorMessages.Add(string.Concat("Employee Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.EmployeeCode.Length > 50)
                ErrorMessages.Add(string.Concat("Employee Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.FirstName))
                ErrorMessages.Add(string.Concat("FirstName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("FirstName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (!string.IsNullOrEmpty(param.MiddleName))
                if (param.MiddleName.Length > 50)
                    ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.LastName))
                ErrorMessages.Add(string.Concat("LastName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.LastName.Length > 50)
                ErrorMessages.Add(string.Concat("LastName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            List<Data.SystemUser.SystemUser> result = new List<Data.SystemUser.SystemUser>();

            if (ErrorMessages.Count == 0)
            {
                result = (await _dbAccess.AddSystemUser(new GetByNameInput
                {
                    EmployeeCode = param.EmployeeCode,
                    FirstName = param.FirstName,
                    LastName = param.LastName,
                    MiddleName = param.MiddleName,
                    CreatedBy = credentials.UserID
                })).ToList();
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
            {
                if (result == null)
                    return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
                else
                    return new OkObjectResult(
                    new Form
                    {
                        ID = result.FirstOrDefault().ID,
                        CompanyID = result.FirstOrDefault().CompanyID,
                        Username = result.FirstOrDefault().Username,
                        Password = result.FirstOrDefault().Password,
                        FirstName = result.FirstOrDefault().FirstName,
                        MiddleName = result.FirstOrDefault().MiddleName,
                        LastName = result.FirstOrDefault().LastName,
                        FailedAttempt = result.FirstOrDefault().FailedAttempt,
                        IsPasswordChanged = result.FirstOrDefault().IsPasswordChanged,
                        IsLoggedIn = result.FirstOrDefault().IsLoggedIn,
                        IntegrationKey = result.FirstOrDefault().IntegrationKey,
                        IsActive = result.FirstOrDefault().IsActive,
                        LastLoggedIn = result.FirstOrDefault().LastLoggedIn,
                        LastLoggedOut = result.FirstOrDefault().LastLoggedOut,
                        LastPasswordChange = result.FirstOrDefault().LastPasswordChange
                    });
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> AddSystemUsers(APICredentials credentials, List<GetByNameInput> param)
        {
            var addSystemUser = (from left in param
                         select new EMS.Security.Data.SystemUser.SystemUser()
                         {
                            CompanyID = 1,
                            Username = left.EmployeeCode,
                            Password = (Utilities.API.SharedUtilities.ComputeSHA256Hash(string.Concat("default", "259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd"))).ToLower(),
                            Salt = "259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd",
                            FirstName = left.FirstName,
                            MiddleName = left.MiddleName,
                            LastName = left.LastName,
                            IsActive = true,
                            CreatedBy = credentials.UserID
                         }).ToList();

            await _dbAccess.AddSystemUsers(addSystemUser);

            var getSystemUser = await _dbAccess.GetSystemUserByUsername(addSystemUser.Select(x => x.Username).ToList());

            List<int> systemUserRoleIDs = new List<int>() { 130, 131, 143, 139, 140, 141 };
            List<SystemUserRole> listSystemUserRole = new List<SystemUserRole>();

            foreach (var item in systemUserRoleIDs)
            {
                var addSystemUserRole = (from left in getSystemUser
                                         select new EMS.Security.Data.SystemRole.SystemUserRole()
                                         {
                                             UserID = left.ID,
                                             RoleID = item,
                                             CreatedBy = credentials.UserID
                                         }).ToList();
                listSystemUserRole.AddRange(addSystemUserRole);
            }

            await _dbAccess.AddSystemUserRoles(listSystemUserRole);

            return new OkObjectResult(getSystemUser);
        }

        public async Task<IActionResult> GetSystemUserDropDownByID(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                SharedUtilities.GetDropdown((await _dbAccess.GetSystemUserByID(ID)).OrderBy(x => x.Username).ToList(), "ID", "Username", "LastName", ID)
            );
        }

        public async Task<IActionResult> GetSystemUserDropDown(APICredentials credentials)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAllSystemUser()).Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = string.Concat(x.Username, " - ", x.FirstName, " ", x.LastName)

                }).ToList()
            );
		}
		
        public async Task<IActionResult> IntegrateWithPortalGlobal(APICredentials credentials)
        {
            static List<Data.SystemUser.SystemUser> GetToAdd(List<Data.SystemUser.SystemUser> left, List<Data.SystemUser.SystemUser> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { x.Username },
                    y => new { y.Username },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.SystemUser.SystemUser
                    {
                        CompanyID = x.newSet.newSet.CompanyID,
                        Username = x.newSet.newSet.Username,
                        Password = x.newSet.newSet.Password,
                        Salt = x.newSet.newSet.Salt,
                        FirstName = x.newSet.newSet.FirstName,
                        MiddleName = x.newSet.newSet.MiddleName,
                        LastName = x.newSet.newSet.LastName,
                        IntegrationKey = x.newSet.newSet.IntegrationKey,
                        IsPasswordChanged = false,
                        IsActive = x.newSet.newSet.IsActive,
                        CreatedBy = x.newSet.newSet.CreatedBy
                    })
                .ToList();
            }

            static List<Data.SystemUser.SystemUser> GetToUpdate(List<Data.SystemUser.SystemUser> left, List<Data.SystemUser.SystemUser> right)
            {
                return left.Join(
                right,
                x => new { x.Username },
                y => new { y.Username },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => 
                    !(x.oldSet.FirstName ?? "").Equals(x.newSet.FirstName)
                    || !(x.oldSet.MiddleName ?? "").Equals(x.newSet.MiddleName)
                    || !(x.oldSet.LastName ?? "").Equals(x.newSet.LastName)
                    || !(x.oldSet.IntegrationKey ?? "").Equals(x.newSet.IntegrationKey)
                    || x.oldSet.IsActive != x.newSet.IsActive
                )
                .Select(y => new Data.SystemUser.SystemUser
                {
                    ID = y.oldSet.ID,
                    CompanyID = y.oldSet.CompanyID,
                    Username = y.oldSet.Username,
                    Password = y.oldSet.Password,
                    Salt = y.oldSet.Salt,
                    FirstName = y.newSet.FirstName,
                    MiddleName = y.newSet.MiddleName,
                    LastName = y.newSet.LastName,
                    FailedAttempt = y.oldSet.FailedAttempt,
                    IsPasswordChanged = y.oldSet.IsPasswordChanged,
                    IsLoggedIn = y.oldSet.IsLoggedIn,
                    IntegrationKey = y.newSet.IntegrationKey,
                    IsActive = y.newSet.IsActive,
                    LastLoggedIn = y.oldSet.LastLoggedIn,
                    LastLoggedOut = y.oldSet.LastLoggedOut,
                    LastPasswordChange = y.oldSet.LastPasswordChange,
                    CreatedBy = y.oldSet.CreatedBy,
                    CreatedDate = y.oldSet.CreatedDate,
                    ModifiedBy = y.newSet.ModifiedBy,
                    ModifiedDate = DateTime.Now

                })
                .ToList();
            }
            
            var defaultPassword = (await _dbAccess.GetDefaultPassword()).ToList();
            var systemUsers = (await _dbAccess.GetAllUsers()).ToList();
            var portalGlobalUsers = (await _tbl_usersDBAccess.Get()).ToList();

            var converted = portalGlobalUsers.Select(x => 
            new Data.SystemUser.SystemUser { 
                CompanyID = 1,
                Username = x.username,
                Password = defaultPassword.FirstOrDefault().Password,
                Salt = defaultPassword.FirstOrDefault().Salt,
                LastName = x.password.Count(x => (x.ToString() == ",")) > 0 ? x.password.Split(',')[0] : "",
                FirstName = x.password.Count(x => (x.ToString() == ",")) > 0 ? x.password.Split(',')[1] : "",
                MiddleName = x.password.Count(x => (x.ToString() == ",")) > 1 ? x.password.Split(',')[2] : "",
                IntegrationKey = "" + x.uid,
                IsActive = x.status.Equals("true"),
                CreatedBy = credentials.UserID
            }).ToList();

            List<Data.SystemUser.SystemUser> ValueToAdd = GetToAdd(converted, systemUsers);
            List<Data.SystemUser.SystemUser> ValueToUpdate = GetToUpdate(systemUsers, converted);

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<Data.SystemUser.SystemUser>(), ValueToAdd, ValueToUpdate);
                return new OkObjectResult(string.Concat("Portal Global ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED));
            }
            else
                return new BadRequestObjectResult(string.Concat("Portal Global ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
        }

        public async Task<IActionResult> ChangePassword(APICredentials credentials, ChangePasswordInput param)
        {
            if (string.IsNullOrEmpty(param.CurrentPassword))
                ErrorMessages.Add(string.Concat("Current Password ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.CurrentPassword = param.CurrentPassword.Trim();
                if (param.CurrentPassword.Length > 200)
                    ErrorMessages.Add(string.Concat("Current Password", MessageUtilities.COMPARE_NOT_EXCEED, "200 characters."));
            }

            if (string.IsNullOrEmpty(param.NewPassword))
                ErrorMessages.Add(string.Concat("New Password ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.NewPassword = param.NewPassword.Trim();
                if (param.NewPassword.Length > 200)
                    ErrorMessages.Add(string.Concat("New Password", MessageUtilities.COMPARE_NOT_EXCEED, "200 characters."));
            }

            if (string.IsNullOrEmpty(param.ConfirmNewPassword))
                ErrorMessages.Add(string.Concat("Confirm New Password ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.ConfirmNewPassword = param.ConfirmNewPassword.Trim();
                if (param.ConfirmNewPassword.Length > 200)
                    ErrorMessages.Add(string.Concat("Confirm New Password", MessageUtilities.COMPARE_NOT_EXCEED, "200 characters."));
            }
            
            var systemUser = (await _dbAccess.GetByID(credentials.UserID));

            if (ErrorMessages.Count == 0)
            {
                var encryptedPassword = (await _dbAccess.EncryptPassword(param.CurrentPassword)).FirstOrDefault();
                if (!systemUser.Password.Equals(encryptedPassword.Password)) {
                    ErrorMessages.Add(string.Concat(MessageUtilities.ERRMSG_CURRENT_NOT_MATCHED));
                }

                if (!param.NewPassword.Equals(param.ConfirmNewPassword))
                {
                    ErrorMessages.Add(string.Concat(MessageUtilities.ERRMSG_NEW_CONFIRM_NOT_MATCHED));
                }
            }

            if (ErrorMessages.Count == 0)
            {
                var newEncryptedPassword = (await _dbAccess.EncryptPassword(param.NewPassword)).FirstOrDefault();
                systemUser.Password = newEncryptedPassword.Password;
                systemUser.IsPasswordChanged = true;
                systemUser.LastPasswordChange = DateTime.Now;

                await _dbAccess.ChangePassword(systemUser);
                return new OkObjectResult(MessageUtilities.SSCSMSG_CHANGED_PASS);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarSystemUser> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Username = x.Username,
                Name = x.Name,
                Status = x.Status,
                DateModified = x.DateModified,
                DateCreated = x.DateCreated
            }).ToList());
        }

        public async Task<IActionResult> UpdateSystemUserRole(APICredentials credentials, Form param)
        {
            param.FirstName = param.FirstName.Trim();
            param.MiddleName = param.MiddleName.Trim();
            param.LastName = param.LastName.Trim();

            if (string.IsNullOrEmpty(param.FirstName))
                ErrorMessages.Add("First Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("First Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.MiddleName))
                ErrorMessages.Add("Middle Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.LastName))
                ErrorMessages.Add("Last Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("Last Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));


            if (ErrorMessages.Count == 0)
            {
                static List<SystemUserRole> GetToAdd(List<SystemUserRole> left, List<SystemUserRole> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.UserID, x.RoleID },
                        y => new { y.UserID, y.RoleID },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new SystemUserRole
                        {
                            UserID = x.newSet.newSet.UserID,
                            RoleID = x.newSet.newSet.RoleID,
                            CreatedBy = x.newSet.newSet.CreatedBy
                        })
                    .ToList();
                }

                static List<SystemUserRole> GetToDelete(List<SystemUserRole> left, List<SystemUserRole> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.UserID, x.RoleID },
                        y => new { y.UserID, y.RoleID },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new SystemUserRole
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                var systemRoles = (await _dbAccess.GetRoles(param.ID)).ToList();

                var converted = param.RoleIDs.Select(x =>
                new SystemUserRole
                {
                    UserID = param.ID,
                    RoleID = x,
                    CreatedBy = credentials.UserID
                }).ToList();

                List<SystemUserRole> ValueToAdd = GetToAdd(converted, systemRoles);
                List<SystemUserRole> ValueToDelete = GetToDelete(systemRoles, converted);

                Data.SystemUser.SystemUser systemUser = await _dbAccess.GetByID(param.ID);
                systemUser.FirstName = param.FirstName;
                systemUser.MiddleName = param.MiddleName;
                systemUser.LastName = param.LastName;
                systemUser.IsActive = param.IsActive;

                await _dbAccess.UpdateSystemUserRole(systemUser, ValueToAdd, ValueToDelete);

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
            {
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> AddSystemUserRole(APICredentials credentials, Form param)
        {
            if (string.IsNullOrEmpty(param.Username))
                ErrorMessages.Add(string.Concat("Username ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {

                param.Username = param.Username.Trim();
                if (param.Username.Length > 50)
                    ErrorMessages.Add(string.Concat("Username", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                else
                {
                    var systemUser = await _dbAccess.GetByUsername(param.Username);
                    if (systemUser != null)
                    {
                        ErrorMessages.Add(string.Concat("Username ", MessageUtilities.SUFF_ERRMSG_REC_EXISTS));
                    }
                }
            }
            
            if (string.IsNullOrEmpty(param.LastName))
                ErrorMessages.Add(string.Concat("LastName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.LastName = param.LastName.Trim();
                if (param.LastName.Length > 50)
                    ErrorMessages.Add(string.Concat("LastName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }
            
            if (string.IsNullOrEmpty(param.FirstName))
                ErrorMessages.Add(string.Concat("FirstName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.FirstName = param.FirstName.Trim();
                if (param.FirstName.Length > 50)
                    ErrorMessages.Add(string.Concat("FirstName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (string.IsNullOrEmpty(param.MiddleName))
                ErrorMessages.Add(string.Concat("MiddleName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.MiddleName = param.MiddleName.Trim();
                if (param.MiddleName.Length > 50)
                    ErrorMessages.Add(string.Concat("MiddleName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (ErrorMessages.Count == 0)
            {
                var defaultPassword = (await _dbAccess.GetDefaultPassword()).FirstOrDefault();

                await _dbAccess.AddSystemUserRole(
                    new Data.SystemUser.SystemUser { 
                    CompanyID = 1,
                    Username = param.Username,
                    Password = defaultPassword.Password,
                    Salt = defaultPassword.Salt,
                    FirstName = param.FirstName,
                    MiddleName = param.MiddleName,
                    LastName = param.LastName,
                    IsActive = param.IsActive,
                    CreatedBy = credentials.UserID
                    }, param.RoleIDs.Select(x => new SystemUserRole { UserID = 0, RoleID = x, CreatedBy = credentials.UserID }).ToList());
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetSystemUserRoleDropDownByUserID(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                SharedUtilities.GetDropdown((await _dbAccess.GetUserRoleByUserID(ID)).ToList(), "RoleID", "RoleID", "", ID)
            );
        }

        public async Task<IActionResult> BatchResetPassword(APICredentials credentials, BatchResetPasswordForm param)
        {   
            if (param.SystemUserIDs == null)
                ErrorMessages.Add(string.Concat("SystemUserIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.SystemUserIDs.Count == 0)
                ErrorMessages.Add(string.Concat("SystemUserIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                var systemUsers = (await _dbAccess.GetByIDs(param.SystemUserIDs));
                var defaultPassword = (await _dbAccess.GetDefaultPassword()).FirstOrDefault();

                await _dbAccess.Put(
                    systemUsers.Select(x => new Data.SystemUser.SystemUser { 
                        ID = x.ID,
                        CompanyID = x.CompanyID,
                        Username = x.Username,
                        Password = defaultPassword.Password,
                        Salt = defaultPassword.Salt,
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        FailedAttempt = x.FailedAttempt,
                        IsPasswordChanged = false, // x.IsPasswordChanged,
                        IsLoggedIn = x.IsLoggedIn,
                        IntegrationKey = x.IntegrationKey,
                        IsActive = x.IsActive,
                        LastLoggedIn = x.LastLoggedIn,
                        LastLoggedOut = x.LastLoggedOut,
                        LastPasswordChange = x.LastPasswordChange,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                        
                    }).ToList());
                return new OkObjectResult(MessageUtilities.SSCSMSG_RESET_PASS);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> EmployeeUploadInsert(APICredentials credentials, List<EmployeeUploadInsertInput> param)
        {

            foreach (var item in param)
            {
                item.NewEmployeeCode = (item.NewEmployeeCode ?? "").Trim();
                item.FirstName = (item.FirstName ?? "").Trim();
                item.MiddleName = (item.MiddleName ?? "").Trim();
                item.LastName = (item.LastName ?? "").Trim();

                if (string.IsNullOrEmpty(item.NewEmployeeCode))
                    ErrorMessages.Add(string.Concat("New Employee Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (item.NewEmployeeCode.Length > 50)
                    ErrorMessages.Add(string.Concat("New Employee Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                if (string.IsNullOrEmpty(item.FirstName))
                    ErrorMessages.Add(string.Concat("FirstName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (item.FirstName.Length > 50)
                    ErrorMessages.Add(string.Concat("FirstName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                if (!string.IsNullOrEmpty(item.MiddleName))
                    if (item.MiddleName.Length > 50)
                        ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                if (string.IsNullOrEmpty(item.LastName))
                    ErrorMessages.Add(string.Concat("LastName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (item.LastName.Length > 50)
                    ErrorMessages.Add(string.Concat("LastName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters.")); 
            }

            List<Transfer.SystemUser.EmployeeUploadInsertOutput> result = new List<Transfer.SystemUser.EmployeeUploadInsertOutput>();

            if (ErrorMessages.Count == 0)
            {
                foreach (var item in param)
                {
                    List<Data.SystemUser.SystemUser> systemUser = new List<Data.SystemUser.SystemUser>();
                    systemUser.AddRange((await _dbAccess.AddSystemUser(new GetByNameInput
                    {
                        EmployeeCode = item.NewEmployeeCode,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        CreatedBy = credentials.UserID
                    })).ToList());
                    
                    result.Add(new EmployeeUploadInsertOutput {
                        SystemUserID = systemUser.FirstOrDefault().ID,
                        NewEmployeeCode = systemUser.FirstOrDefault().Username
                    });
                }
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
            {
                if (new List<Data.SystemUser.SystemUser>() == null)
                    return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
                else
                    return new OkObjectResult(result);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> UpdateUsername(APICredentials credentials, UpdateUsername param)
        {
            if (string.IsNullOrEmpty(param.Username))
                ErrorMessages.Add("Username " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Username.Length > 50)
                ErrorMessages.Add(string.Concat("Username", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));


            if (ErrorMessages.Count == 0)
            {
                var systemUser = (await _dbAccess.GetByID(param.ID));
                if (systemUser != null)
                {
                    systemUser.Username = param.Username;
                    systemUser.ModifiedBy = credentials.UserID;
                    systemUser.ModifiedDate = DateTime.Now;

                    await _dbAccess.Put(systemUser); 
                }
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
            {
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> ChangeStatus(APICredentials credentials, ChangeStatusForm param)
        {
            if (param.SystemUserIDs == null)
                ErrorMessages.Add(string.Concat("SystemUserIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.SystemUserIDs.Count == 0)
                ErrorMessages.Add(string.Concat("SystemUserIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                var systemUsers = (await _dbAccess.GetByIDs(param.SystemUserIDs));

                foreach(var obj in systemUsers.Select(x => x.ID))
                {
                    await _dbAccess.Put(
                        systemUsers.Where(x => x.ID == obj).Select(x => new Data.SystemUser.SystemUser
                        {
                            ID = x.ID,
                            CompanyID = x.CompanyID,
                            Username = x.Username,
                            Password = x.Password,
                            Salt = x.Salt,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.LastName,
                            FailedAttempt = x.FailedAttempt,
                            IsPasswordChanged = x.IsPasswordChanged,
                            IsLoggedIn = x.IsLoggedIn,
                            IntegrationKey = x.IntegrationKey,
                            IsActive = x.IsActive == true ? false : true,
                            LastLoggedIn = x.LastLoggedIn,
                            LastLoggedOut = x.LastLoggedOut,
                            LastPasswordChange = x.LastPasswordChange,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            ModifiedBy = credentials.UserID,
                            ModifiedDate = DateTime.Now

                        }).ToList());
                }

                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> ForceChangePassword(APICredentials credentials, ForceChangePasswordInput param)
        {
            if (string.IsNullOrEmpty(param.NewPassword))
                ErrorMessages.Add(string.Concat("New Password ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.NewPassword = param.NewPassword.Trim();

                if (param.NewPassword.Length < 8)
                    ErrorMessages.Add(string.Concat(MessageUtilities.ERRMSG_NEW_PASS_MIN));

                if (param.NewPassword.Length > 200)
                    ErrorMessages.Add(string.Concat("New Password", MessageUtilities.COMPARE_NOT_EXCEED, "200 characters."));
            }

            if (string.IsNullOrEmpty(param.ConfirmNewPassword))
                ErrorMessages.Add(string.Concat("Confirm New Password ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.ConfirmNewPassword = param.ConfirmNewPassword.Trim();
                if (param.ConfirmNewPassword.Length > 200)
                    ErrorMessages.Add(string.Concat("Confirm New Password", MessageUtilities.COMPARE_NOT_EXCEED, "200 characters."));
            }

            var systemUser = (await _dbAccess.GetByID(credentials.UserID));

            if (ErrorMessages.Count == 0)
            {
                var encryptedPassword = (await _dbAccess.EncryptPassword(param.NewPassword)).FirstOrDefault();
                if (systemUser.Password.Equals(encryptedPassword.Password))
                {
                    ErrorMessages.Add(string.Concat(MessageUtilities.ERRMSG_NEW_PASS_NOT_EQUAL_DEFAULT));
                }

                if (!param.NewPassword.Equals(param.ConfirmNewPassword))
                {
                    ErrorMessages.Add(string.Concat(MessageUtilities.ERRMSG_NEW_CONFIRM_NOT_MATCHED));
                }
            }

            if (ErrorMessages.Count == 0)
            {
                var newEncryptedPassword = (await _dbAccess.EncryptPassword(param.NewPassword)).FirstOrDefault();
                systemUser.Password = newEncryptedPassword.Password;
                systemUser.IsPasswordChanged = true;
                systemUser.LastPasswordChange = DateTime.Now;

                await _dbAccess.ChangePassword(systemUser);
                return new OkObjectResult(MessageUtilities.SSCSMSG_CHANGED_PASS);
            }
            
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> SyncFromH2Pay(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            List<H2Pay.Data.SystemUser.spSystemUserGetAllSync_Result> param =
                (await _H2PaySystemUserDBAccess.GetSync(unit, value)).ToList();

            static List<Data.SystemUser.SystemUser> GetToAdd(List<Data.SystemUser.SystemUser> left, List<Data.SystemUser.SystemUser> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { x.IntegrationKey },
                    y => new { y.IntegrationKey },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.SystemUser.SystemUser
                    {
                        IntegrationKey = x.newSet.newSet.IntegrationKey,
                        Username = x.newSet.newSet.Username,
                        Password = x.newSet.newSet.Password,
                        FirstName = x.newSet.newSet.FirstName,
                        MiddleName = x.newSet.newSet.MiddleName,
                        LastName = x.newSet.newSet.LastName,
                        IsPasswordChanged = x.newSet.newSet.IsPasswordChanged,
                        FailedAttempt = x.newSet.newSet.FailedAttempt,
                        IsActive = x.newSet.newSet.IsActive,
                    })
                .ToList();
            }

            static List<Data.SystemUser.SystemUser> GetToUpdate(List<Data.SystemUser.SystemUser> left, List<Data.SystemUser.SystemUser> right)
            {
                return left.Join(
                right,
                x => new { x.IntegrationKey },
                y => new { y.IntegrationKey },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Username.Equals(x.newSet.Username)
                    || !x.oldSet.Password.Equals(x.newSet.Password)
                    || !x.oldSet.FirstName.Equals(x.newSet.FirstName)
                    || !x.oldSet.MiddleName.Equals(x.newSet.MiddleName)
                    || !x.oldSet.LastName.Equals(x.newSet.LastName)
                    || x.oldSet.FailedAttempt != x.newSet.FailedAttempt
                    || x.oldSet.IsPasswordChanged != x.newSet.IsPasswordChanged
                    || x.oldSet.IsActive != x.newSet.IsActive
                )
                .Select(y => new Data.SystemUser.SystemUser
                {
                    ID = y.oldSet.ID,
                    CompanyID = y.oldSet.CompanyID,
                    Salt = y.oldSet.Salt,
                    IsLoggedIn = y.oldSet.IsLoggedIn,
                    LastLoggedIn = y.oldSet.LastLoggedIn,
                    LastLoggedOut = y.oldSet.LastLoggedOut,
                    LastPasswordChange = y.oldSet.LastPasswordChange,
                    CreatedBy = y.oldSet.CreatedBy,
                    CreatedDate = y.oldSet.CreatedDate,
                    ModifiedBy = null,
                    ModifiedDate = DateTime.Now,

                    IntegrationKey = y.newSet.IntegrationKey,
                    Username = y.newSet.Username,
                    Password = y.newSet.Password,
                    FirstName = y.newSet.FirstName,
                    MiddleName = y.newSet.MiddleName,
                    LastName = y.newSet.LastName,
                    FailedAttempt = y.newSet.FailedAttempt,
                    IsPasswordChanged = y.newSet.IsPasswordChanged,
                    IsActive = y.newSet.IsActive
                })
                .ToList();
            }

            List<Data.SystemUser.SystemUser> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.Id.ToString()).ToList())).ToList();

            List<Data.SystemUser.SystemUser> converted =
                param.Select(x => new Data.SystemUser.SystemUser
                {
                    IntegrationKey = x.Id.ToString(),
                    Username = x.Username,
                    Password = x.Password,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    IsPasswordChanged = x.IsPasswordChanged,
                    IsActive = x.IsActive
                }).ToList();


            List<Data.SystemUser.SystemUser> ValueToAdd = GetToAdd(converted, localCopy)
            .GroupBy(x => x.IntegrationKey)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<Data.SystemUser.SystemUser> ValueToUpdate = GetToUpdate(localCopy, converted)
            .GroupBy(x => x.IntegrationKey)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<Data.SystemUser.SystemUser>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added SystemUser record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated SystemUser record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage.ToString());
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("SystemUser ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }


        public async Task<IActionResult> DisableEmployeeAccount(APICredentials credentials, UpdateSystemUser param)
        {
            if (param.Username == null)
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                var systemUsers = (await _dbAccess.GetByUsername(param.Username));
                if (systemUsers != null)
                {
                    systemUsers.IsActive = param.IsActive;
                    systemUsers.ModifiedBy = credentials.UserID;
                    systemUsers.ModifiedDate = DateTime.Now;

                    await _dbAccess.Put(systemUsers);
                }

                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }

            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> ResetPassword(APICredentials credentials, ResetPassword param)
        {
            if (param.ID == 0)
                ErrorMessages.Add(string.Concat("Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (param.NewPassword == null)
                ErrorMessages.Add(string.Concat("New Password ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            if (ErrorMessages.Count == 0)
            {
                var systemUser = (await _dbAccess.GetByID(param.ID));
                var newEncryptedPassword = (await _dbAccess.EncryptPassword(param.NewPassword)).FirstOrDefault();
                systemUser.Password = newEncryptedPassword.Password;
                systemUser.IsPasswordChanged = true;
                systemUser.LastPasswordChange = DateTime.Now;
                systemUser.ModifiedBy = credentials.UserID;
                systemUser.ModifiedDate = DateTime.Now;
                if (param.ForceChangePassword)
                    systemUser.IsPasswordChanged = false;

                await _dbAccess.ChangePassword(systemUser);
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_SAVE);
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetUserByRoleIDs(APICredentials credentials, List<int> ID)
        {
            var GetUser = (await _dbAccess.GetUserByRoleIDs(ID)).Select(x=>x.UserID).ToList();

            return new OkObjectResult(await _dbAccess.GetByIDs(GetUser));
        }
    }
}
