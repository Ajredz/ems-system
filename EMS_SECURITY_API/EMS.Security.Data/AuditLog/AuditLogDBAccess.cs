using EMS.Security.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using EMS.Security.Transfer.AuditLog;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Security.Data.AuditLog
{
    public interface IAuditLogDBAccess
    {
        Task<IEnumerable<TableVarAuditLogs>> GetList(GetListInput input, int rowStart);

        Task<bool> Add(AuditLog param);

        Task<IEnumerable<TableVarEventType>> GetEventTypeAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<TableVarTableName>> GetTableNameAutoComplete(GetAutoCompleteInput param);
    }

    public class AuditLogDBAccess : IAuditLogDBAccess
    {
        private readonly SystemAccessContext _dbContext;

        public AuditLogDBAccess(SystemAccessContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarAuditLogs>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarAuditLogs
                .FromSqlRaw(@"CALL sp_audit_logs_get_list(
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
                            )", input.ID ?? 0
                               , input.EventTypeDelimited ?? ""
                               , input.TableNameDelimited ?? ""
                               , input.Remarks ?? ""
                               , input.Name ?? ""
                               , input.IPAddress ?? ""
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

        public async Task<bool> Add(AuditLog param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.AuditLog.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarEventType>> GetEventTypeAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.TableVarEventType
                .FromSqlRaw("CALL sp_event_type_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarTableName>> GetTableNameAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.TableVarTableName
                .FromSqlRaw("CALL sp_table_name_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}