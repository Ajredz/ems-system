using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.Shared;
using EMS.IPM.Transfer.KRASubGroup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.KRASubGroup
{
    public interface IKRASubGroupDBAccess
    {
        Task<bool> Post(KRASubGroup param);

        Task<IEnumerable<KRASubGroup>> GetAll();

        Task<IEnumerable<KRASubGroup>> GetKRASubGroup(string Name);

        Task<IEnumerable<TableVariableAutoComplete>> GetAutoComplete(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param);
    }

    public class KRASubGroupDBAccess : IKRASubGroupDBAccess
    {
        private readonly IPMContext _dbContext;

        public KRASubGroupDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Post(KRASubGroup param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KRASubGroup.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<KRASubGroup>> GetAll()
        {
            return await _dbContext.KRASubGroup.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<KRASubGroup>> GetKRASubGroup(string Name)
        {
            return await _dbContext.KRASubGroup.AsNoTracking()
                .Where(x => x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVariableAutoComplete>> GetAutoComplete(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _dbContext.TableVariableAutoComplete
                .FromSqlRaw("CALL sp_kra_sub_group_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}