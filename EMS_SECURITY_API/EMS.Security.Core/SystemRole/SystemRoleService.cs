using EMS.Security.Data.DBContexts;
using EMS.Security.Data.SystemRole;
using EMS.Security.Transfer.SystemRole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Security.Core.SystemRole
{
    public interface ISystemRoleService
    {
        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetDropDown(APICredentials credentials);

        Task<IActionResult> GetByUserID(APICredentials credentials);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> GetSystemUserRoleDropDownByRoleID(APICredentials credentials, int ID);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetRolePage(APICredentials credentials, int ID);
    }
    public class SystemRoleService : EMS.Security.Core.Shared.Utilities, ISystemRoleService
    {
        private readonly ISystemRoleDBAccess _dbAccess;

        public SystemRoleService(SystemAccessContext dbContext, IConfiguration iconfiguration,
            ISystemRoleDBAccess dbAccess) :base (dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials ,GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param.Term, param.TopResults, param.CompanyID))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.RoleName
                })
            );
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials)
        {

            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.RoleName).ToList(), "ID", "RoleName")
                );
        }

        public async Task<IActionResult> GetByUserID(APICredentials credentials)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByUserID(credentials.UserID))
                .Select(x => new GetByUserIDOutput
                { 
                    UserID = x.UserID,
                    RoleID = x.RoleID
                }).ToList()
                );
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarSystemRole> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                RoleName = x.RoleName,
                DateCreated = x.DateCreated
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.SystemRole.SystemRole result = await _dbAccess.GetByID(ID);
            List<TableVarSystemRoleAccess> accessList = (await _dbAccess.GetAccess(ID)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    RoleName = result.RoleName,
                    CreatedBy = result.CreatedBy,
                    SystemRoleAccessList = accessList.Select(x => new SystemRoleAccess { 
                        ID = x.ID,
                        ParentCode = x.ParentCode,
                        ParentPageID = x.ParentPageID,
                        PageID = x.PageID,
                        Title = x.Title,
                        Description = x.Description,
                        FunctionType = x.FunctionType,
                        HasAccess = x.HasAccess,
                    }).ToList()
                });
        }

        public async Task<IActionResult> GetSystemUserRoleDropDownByRoleID(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                SharedUtilities.GetDropdown((await _dbAccess.GetByRoleID(ID)).ToList(), "UserID", "UserID", "", ID)
            );
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            if (string.IsNullOrEmpty(param.RoleName))
                ErrorMessages.Add("Role Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.RoleName = param.RoleName.Trim();

                if (param.RoleName.Length > 50)
                    ErrorMessages.Add(string.Concat("Role Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                else
                {
                    List<Data.SystemRole.SystemRole> form = (await _dbAccess.GetByRoleName(param.RoleName)).ToList();

                    if (form.Count() > 0)
                    {
                        ErrorMessages.Add("Role Name " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                    }
                }
            }


            if (param.SystemRoleAccessList != null)
            {
                if(param.SystemRoleAccessList.Count == 0)
                    ErrorMessages.Add("System Role Access" + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            }               
            else
                ErrorMessages.Add("System Role Access " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);


            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.SystemRole.SystemRole
                {
                    CompanyID = 1,
                    RoleName = param.RoleName,
                    IsActive = true,
                    CreatedBy = param.CreatedBy
                }
                , param.SystemRoleAccessList?.Select(x => new SystemRolePage
                {
                    PageID = x.PageID,
                    FunctionType = x.FunctionType,
                    RoleID = 0, // To be populated on DBAccess
                    IsHidden = false,
                    CreatedBy = param.CreatedBy
                }).ToList()
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            if (string.IsNullOrEmpty(param.RoleName))
                ErrorMessages.Add("Role Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.RoleName = param.RoleName.Trim();

                if (param.RoleName.Length > 50)
                    ErrorMessages.Add(string.Concat("Role Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                else
                {
                    List<Data.SystemRole.SystemRole> form = (await _dbAccess.GetByRoleName(param.RoleName)).ToList();

                    if (form.Where(x => x.ID != param.ID).Count() > 0)
                    {
                        ErrorMessages.Add("Role Name " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                    }
                }
            }

            if (param.SystemRoleAccessList != null)
            {
                if (param.SystemRoleAccessList.Count == 0)
                    ErrorMessages.Add("System Role Access" + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            }
            else
                ErrorMessages.Add("System Role Access " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);


            if (ErrorMessages.Count == 0)
            {
                static IEnumerable<SystemRolePage> GetToAdd(List<SystemRolePage> left, List<SystemRolePage> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.RoleID, x.PageID, x.FunctionType },
                             y => new { y.RoleID, y.PageID, y.FunctionType },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new SystemRolePage
                            {
                                PageID = x.newSet.newSet.PageID,
                                FunctionType = x.newSet.newSet.FunctionType,
                                RoleID = x.newSet.newSet.RoleID,
                                CreatedBy = x.newSet.newSet.CreatedBy
                            }).ToList();
                }

                static IEnumerable<SystemRolePage> GetToDelete(List<SystemRolePage> left, List<SystemRolePage> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.RoleID, x.PageID, x.FunctionType },
                             y => new { y.RoleID, y.PageID, y.FunctionType },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new SystemRolePage
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                List<SystemRolePage> OldSystemRolePage = (await _dbAccess.GetPage(param.ID)).ToList();

                List<SystemRolePage> SystemRolePageToAdd = GetToAdd(OldSystemRolePage,
                    param.SystemRoleAccessList == null ? new List<SystemRolePage>() :
                    param.SystemRoleAccessList.Select(x => new SystemRolePage
                    {
                        FunctionType = x.FunctionType,
                        PageID = x.PageID,
                        RoleID = param.ID,
                        CreatedBy = credentials.UserID
                    }).ToList()).ToList();

                List<SystemRolePage> SystemRolePageToDelete = GetToDelete(OldSystemRolePage,
                    param.SystemRoleAccessList == null ? new List<SystemRolePage>() :
                    param.SystemRoleAccessList.Select(x => new SystemRolePage
                    {
                        FunctionType = x.FunctionType,
                        PageID = x.PageID,
                        RoleID = param.ID
                    }).ToList()).ToList();

                Data.SystemRole.SystemRole SystemRoleData = await _dbAccess.GetByID(param.ID);
                SystemRoleData.CompanyID = 1;
                SystemRoleData.RoleName = param.RoleName;
                SystemRoleData.IsActive = true;
                SystemRoleData.ModifiedBy = credentials.UserID;
                SystemRoleData.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(
                     SystemRoleData
                    , SystemRolePageToAdd
                    , SystemRolePageToDelete
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            EMS.Security.Data.SystemRole.SystemRole systemRole = await _dbAccess.GetByID(ID);
            systemRole.IsActive = false;
            systemRole.ModifiedBy = credentials.UserID;
            systemRole.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(systemRole))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> GetRolePage(APICredentials credentials, int ID)
        {
            List<TableVarSystemRoleAccess> accessList = (await _dbAccess.GetAccess(ID)).ToList();


            if (accessList == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                accessList.Select(x => new SystemRoleAccess
                {
                    ID = x.ID,
                    ParentCode = x.ParentCode,
                    ParentPageID = x.ParentPageID,
                    PageID = x.PageID,
                    Title = x.Title,
                    Description = x.Description,
                    FunctionType = x.FunctionType,
                    HasAccess = x.HasAccess,
                }).ToList());
            
        }
    }
}
