using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.Region;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.Region
{
    public interface IRegionDBAccess
    {
        Task<IEnumerable<TableVarRegion>> GetList(GetListInput input, int rowStart);

        Task<Region> GetByID(int ID);

        Task<IEnumerable<Region>> GetByCode(string Code);

        Task<bool> Post(Region param);

        Task<bool> Put(Region param);

        Task<bool> Delete(int ID);

        Task<IEnumerable<Region>> GetAutoComplete(string Term, int TopResult);

        Task<IEnumerable<Region>> GetAll();
    }

    public class RegionDBAccess : IRegionDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public RegionDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarRegion>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarRegion
               .FromSqlRaw(@"CALL sp_region_get_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                        )",   input.ID ?? 0
                            , input.Code ?? ""
                            , input.Description ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<Region> GetByID(int ID)
        {
            return await _dbContext.Region.FindAsync(ID);
        }

        public async Task<IEnumerable<Region>> GetByCode(string Code)
        {
            return await _dbContext.Region.AsNoTracking()
                .Where(x => x.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<bool> Post(Region param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Region.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(Region param)
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
                _dbContext.Region.Remove(new Region() { ID = ID });
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<Region>> GetAutoComplete(string Term, int TopResult)
        {
            return await _dbContext.Region
                .FromSqlRaw("CALL sp_region_autocomplete({0},{1})", (Term ?? ""), TopResult)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Region>> GetAll()
        {
            return await _dbContext.Region.AsNoTracking().ToListAsync();
        }
    }
}