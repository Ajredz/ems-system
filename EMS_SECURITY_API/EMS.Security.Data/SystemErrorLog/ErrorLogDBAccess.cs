using EMS.Security.Data.DBContexts;
using EMS.Security.Data.SystemRole;
using EMS.Security.Transfer.SystemErrorLog;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Security.Data.SystemErrorLog
{
    public interface IErrorLogDBAccess
    {
        Task<ErrorLog> GetByID(int ID);

        Task<IEnumerable<TableVarErrorLog>> GetList(GetListInput input, int rowStart);
    }

    public class ErrorLogDBAccess : IErrorLogDBAccess
    {
        private readonly SystemAccessContext _dbContext;

        public ErrorLogDBAccess(SystemAccessContext dbContext)
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