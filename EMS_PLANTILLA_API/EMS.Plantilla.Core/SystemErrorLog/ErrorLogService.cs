using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.SystemErrorLog;
using EMS.Plantilla.Transfer.SystemErrorLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.Core.SystemErrorLog
{
    public interface IErrorLogService
    {
        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);
    }
    public class ErrorLogService : EMS.Plantilla.Core.Shared.Utilities, IErrorLogService
    {
        private readonly IErrorLogDBAccess _dbAccess;

        public ErrorLogService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IErrorLogDBAccess dbAccess) :base (dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            EMS.Plantilla.Data.SystemErrorLog.ErrorLog result = (await _dbAccess.GetByID(ID));
            
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(new Form { 
                    ID = result.ID,
                    Method = result.Method,
                    Class = result.Class,
                    ErrorMessage = result.ErrorMessage,
                    InnerException = result.InnerException,
                    UserID = result.UserID,
                    CreatedDate = result.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt")
                });
            }
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarErrorLog> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Class = x.Class,
                ErrorMessage = x.ErrorMessage,
                CreatedDate = x.CreatedDate,
                UserID = x.UserID,
                User = x.User,
            }).ToList());
        }

    }
}
