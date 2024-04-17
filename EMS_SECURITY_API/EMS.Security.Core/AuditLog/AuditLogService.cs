using EMS.Security.Data.DBContexts;
using EMS.Security.Data.AuditLog;
using EMS.Security.Transfer.AuditLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Security.Core.AuditLog
{
    public interface IAuditLogService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> Add(APICredentials credentials, Form param);

        Task<IActionResult> GetEventTypeByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetTableNameByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);
    }
    public class AuditLogService : EMS.Security.Core.Shared.Utilities, IAuditLogService
    {
        private readonly IAuditLogDBAccess _dbAccess;

        public AuditLogService(SystemAccessContext dbContext, IConfiguration iconfiguration,
            IAuditLogDBAccess dbAccess) :base (dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarAuditLogs> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Type = x.Type,
                TableName = x.TableName,
                Remarks = x.Remarks,
                Name = x.Name,
                IPAddress = x.IPAddress,
                DateCreated = x.DateCreated
            }).ToList());
        }

        public async Task<IActionResult> Add(APICredentials credentials, Form param)
        {
            if(await _dbAccess.Add(new Data.AuditLog.AuditLog { 
                EventType = param.EventType,
                TableName = param.TableName,
                TableID = param.TableID,
                Remarks = param.Remarks,
                IsSuccess = param.IsSuccess,
                IPAddress = param.IPAddress,
                CreatedBy = param.CreatedBy
            }))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new OkObjectResult(MessageUtilities.ERRMSG_REC_SAVE);
        }

        public async Task<IActionResult> GetEventTypeByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEventTypeAutoComplete(param))
                .Select(x => new GetEventTypeByAutoCompleteOutput
                {
                    Description = x.EventType
                })
            );
        }

        public async Task<IActionResult> GetTableNameByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetTableNameAutoComplete(param))
                .Select(x => new GetTableNameByAutoCompleteOutput
                {
                    Description = x.TableName
                })
            );
        }
    }
}
