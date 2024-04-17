using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer.KPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.KPI
{
    public interface IKPIDBAccess
    {
        Task<IEnumerable<TableVarKPI>> GetList(GetListInput input, int rowStart);

        Task<bool> Post(KPI param);

        Task<bool> Put(KPI param);

        Task<IEnumerable<TableVarNewKPICode>> GetNewKPICode(int ctr);

        Task<KPI> GetByID(int ID);

        Task<IEnumerable<KPI>> GetByCode(string Code);

        Task<IEnumerable<KPI>> GetByOldKPICode(string Code);

        Task<IEnumerable<KPI>> GetByCodes(List<string> Codes);

        Task<IEnumerable<KPI>> GetOldKPICodes(List<string> Code);

        Task<IEnumerable<ReferenceValue>> GetRefCodes(List<string> Code);

        Task<IEnumerable<KPI>> GetAll();

        Task<IEnumerable<KPIPosition.KPIPosition>> GetKPIIfUsed(int ID);

        Task<IEnumerable<KPI>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<TableVarKPIDetails>> GetAllDetails();

        Task<bool> Upload(List<KPI> param);
    }

    public class KPIDBAccess : IKPIDBAccess
    {
        private readonly IPMContext _dbContext;

        public KPIDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarKPI>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarKPI
                   .FromSqlRaw(@"CALL sp_kpi_get_list(
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
                        )", input.ID ?? 0
                          , input.Code ?? ""
                          , input.KRATypeDelimited ?? ""
                          , input.KRAGroup ?? ""
                          , input.KRASubGroup ?? ""
                          , input.Name ?? ""
                          , input.OldKPICode ?? ""
                          , input.KPITypeDelimited ?? ""
                          , input.SourceTypeDelimited ?? ""
                          , input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                        )
                   .AsNoTracking()
                   .ToListAsync();
        }

        public async Task<bool> Post(KPI param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KPI.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(KPI param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarNewKPICode>> GetNewKPICode(int ctr)
        {
            return await _dbContext.TableVarNewKPICode
                .FromSqlRaw("CALL sp_kpi_get_new_code({0})", ctr)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<KPI> GetByID(int ID)
        {
            return await _dbContext.KPI.FindAsync(ID);
        }

        public async Task<IEnumerable<KPI>> GetByCode(string Code)
        {
            return await _dbContext.KPI.AsNoTracking()
                .Where(x => x.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<KPI>> GetByOldKPICode(string Code)
        {
            return await _dbContext.KPI.AsNoTracking()
                .Where(x => x.OldKPICode.Equals(Code, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<KPI>> GetAll()
        {
            return await _dbContext.KPI.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<KPIPosition.KPIPosition>> GetKPIIfUsed(int ID)
        {
            return await _dbContext.KPIPosition.AsNoTracking()
                .Where(x => x.KPI == ID)
                .ToListAsync();
        }

        public async Task<IEnumerable<KPI>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.KPI
                .FromSqlRaw("CALL sp_kpi_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarKPIDetails>> GetAllDetails()
        {
            return await _dbContext.TableVarKPIDetails
                .FromSqlRaw("CALL sp_kpi_details()")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<KPI>> GetByCodes(List<string> Codes)
        {
            return await _dbContext.KPI.AsNoTracking()
                .Where(x => Codes.Contains(x.Code))
                .Distinct().ToListAsync();
        }

        public async Task<IEnumerable<KPI>> GetOldKPICodes(List<string> Codes)
        {
            return await _dbContext.KPI.AsNoTracking()
                .Where(x => Codes.Contains(x.OldKPICode))
                .Distinct().ToListAsync();
        }

        public async Task<IEnumerable<ReferenceValue>> GetRefCodes(List<string> RefCodes)
        {
            return await _dbContext.ReferenceValue.AsNoTracking()
                .Where(x => RefCodes.Contains(x.RefCode))
                .Distinct().ToListAsync();
        }

        public async Task<bool> Upload(List<KPI> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KPI.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }

            return true;
        }
    }
}