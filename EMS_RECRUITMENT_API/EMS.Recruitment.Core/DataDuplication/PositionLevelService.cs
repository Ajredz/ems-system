using EMS.Recruitment.Data.DataDuplication.PositionLevel;
using EMS.Recruitment.Data.DBContexts;
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
    public interface IPositionLevelService
    {
        Task<IActionResult> Sync(List<PositionLevel> param);

        Task<List<PositionLevel>> GetBySyncIDs(List<int> IDs);
    }

    public class PositionLevelService : Core.Shared.Utilities, IPositionLevelService
    {
        private readonly IPositionLevelDBAccess _dbAccess;

        public PositionLevelService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            IPositionLevelDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Sync(List<PositionLevel> param)
        {
            static List<PositionLevel> GetToAdd(List<PositionLevel> left, List<PositionLevel> right)
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
                    new PositionLevel
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        Description = x.newSet.newSet.Description,
                        IsActive = x.newSet.newSet.IsActive,
                    })
                .ToList();
            }

            static List<PositionLevel> GetToUpdate(List<PositionLevel> left, List<PositionLevel> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Description.Equals(x.newSet.Description)
                || x.oldSet.IsActive != x.newSet.IsActive)
                .Select(y => new PositionLevel
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    Description = y.newSet.Description,
                    IsActive = y.newSet.IsActive,
                })
                .ToList();
            }

            List<PositionLevel> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<PositionLevel> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<PositionLevel> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<PositionLevel>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added Position Level record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated Position Level record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("Position Level ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<PositionLevel>> GetBySyncIDs(List<int> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }
    }
}