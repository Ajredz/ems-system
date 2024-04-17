using EMS.IPM.Data.DataDuplication.PSGCRegion;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.PSGCRegion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.DataDuplication
{
    public interface IPSGCRegionService
    {
        Task<IActionResult> Sync(List<PSGCRegion> param);

        Task<List<PSGCRegion>> GetBySyncIDs(List<int> IDs);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);
    }

    public class PSGCRegionService : Core.Shared.Utilities, IPSGCRegionService
    {
        private readonly IPSGCRegionDBAccess _dbAccess;

        public PSGCRegionService(IPMContext dbContext, IConfiguration iconfiguration,
            IPSGCRegionDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Sync(List<PSGCRegion> param)
        {
            static List<PSGCRegion> GetToAdd(List<PSGCRegion> left, List<PSGCRegion> right)
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
                    new PSGCRegion
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        Code = x.newSet.newSet.Code,
                        Description = x.newSet.newSet.Description
                    })
                .ToList();
            }

            static List<PSGCRegion> GetToUpdate(List<PSGCRegion> left, List<PSGCRegion> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                    || !x.oldSet.Description.Equals(x.newSet.Description)
                )
                .Select(y => new PSGCRegion
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    Code = y.newSet.Code,
                    Description = y.newSet.Description
                })
                .ToList();
            }

            List<PSGCRegion> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<PSGCRegion> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<PSGCRegion> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<PSGCRegion>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added PSGCRegion record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated PSGCRegion record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("PSGCRegion ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<PSGCRegion>> GetBySyncIDs(List<int> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "Code", "Description")
                );
        }
    }
}