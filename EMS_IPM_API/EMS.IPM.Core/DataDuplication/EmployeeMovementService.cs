using EMS.IPM.Data.DataDuplication.EmployeeMovement;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.EmployeeMovement;
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
    public interface IEmployeeMovementService
    {
        Task<IActionResult> Sync(List<EmployeeMovement> param);

        Task<List<EmployeeMovement>> GetBySyncIDs(List<long> IDs);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);
    }

    public class EmployeeMovementService : Core.Shared.Utilities, IEmployeeMovementService
    {
        private readonly IEmployeeMovementDBAccess _dbAccess;

        public EmployeeMovementService(IPMContext dbContext, IConfiguration iconfiguration,
            IEmployeeMovementDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Sync(List<EmployeeMovement> param)
        {
            static List<EmployeeMovement> GetToAdd(List<EmployeeMovement> left, List<EmployeeMovement> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { SyncID = x.ID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Where(x => x.newSet.newSet.EmployeeField == "POSITION" 
                         || x.newSet.newSet.EmployeeField == "ORG_GROUP"
                         || x.newSet.newSet.EmployeeField == "EMPLOYMENT_STATUS"
                         || x.newSet.newSet.EmployeeField == "SECONDARY_DESIG"
                         )
                .Select(x =>
                    new EmployeeMovement
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        EmployeeID = x.newSet.newSet.EmployeeID,
                        EmployeeField = x.newSet.newSet.EmployeeField,
                        MovementType = x.newSet.newSet.MovementType,
                        From = x.newSet.newSet.From,
                        FromID = x.newSet.newSet.FromID,
                        To = x.newSet.newSet.To,
                        ToID = x.newSet.newSet.ToID,
                        DateEffectiveFrom = x.newSet.newSet.DateEffectiveFrom,
                        DateEffectiveTo = x.newSet.newSet.DateEffectiveTo,
                        HRDComments = x.newSet.newSet.HRDComments,
                        IsLatest = x.newSet.newSet.IsLatest,
                        IsActive = x.newSet.newSet.IsActive,
                        CreatedBy = x.newSet.newSet.CreatedBy,
                        CreatedDate = x.newSet.newSet.CreatedDate,
                        Reason = x.newSet.newSet.Reason,
                        ModifiedBy = x.newSet.newSet.ModifiedBy,
                        ModifiedDate = x.newSet.newSet.ModifiedDate
                    })
                .ToList();
            }

            static List<EmployeeMovement> GetToUpdate(List<EmployeeMovement> left, List<EmployeeMovement> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.EmployeeID != x.newSet.EmployeeID
                    || !x.oldSet.EmployeeField.Equals(x.newSet.EmployeeField)
                    || !x.oldSet.MovementType.Equals(x.newSet.MovementType)
                    || !(x.oldSet.From ?? String.Empty).Equals(x.newSet.From)
                    || !(x.oldSet.FromID ?? String.Empty).Equals(x.newSet.FromID)
                    || !(x.oldSet.To ?? String.Empty).Equals(x.newSet.To)
                    || !(x.oldSet.ToID ?? String.Empty).Equals(x.newSet.FromID)
                    || x.oldSet.DateEffectiveFrom != x.newSet.DateEffectiveFrom
                    || x.oldSet.DateEffectiveTo != x.newSet.DateEffectiveTo
                    || !x.oldSet.HRDComments.Equals(x.newSet.HRDComments)
                    || x.oldSet.IsLatest != x.newSet.IsLatest
                    || x.oldSet.IsActive != x.newSet.IsActive
                    &&
                       x.oldSet.EmployeeField == "POSITION"
                    || x.oldSet.EmployeeField == "ORG_GROUP"
                    || x.oldSet.EmployeeField == "EMPLOYMENT_STATUS"
                    || x.oldSet.EmployeeField == "SECONDARY_DESIG"
                )
                .Select(y => new EmployeeMovement
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    EmployeeID = y.newSet.EmployeeID,
                    EmployeeField = y.newSet.EmployeeField,
                    MovementType = y.newSet.MovementType,
                    From = y.newSet.From,
                    FromID = y.newSet.FromID,
                    To = y.newSet.To,
                    ToID = y.newSet.ToID,
                    DateEffectiveFrom = y.newSet.DateEffectiveFrom,
                    DateEffectiveTo = y.newSet.DateEffectiveTo,
                    HRDComments = y.newSet.HRDComments,
                    IsLatest = y.newSet.IsLatest,
                    IsActive = y.newSet.IsActive,
                    CreatedBy = y.newSet.CreatedBy,
                    CreatedDate = y.newSet.CreatedDate,
                    Reason = y.newSet.Reason,
                    ModifiedBy = y.newSet.ModifiedBy,
                    ModifiedDate = y.newSet.ModifiedDate
                })
                .ToList();
            }

            List<EmployeeMovement> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<EmployeeMovement> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<EmployeeMovement> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<EmployeeMovement>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added EmployeeMovement record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated EmployeeMovement record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("EmployeeMovement ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<EmployeeMovement>> GetBySyncIDs(List<long> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "Code", "Title")
                );
        }
    }
}