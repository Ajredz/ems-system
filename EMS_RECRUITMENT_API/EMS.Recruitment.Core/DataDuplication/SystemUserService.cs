using EMS.Recruitment.Data.DataDuplication.SystemUser;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.DataDuplication.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.DataDuplication
{
    public interface ISystemUserService
    {
        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetBySyncID(APICredentials credentials, int ID);

        Task<IActionResult> Sync(List<SystemUser> param);
    }

    public class SystemUserService : Core.Shared.Utilities, ISystemUserService
    {
        private readonly ISystemUserDBAccess _dbAccess;

        public SystemUserService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            ISystemUserDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Name
                })
            );
        }
        
        public async Task<IActionResult> GetBySyncID(APICredentials credentials, int ID)
        {
            SystemUser result = (await _dbAccess.GetBySyncID(ID));

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(new GetIDByAutoCompleteOutput
                {
                    ID = result.SyncID,
                    Description = result.LastName + ", " + result.FirstName + " " + result.MiddleName
                });
            }
        }

        public async Task<IActionResult> Sync(List<SystemUser> param)
        {
            static List<SystemUser> GetToAdd(List<SystemUser> left, List<SystemUser> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { SyncID = x.ID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new SystemUser
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        FirstName = x.newSet.newSet.FirstName,
                        MiddleName = x.newSet.newSet.MiddleName,
                        LastName = x.newSet.newSet.LastName,
                        IsActive = x.newSet.newSet.IsActive,
                        UserName = x.newSet.newSet.UserName,
                    })
                .ToList();
            }

            static List<SystemUser> GetToUpdate(List<SystemUser> left, List<SystemUser> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.FirstName.Equals(x.newSet.FirstName)
                    || !(x.oldSet.MiddleName ?? "").Equals((x.oldSet.MiddleName ?? ""))
                    || !x.oldSet.LastName.Equals(x.newSet.LastName)
                    || x.oldSet.IsActive != x.newSet.IsActive
                    || !x.oldSet.UserName.Equals(x.newSet.UserName)
                )
                .Select(y => new SystemUser
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    FirstName = y.newSet.FirstName,
                    MiddleName = y.newSet.MiddleName,
                    LastName = y.newSet.LastName,
                    IsActive = y.newSet.IsActive,
                    UserName = y.newSet.UserName,
                })
                .ToList();
            }

            List<SystemUser> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<SystemUser> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<SystemUser> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<SystemUser>(), ValueToAdd, ValueToUpdate);
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

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("SystemUser ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

    }
}