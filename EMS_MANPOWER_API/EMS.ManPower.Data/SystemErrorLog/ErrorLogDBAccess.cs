using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.SystemErrorLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.SystemErrorLog
{
    public interface IErrorLogDBAccess
    {
        Task<ErrorLog> GetByID(int ID);

        Task<IEnumerable<TableVarErrorLog>> GetList(GetListInput input, int rowStart);
    }

    public class ErrorLogDBAccess : IErrorLogDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public ErrorLogDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorLog> GetByID(int ID)
        {
            return await _dbContext.ErrorLog.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TableVarErrorLog>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarErrorLog
                .FromSqlRaw(@"CALL sp_error_log_get_list(
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
                            )", input.ID ?? 0
                               , input.Method ?? ""
                               , input.Class ?? ""
                               , input.ErrorMessage ?? ""
                               , input.UserIDDelimited ?? ""
                               , input.DateCreatedFrom ?? ""
                               , input.DateCreatedTo ?? ""
                               , input.IsExport
                               , input.sidx ?? ""
                               , input.sord ?? ""
                               , rowStart
                               , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}