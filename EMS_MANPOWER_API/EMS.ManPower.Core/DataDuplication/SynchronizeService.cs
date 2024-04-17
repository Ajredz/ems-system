using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Data.DataDuplication.PositionLevel;
using EMS.Manpower.Data.DBContexts;
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
    public interface ISynchronizeService
    {
        Task<IActionResult> SyncPositionLevel(APICredentials credentials);

        Task<IActionResult> SyncPosition(APICredentials credentials);
    }

    public class SynchronizeService : Core.Shared.Utilities, ISynchronizeService
    {
        private readonly IPositionDBAccess _dbAccessPosition;
        private readonly IPositionLevelDBAccess _dbAccessPositionLevel;

        public SynchronizeService(ManpowerContext dbContext, IConfiguration iconfiguration,
            IPositionDBAccess dbAccessPosition,
            IPositionLevelDBAccess dbAccessPositionLevel) : base(dbContext, iconfiguration)
        {
            _dbAccessPosition = dbAccessPosition;
            _dbAccessPositionLevel = dbAccessPositionLevel;
        }

        public async Task<IActionResult> SyncPositionLevel(APICredentials credentials)
        {
            List<Data.DataDuplication.PositionLevel.PositionLevel> GetPositionLevelToDelete(
                List<Data.DataDuplication.PositionLevel.PositionLevel> left, 
                List<Data.DataDuplication.PositionLevel.PositionLevel> right)
            {
                return left.GroupJoin(
                            right,
                            x => new { x.SyncID },
                            y => new { SyncID = y.ID },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new Data.DataDuplication.PositionLevel.PositionLevel
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
            }

            List<Data.DataDuplication.PositionLevel.PositionLevel> GetPositionLevelToAdd(
                List<Data.DataDuplication.PositionLevel.PositionLevel> left, 
                List<Data.DataDuplication.PositionLevel.PositionLevel> right)
            {
                return right.GroupJoin(
                        left,
                        x => new { SyncID = x.ID },
                        y => new { y.SyncID },
                  (x, y) => new { newSet = x, oldSet = y })
                  .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                  (x, y) => new { newSet = x, oldSet = y })
                  .Where(x => x.oldSet == null)
                  .Select(x =>
                      new Data.DataDuplication.PositionLevel.PositionLevel
                      {
                          SyncID = x.newSet.newSet.ID,
                          SyncDate = DateTime.Now,
                          Description = x.newSet.newSet.Description
                      }).ToList();
            }

            List<Data.DataDuplication.PositionLevel.PositionLevel> GetPositionLevelToUpdate(
                List<Data.DataDuplication.PositionLevel.PositionLevel> left, 
                List<Data.DataDuplication.PositionLevel.PositionLevel> right)
            {
                return left.Join(
                           right,
                            x => new { x.SyncID },
                            y => new { SyncID = y.ID },
                           (x, y) => new { oldSet = x, newSet = y })
                           .Where(x => !x.oldSet.Description.Equals(x.newSet.Description))
                           .Select(y => new Data.DataDuplication.PositionLevel.PositionLevel
                           {
                               ID = y.oldSet.ID,
                               SyncID = y.oldSet.SyncID,
                               SyncDate = DateTime.Now,
                               Description = y.newSet.Description
                           }).ToList();
            }

            var PositionLevelURL =
              string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").GetSection("GetAll").Value, "?",
                  "userid=", credentials.UserID);
            var (PositionLevelResult, PositionLevelIsSuccess, PositionLevelErrorMessage) = 
                await SharedUtilities.GetFromAPI(new List<EMS.Manpower.Data.DataDuplication.PositionLevel.PositionLevel>(), PositionLevelURL);

            StringBuilder successMessage = new StringBuilder();

            if (PositionLevelIsSuccess)
            {
                List<Data.DataDuplication.PositionLevel.PositionLevel> localCopy = 
                    (await _dbAccessPositionLevel.GetAll()).ToList();
                List<Data.DataDuplication.PositionLevel.PositionLevel> ValueToDelete = 
                    GetPositionLevelToDelete(localCopy, PositionLevelResult).ToList();
                List<Data.DataDuplication.PositionLevel.PositionLevel> ValueToAdd = 
                    GetPositionLevelToAdd(localCopy, PositionLevelResult).ToList();
                List<Data.DataDuplication.PositionLevel.PositionLevel> ValueToUpdate = 
                    GetPositionLevelToUpdate(localCopy, PositionLevelResult).ToList();

                if (ValueToDelete.Count > 0 || ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
                    await _dbAccessPositionLevel.Sync(ValueToDelete, ValueToAdd, ValueToUpdate);

                if (ValueToDelete.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToDelete.Count
                        , " deleted Position Level record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

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
            }

            return new OkObjectResult(successMessage.Length > 0 ? successMessage.ToString() : 
                string.Concat("Position ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));

        }

        public async Task<IActionResult> SyncPosition(APICredentials credentials)
        {

            List<Data.DataDuplication.Position.Position> GetPositionToDelete(
                List<Data.DataDuplication.Position.Position> left, 
                List<Data.DataDuplication.Position.Position> right)
            {
                return left.GroupJoin(
                            right,
                            x => new { x.SyncID },
                            y => new { SyncID = y.ID },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new Data.DataDuplication.Position.Position
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
            }

            List<Data.DataDuplication.Position.Position> GetPositionToAdd(
                List<Data.DataDuplication.Position.Position> left, 
                List<Data.DataDuplication.Position.Position> right)
            {
                return right.GroupJoin(
                        left,
                        x => new { SyncID = x.ID },
                        y => new { y.SyncID },
                  (x, y) => new { newSet = x, oldSet = y })
                  .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                  (x, y) => new { newSet = x, oldSet = y })
                  .Where(x => x.oldSet == null)
                  .Select(x =>
                      new Data.DataDuplication.Position.Position
                      {
                          SyncID = x.newSet.newSet.ID,
                          SyncDate = DateTime.Now,
                          Code = x.newSet.newSet.Code,
                          Title = x.newSet.newSet.Title,
                          PositionLevelID = x.newSet.newSet.PositionLevelID
                      }).ToList();
            }

            List<Data.DataDuplication.Position.Position> GetPositionToUpdate(
                List<Data.DataDuplication.Position.Position> left, 
                List<Data.DataDuplication.Position.Position> right)
            {
                return left.Join(
                           right,
                            x => new { x.SyncID },
                            y => new { SyncID = y.ID },
                           (x, y) => new { oldSet = x, newSet = y })
                           .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                            || !x.oldSet.Title.Equals(x.newSet.Title)
                            || x.oldSet.PositionLevelID != x.newSet.PositionLevelID
                            )
                           .Select(y => new Data.DataDuplication.Position.Position
                           {
                               ID = y.oldSet.ID,
                               SyncID = y.oldSet.SyncID,
                               SyncDate = DateTime.Now,
                               Code = y.newSet.Code,
                               Title = y.newSet.Title,
                               PositionLevelID = y.newSet.PositionLevelID
                           }).ToList();
            }

            var PositionURL =
            string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Position").GetSection("GetAll").Value, "?",
                "userid=", credentials.UserID);
            var (PositionResult, PositionIsSuccess, PositionErrorMessage) = await SharedUtilities.GetFromAPI(
                new List<EMS.Manpower.Data.DataDuplication.Position.Position>(), PositionURL);

            StringBuilder successMessage = new StringBuilder();

            if (PositionIsSuccess)
            {
                List<Data.DataDuplication.Position.Position> localCopy = (await _dbAccessPosition.GetAll()).ToList();
                List<Data.DataDuplication.Position.Position> ValueToDelete = GetPositionToDelete(localCopy, PositionResult).ToList();
                List<Data.DataDuplication.Position.Position> ValueToAdd = GetPositionToAdd(localCopy, PositionResult).ToList();
                List<Data.DataDuplication.Position.Position> ValueToUpdate = GetPositionToUpdate(localCopy, PositionResult).ToList();
                if (ValueToDelete.Count > 0 || ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
                    await _dbAccessPosition.Sync(ValueToDelete, ValueToAdd, ValueToUpdate);

                if (ValueToDelete.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToDelete.Count
                        , " deleted Position record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added Position record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated Position record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));
            }

            return new OkObjectResult(successMessage.Length > 0 ? successMessage.ToString() : 
                string.Concat("Position ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
        }

    }
}