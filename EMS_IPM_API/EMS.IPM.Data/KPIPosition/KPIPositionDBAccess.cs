using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.Shared;
using EMS.IPM.Transfer.KPIPosition;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Data.KPIPosition
{
    public interface IKPIPositionDBAccess
    {
        Task<IEnumerable<TableVarKPIPosition>> GetList(GetListInput input, int rowStart);

        Task<bool> Post(List<KPIPosition> param);

        Task<bool> Put(List<KPIPosition> toAdd, List<KPIPosition> toUpdate, List<KPIPosition> toDelete);

        Task<bool> Delete(List<KPIPosition> param);

        Task<IEnumerable<KPIPosition>> GetByPositionID(int PositionID);

        Task<IEnumerable<KPIPosition>> GetByPositionID(int PositionID, DateTime EffectiveDate);

        Task<IEnumerable<TableVarKPIPositionDetails>> GetDetailsByPositionID(int ID, string EffectiveDate);

        Task<IEnumerable<KPIPosition>> GetAll();

        Task<IEnumerable<TableVarKPIPositionDetails>> GetAllDetails();

        Task<bool> UploadInsert(List<KPIPosition> param);

        Task<IEnumerable<TableVarKPIPositionExport>> GetExportList(GetListInput input, int rowStart);
        Task<IEnumerable<TableVariableAutoComplete>> GetCopyPosition(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param);
    }

    public class KPIPositionDBAccess : IKPIPositionDBAccess
    {
        private readonly IPMContext _dbContext;

        public KPIPositionDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarKPIPosition>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarKPIPosition
              .FromSqlRaw(@"CALL sp_kpi_position_get_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                            , {8}
                            , {9}
                            , {10}
                        )", input.ID ?? 0
                           , input.PositionDelimited ?? ""
                           , input.Weight ?? 0
                           , input.DateEffectiveFrom ?? ""
                           , input.DateEffectiveTo ?? ""
                           , input.IsShowRecentOnly
                           , input.IsExport
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                       )
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<bool> Post(List<KPIPosition> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KPIPosition.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> Put(List<KPIPosition> toAdd, List<KPIPosition> toUpdate, List<KPIPosition> toDelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                if (toAdd != null)
                {
                    await _dbContext.KPIPosition.AddRangeAsync(toAdd);
                }

                if (toUpdate != null)
                {
                    toUpdate
                    .Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                if (toDelete != null)
                {
                    _dbContext.KPIPosition.RemoveRange(toDelete);
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Delete(List<KPIPosition> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.KPIPosition.RemoveRange(param);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarKPIPositionDetails>> GetDetailsByPositionID(int ID, string EffectiveDate)
        {
            return await _dbContext.TableVarKPIPositionDetails
              .FromSqlRaw(@"CALL sp_kpi_position_get_by_position({0}, {1})", ID, EffectiveDate)
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<IEnumerable<KPIPosition>> GetByPositionID(int PositionID)
        {
            return await _dbContext.KPIPosition.AsNoTracking()
                                               .Where(x => x.Position == PositionID)
                                               .ToListAsync();
        }

        public async Task<IEnumerable<KPIPosition>> GetByPositionID(int PositionID, DateTime EffectiveDate)
        {
            return await _dbContext.KPIPosition.AsNoTracking()
                                               .Where(x => x.Position == PositionID)
                                               .Where(x => x.TDate == EffectiveDate)
                                               .ToListAsync();
        }

        public async Task<IEnumerable<KPIPosition>> GetByID(int ID, string EffectiveDate)
        {
            return await _dbContext.KPIPosition.AsNoTracking()
                                               .Where(x => x.ID == ID)
                                               .Where(x => x.TDate == Convert.ToDateTime(EffectiveDate))
                                               .ToListAsync();
        }

        public async Task<IEnumerable<KPIPosition>> GetAll()
        {
            return await _dbContext.KPIPosition.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVarKPIPositionDetails>> GetAllDetails()
        {
            return await _dbContext.TableVarKPIPositionDetails
                .FromSqlRaw("CALL sp_kpi_position_details()")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UploadInsert(List<KPIPosition> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KPIPosition.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<TableVarKPIPositionExport>> GetExportList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarKPIPositionExport
              .FromSqlRaw(@"CALL sp_kpi_position_get_export_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                            , {8}
                            , {9}
                        )", input.ID ?? 0
                           , input.PositionDelimited ?? ""
                           , input.DateEffectiveFrom ?? ""
                           , input.DateEffectiveTo ?? ""
                           , input.IsShowRecentOnly
                           , input.IsExport
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                       )
              .AsNoTracking()
              .ToListAsync();
        }
        public async Task<IEnumerable<TableVariableAutoComplete>> GetCopyPosition(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _dbContext.TableVariableAutoComplete
                .FromSqlRaw("CALL sp_copy_kpi_position_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}