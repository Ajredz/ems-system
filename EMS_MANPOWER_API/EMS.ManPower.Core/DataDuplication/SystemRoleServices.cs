using EMS.Manpower.Data.DataDuplication.SystemRole;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.DataDuplication.SystemRole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.Core.DataDuplication
{
    public interface ISystemRoleService
    {
        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetDropDown(APICredentials credentials);

        //Task Sync(List<SystemRole> param);

        Task<IActionResult> Sync(List<SystemRole> param);
    }

    public class SystemRoleService : EMS.Manpower.Core.Shared.Utilities, ISystemRoleService
    {
        private readonly ISystemRoleDBAccess _dbAccess;

        public SystemRoleService(ManpowerContext dbContext, IConfiguration iconfiguration,
            ISystemRoleDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param.Term, param.TopResults, param.CompanyID))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.SyncID,
                    Description = x.RoleName
                })
            );
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "RoleName")
                );
        }

        //public async Task Sync(List<SystemRole> param)
        //{

        //    static List<SystemRole> GetToAdd(List<SystemRole> left, List<SystemRole> right)
        //    {
        //        return right.GroupJoin(
        //            left,
        //            x => new { x.SyncID },
        //            y => new { y.SyncID },
        //        (x, y) => new { newSet = x, oldSet = y })
        //        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
        //        (x, y) => new { newSet = x, oldSet = y })
        //        .Where(x => x.oldSet == null)
        //        .Select(x =>
        //            new SystemRole
        //            {
        //                SyncID = x.newSet.newSet.SyncID,
        //                SyncDate = DateTime.Now,
        //                RoleName = x.newSet.newSet.RoleName,
        //                CompanyID = x.newSet.newSet.CompanyID,
        //                IsActive = x.newSet.newSet.IsActive,
        //            })
        //        .ToList();
        //    }

        //    static List<SystemRole> GetToUpdate(List<SystemRole> left, List<SystemRole> right)
        //    {
        //        return left.Join(
        //        right,
        //        x => new { x.SyncID },
        //        y => new { y.SyncID },
        //        (x, y) => new { oldSet = x, newSet = y })
        //        .Where(x => !x.oldSet.RoleName.Equals(x.newSet.RoleName)
        //        || x.oldSet.IsActive != x.newSet.IsActive)
        //        .Select(y => new SystemRole
        //        {
        //            ID = y.oldSet.ID,
        //            SyncID = y.oldSet.SyncID,
        //            SyncDate = DateTime.Now,
        //            RoleName = y.newSet.RoleName,
        //            CompanyID = y.newSet.CompanyID,
        //            IsActive = y.newSet.IsActive,
        //        })
        //        .GroupBy(x => x.SyncID)
        //        .Select(y => y.FirstOrDefault())
        //        .ToList();
        //    }

        //    List<SystemRole> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.SyncID).ToList())).ToList();
        //    //List<SystemRole> ValueToDelete = GetToDelete(localCopy, param).ToList();
        //    List<SystemRole> ValueToAdd = GetToAdd(localCopy, param)
        //    .GroupBy(x => x.SyncID)
        //    .Select(y => y.FirstOrDefault())
        //    .ToList();

        //    List<SystemRole> ValueToUpdate = GetToUpdate(localCopy, param)
        //    .GroupBy(x => x.SyncID)
        //    .Select(y => y.FirstOrDefault())
        //    .ToList();

        //    if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
        //        await _dbAccess.Sync(new List<SystemRole>(), ValueToAdd, ValueToUpdate);
        //}

        public async Task<IActionResult> Sync(List<SystemRole> param)
        {
            static List<SystemRole> GetToAdd(List<SystemRole> left, List<SystemRole> right)
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
                    new SystemRole
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        RoleName = x.newSet.newSet.RoleName,
                        CompanyID = x.newSet.newSet.CompanyID,
                        IsActive = x.newSet.newSet.IsActive,
                    })
                .ToList();
            }

            static List<SystemRole> GetToUpdate(List<SystemRole> left, List<SystemRole> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.RoleName.Equals(x.newSet.RoleName)
                    || x.oldSet.CompanyID != x.newSet.CompanyID
                    || x.oldSet.IsActive != x.newSet.IsActive
                )
                .Select(y => new SystemRole
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    RoleName = y.newSet.RoleName,
                    CompanyID = y.newSet.CompanyID,
                    IsActive = y.newSet.IsActive,
                })
                .ToList();
            }

            List<SystemRole> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            //List<SystemRole> ValueToDelete = GetToDelete(localCopy, param).ToList();
            List<SystemRole> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<SystemRole> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<SystemRole>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added SystemRole record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated SystemRole record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage.ToString());
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("SystemRole ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }



    }
}