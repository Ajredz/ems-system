using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.KPIScore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Data.KPIScore
{
    public interface IKPIScoreDBAccess
    {
        Task<IEnumerable<TableVarKPIScore>> GetList(GetListInput input, int rowStart);

        Task<bool> Post(List<KPIScore> param);

        Task<bool> Put(List<KPIScore> param);

        Task<IEnumerable<TableVarKPIScoreGetByID>> GetByID(int ID, string KPIType);

        Task<bool> UploadScoresInsert(List<KPIScore> toAdd, List<KPIScore> toUpdate);

        Task<bool> Truncate();

        Task<List<KPIScore>> GetByKPIOrg(int OrgGroupID, int KPIID);

        Task<IEnumerable<KPIScore>> GetAll();

        Task<IEnumerable<KPIScore>> GetByPeriods(List<DateTime> Dates);

        Task<IEnumerable<KPIScorePerEmployee>> GetPerEmployeeByPeriods(List<DateTime> Dates);

        Task<IEnumerable<TableVarKPIScore>> GetPerEmployeeList(GetListInput input, int rowStart);

        Task<bool> UploadScoresPerEmployeeInsert(List<KPIScorePerEmployee> toAdd, List<KPIScorePerEmployee> toUpdate);
    }

    public class KPIScoreDBAccess : IKPIScoreDBAccess
    {
        private readonly IPMContext _dbContext;

        public KPIScoreDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarKPIScore>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarKPIScore
              .FromSqlRaw(@"CALL sp_kpi_score_get_list(
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
                            , {11}
                            , {12}
                            , {13}
                        )", input.ID ?? ""
                           , input.ParentOrgGroup ?? ""
                           , input.OrgGroupDelimited ?? ""
                           , input.KPIDelimited ?? ""
                           , input.Target ?? 0
                           , input.Actual ?? 0
                           , input.Rate ?? 0
                           , input.PeriodFrom ?? ""
                           , input.PeriodTo ?? ""
                           , input.IsExport
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                       )
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<IEnumerable<TableVarKPIScore>> GetPerEmployeeList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarKPIScore
              .FromSqlRaw(@"CALL sp_kpi_score_per_employee_get_list(
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
                            , {11}
                            , {12}
                        )", input.ID ?? ""
                           , input.EmployeeDelimited ?? ""
                           , input.KPIDelimited ?? ""
                           , input.Target ?? 0
                           , input.Actual ?? 0
                           , input.Rate ?? 0
                           , input.PeriodFrom ?? ""
                           , input.PeriodTo ?? ""
                           , input.IsExport
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                       )
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<bool> Post(List<KPIScore> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KPIScore.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(List<KPIScore> scores)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                if (scores != null)
                {
                    scores
                    .Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarKPIScoreGetByID>> GetByID(int ID, string KPIType)
        {
            return await _dbContext.TableVarKPIScoreGetByID
                .FromSqlRaw("CALL sp_kpi_score_get_by_id({0},{1})", ID, KPIType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UploadScoresInsert(List<KPIScore> toAdd, List<KPIScore> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (toUpdate.Count > 0)
                {
                    toUpdate.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                await _dbContext.KPIScore.AddRangeAsync(toAdd);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> UploadScoresPerEmployeeInsert(List<KPIScorePerEmployee> toAdd, List<KPIScorePerEmployee> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (toUpdate.Count > 0)
                {
                    toUpdate.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                await _dbContext.KPIScorePerEmployee.AddRangeAsync(toAdd);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Truncate()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE kpi_score");
            return true;
        }

        public async Task<List<KPIScore>> GetByKPIOrg(int OrgGroupID, int KPIID)
        {
            return await _dbContext.KPIScore.AsNoTracking()
                                            .Where(x => x.OrgGroup == OrgGroupID)
                                            .Where(x => x.KPI == KPIID)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<KPIScore>> GetAll()
        {
            return await _dbContext.KPIScore.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<KPIScore>> GetByPeriods(List<DateTime> Dates)
        {
            return await _dbContext.KPIScore.AsNoTracking().Where(x => Dates.Contains(x.Period)).ToListAsync();
        }
        
        public async Task<IEnumerable<KPIScorePerEmployee>> GetPerEmployeeByPeriods(List<DateTime> Dates)
        {
            return await _dbContext.KPIScorePerEmployee.AsNoTracking().Where(x => Dates.Contains(x.Period)).ToListAsync();
        }
    }
}