using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.Shared;
using EMS.IPM.Transfer.KRAGroup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.KRAGroup
{
    public interface IKRAGroupDBAccess
    {
        Task<bool> Post(KRAGroup param);

        Task<IEnumerable<KRAGroup>> GetAll();

        Task<IEnumerable<KRAGroup>> GetKRAGroup(string Name);

        Task<IEnumerable<TableVariableAutoComplete>> GetAutoComplete(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param);
    }

    public class KRAGroupDBAccess : IKRAGroupDBAccess
    {
        private readonly IPMContext _dbContext;

        public KRAGroupDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Post(KRAGroup param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KRAGroup.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<KRAGroup>> GetAll()
        {
            return await _dbContext.KRAGroup.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<KRAGroup>> GetKRAGroup(string Name)
        {
            return await _dbContext.KRAGroup.AsNoTracking()
                .Where(x => x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVariableAutoComplete>> GetAutoComplete(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _dbContext.TableVariableAutoComplete
                .FromSqlRaw("CALL sp_kra_group_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}