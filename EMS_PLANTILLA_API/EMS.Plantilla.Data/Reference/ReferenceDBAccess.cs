using EMS.Plantilla.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.Reference
{
    public interface IReferenceDBAccess
    {
        Task<IEnumerable<Reference>> GetByCodes(List<string> Codes);

        Task<IEnumerable<ReferenceValue>> GetByRefCodes(List<string> RefCodes);

        Task<bool> Put(ReferenceValue param);

        Task<IEnumerable<ReferenceValue>> GetAutoComplete(string RefCode, string Term, int TopResult);

        Task<bool> UpdateSet(List<ReferenceValue> toDelete,
            List<ReferenceValue> toAdd,
            List<ReferenceValue> toUpdate);

        Task<IEnumerable<Reference>> GetMaintainedReference();

        Task<ReferenceValue> GetRefCodeAndValue(string RefCode, string Value);

        Task<IEnumerable<Reference>> GetByIsMaintainable(bool IsMaintainable);

        Task<IEnumerable<ReferenceValue>> GetByRefCodeValue(string Code, string Value);

        Task<IEnumerable<ReferenceValue>> GetByRefCode(string RefCode);

        Task<bool> Post(List<ReferenceValue> param);

        Task<IEnumerable<ReferenceValue>> GetReferenceValueList(string RefCode);

        Task<bool> AddReferenceValue(ReferenceValue param);

        Task<ReferenceValue> GetReferenceValueByID(int ID);

        Task<ReferenceValue> GetValueByRefCodeValue(string RefCode, string Value);

    }

    public class ReferenceDBAccess : IReferenceDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public ReferenceDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Reference>> GetByCodes(List<string> Codes)
        {
            return await _dbContext.Reference.AsNoTracking()
                .Where(x => Codes.Contains(x.Code))
                .Distinct().ToListAsync();
        }

        public async Task<IEnumerable<ReferenceValue>> GetByRefCodes(List<string> RefCodes)
        {
            return await _dbContext.ReferenceValue.AsNoTracking()
                .Where(x => RefCodes.Contains(x.RefCode))
                .Distinct().ToListAsync();
        }

        public async Task<bool> Put(ReferenceValue param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<ReferenceValue>> GetAutoComplete(string RefCode, string Term, int TopResult)
        {
            return await _dbContext.ReferenceValue
                .FromSqlRaw("CALL sp_reference_value_autocomplete({0},{1},{2})",
                RefCode ?? "",
                Term ?? "",
                TopResult)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> UpdateSet(List<ReferenceValue> toDelete,
            List<ReferenceValue> toAdd,
            List<ReferenceValue> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.ReferenceValue.RemoveRange(toDelete);
                _dbContext.ReferenceValue.AddRange(toAdd);
                toUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<Reference>> GetMaintainedReference()
        {
            return await _dbContext.Reference.AsNoTracking().Where(x => x.IsMaintainable).ToListAsync();
        }

        public async Task<ReferenceValue> GetRefCodeAndValue(string RefCode, string Value)
        {
            return await _dbContext.ReferenceValue.AsNoTracking()
                .Where(x => x.RefCode.Equals(RefCode) & x.Value.Equals(Value)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Reference>> GetByIsMaintainable(bool IsMaintainable)
        {
            return await _dbContext.Reference.AsNoTracking().Where(x => x.IsMaintainable == IsMaintainable).ToListAsync();
        }

        public async Task<IEnumerable<ReferenceValue>> GetByRefCode(string RefCode)
        {
            return await _dbContext.ReferenceValue.AsNoTracking()
                .Where(x => x.RefCode.Equals(RefCode))
                .ToListAsync();
        }

        public async Task<IEnumerable<ReferenceValue>> GetByRefCodeValue(string Code, string Value)
        {
            return await _dbContext.ReferenceValue.AsNoTracking().Where(x => x.RefCode.Equals(Code, StringComparison.CurrentCultureIgnoreCase) && x.Value.Equals(Value, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
        }


        public async Task<bool> Post(List<ReferenceValue> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ReferenceValue.AddRange(param);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<ReferenceValue>> GetReferenceValueList(string RefCode)
        {
            return await _dbContext.ReferenceValue.AsNoTracking().Where(x => x.RefCode.Equals(RefCode, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
        }

        public async Task<bool> AddReferenceValue(ReferenceValue param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ReferenceValue.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<ReferenceValue> GetReferenceValueByID(int ID)
        {
            return await _dbContext.ReferenceValue.FindAsync(ID);
        }

        public async Task<ReferenceValue> GetValueByRefCodeValue(string RefCode, string Value)
        {
            return await _dbContext.ReferenceValue.Where(x => x.RefCode.Equals(RefCode) & x.Value.Equals(Value)).FirstOrDefaultAsync();
        }
    }
}