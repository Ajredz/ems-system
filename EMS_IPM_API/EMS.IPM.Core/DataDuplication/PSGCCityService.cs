using EMS.IPM.Data.DataDuplication.PSGCCity;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.PSGCCity;
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
    public interface IPSGCCityService
    {
        Task<IActionResult> Sync(List<PSGCCity> param);

        Task<List<PSGCCity>> GetBySyncIDs(List<int> IDs);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);
    }

    public class PSGCCityService : Core.Shared.Utilities, IPSGCCityService
    {
        private readonly IPSGCCityDBAccess _dbAccess;

        public PSGCCityService(IPMContext dbContext, IConfiguration iconfiguration,
            IPSGCCityDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Sync(List<PSGCCity> param)
        {
            static List<PSGCCity> GetToAdd(List<PSGCCity> left, List<PSGCCity> right)
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
                    new PSGCCity
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        PSGCRegionID = x.newSet.newSet.PSGCRegionID,
                        Code = x.newSet.newSet.Code,
                        Description = x.newSet.newSet.Description
                    })
                .ToList();
            }

            static List<PSGCCity> GetToUpdate(List<PSGCCity> left, List<PSGCCity> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                    || x.oldSet.PSGCRegionID != x.newSet.PSGCRegionID
                    || !x.oldSet.Description.Equals(x.newSet.Description)
                )
                .Select(y => new PSGCCity
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    Code = y.newSet.Code,
                    PSGCRegionID = y.newSet.PSGCRegionID,
                    Description = y.newSet.Description,
                })
                .ToList();
            }

            List<PSGCCity> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<PSGCCity> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<PSGCCity> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<PSGCCity>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added PSGCCity record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated PSGCCity record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("PSGCCity ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<PSGCCity>> GetBySyncIDs(List<int> IDs)
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