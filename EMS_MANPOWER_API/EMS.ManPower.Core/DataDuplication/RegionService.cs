using EMS.Manpower.Data.DataDuplication.Region;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.DataDuplication.Region;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.Core.DataDuplication
{
    public interface IRegionService
    {
        Task Sync(List<Region> param);

        Task<List<Region>> GetBySyncIDs(List<int> IDs);
    }

    public class RegionService : Core.Shared.Utilities, IRegionService
    {
        private readonly IRegionDBAccess _dbAccess;

        public RegionService(ManpowerContext dbContext, IConfiguration iconfiguration,
            IRegionDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task Sync(List<Region> param)
        {

            List<Region> GetToAdd(List<Region> left, List<Region> right)
            {
                return right.GroupJoin(
                    left,
                    x => new { x.SyncID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Region
                    {
                        SyncID = x.newSet.newSet.SyncID,
                        SyncDate = DateTime.Now,
                        Code = x.newSet.newSet.Code,
                        Description = x.newSet.newSet.Description,

                    })
                .ToList();
            }

            List<Region> GetToUpdate(List<Region> left, List<Region> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { y.SyncID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                    || !x.oldSet.Description.Equals(x.newSet.Description)
                )
                .Select(y => new Region
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    Code = y.newSet.Code,
                    Description = y.newSet.Description,
                })
                .GroupBy(x => x.SyncID)
                .Select(y => y.FirstOrDefault())
                .ToList();
            }

            List<Region> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.SyncID).ToList())).ToList();
            //List<Region> ValueToDelete = GetToDelete(localCopy, param).ToList();
            List<Region> ValueToAdd = GetToAdd(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<Region> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
                await _dbAccess.Sync(new List<Region>(), ValueToAdd, ValueToUpdate);
        }

        public async Task<List<Region>> GetBySyncIDs(List<int> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }

    }
}