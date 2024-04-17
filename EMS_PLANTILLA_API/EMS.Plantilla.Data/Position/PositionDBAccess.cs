using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.Position;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.Position
{
    public interface IPositionDBAccess
    {
        Task<IEnumerable<TableVarPosition>> GetList(GetListInput input, int rowStart);

        Task<Position> GetByID(int ID);

        Task<IEnumerable<Position>> GetByCode(string Code);

        Task<IEnumerable<Position>> GetByCodes(List<string> Codes);

        Task<bool> Post(Position param);

        Task<bool> Put(Position param);

        Task<bool> Delete(int ID);

        Task<IEnumerable<Position>> GetAll();

        Task<IEnumerable<Position>> GetByPositionLevel(int PositionLevelID);

        Task<IEnumerable<Position>> GetLastModified(DateTime? From, DateTime? To);

        Task<IEnumerable<Position>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<Position>> GetByIDs(List<int> IDs);

        Task<IEnumerable<TableVarPositionWithLevelAutoComplete>> GetPositionWithLevelByAutoComplete(GetAutoCompleteInput input);

    }

    public class PositionDBAccess : IPositionDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public PositionDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarPosition>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarPosition
               .FromSqlRaw(@"CALL sp_position_get_list(
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
                            , input.PositionLevelIDs ?? ""
                            , input.Code ?? ""
                            , input.Title ?? ""
                            , input.ParentPositionDelimited ?? ""
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<Position> GetByID(int ID)
        {
            return await _dbContext.Position.FindAsync(ID);
        }

        public async Task<IEnumerable<Position>> GetByCode(string Code)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => x.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase) & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByCodes(List<string> Codes)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => Codes.Contains(x.Code)).Where(x => x.IsActive)
                .Distinct().ToListAsync();
        }

        public async Task<bool> Post(Position param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Position.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(Position param)
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
                _dbContext.Position.Remove(new Position() { ID = ID });
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<Position>> GetAll()
        {
            return await _dbContext.Position.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByPositionLevel(int PositionLevelID)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => x.PositionLevelID == PositionLevelID & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.Position.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Position
                .FromSqlRaw("CALL sp_position_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByIDs(List<int> IDs)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => IDs.Contains(x.ID)).Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<TableVarPositionWithLevelAutoComplete>> GetPositionWithLevelByAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.TableVarPositionWithLevelAutoComplete
                .FromSqlRaw("CALL sp_position_with_level_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}