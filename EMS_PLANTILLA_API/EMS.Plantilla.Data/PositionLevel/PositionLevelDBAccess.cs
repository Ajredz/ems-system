using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.PositionLevel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.PositionLevel
{
    public interface IPositionLevelDBAccess
    {
        Task<IEnumerable<TableVarPositionLevel>> GetList(GetListInput input, int rowStart);

        Task<PositionLevel> GetByID(int ID);

        Task<IEnumerable<PositionLevel>> GetByDescription(string Description);

        Task<bool> Post(PositionLevel param);

        Task<bool> Put(PositionLevel param);

        Task<bool> Delete(int ID);

        Task<IEnumerable<PositionLevel>> GetAutoComplete(string Term, int TopResult);

        Task<IEnumerable<PositionLevel>> GetAll();

        Task<IEnumerable<PositionLevel>> GetByOrgGroupID(int OrgGroupID);

        Task<IEnumerable<PositionLevel>> GetLastModified(DateTime? From, DateTime? To);
    }

    public class PositionLevelDBAccess : IPositionLevelDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public PositionLevelDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarPositionLevel>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarPositionLevel
                .FromSqlRaw(@"CALL sp_position_level_get_list(
                                 {0}
                               , {1}
                               , {2}
                               , {3}
                               , {4}
                               , {5}
                               , {6}
                            )", input.ID ?? 0
                               , input.Description ?? ""
                               , input.IsExport
                               , input.sidx ?? ""
                               , input.sord ?? ""
                               , rowStart
                               , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PositionLevel> GetByID(int ID)
        {
            return await _dbContext.PositionLevel.FindAsync(ID);
        }

        public async Task<IEnumerable<PositionLevel>> GetByDescription(string Description)
        {
            return await _dbContext.PositionLevel.AsNoTracking()
                .Where(x => x.Description.Equals(Description, StringComparison.CurrentCultureIgnoreCase) & x.IsActive)
                .ToListAsync();
        }

        public async Task<bool> Post(PositionLevel param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.PositionLevel.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(PositionLevel param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Delete(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.PositionLevel.Remove(new PositionLevel() { ID = ID });
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<PositionLevel>> GetAutoComplete(string Term, int TopResult)
        {
            return await _dbContext.PositionLevel
                .FromSqlRaw("CALL sp_position_level_autocomplete({0},{1})", Term ?? "", TopResult)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<PositionLevel>> GetAll()
        {
            return await _dbContext.PositionLevel.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<PositionLevel>> GetByOrgGroupID(int OrgGroupID)
        {
            return await _dbContext.PositionLevel
                .FromSqlRaw("CALL sp_position_level_get_by_org_group({0})", OrgGroupID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<PositionLevel>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.PositionLevel.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }
    }
}