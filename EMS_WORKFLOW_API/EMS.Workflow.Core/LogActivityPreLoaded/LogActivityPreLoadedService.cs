using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.LogActivityPreLoaded;
using EMS.Workflow.Transfer.LogActivityPreLoaded;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;


namespace EMS.Workflow.Core.LogActivityPreLoaded
{
    public interface ILogActivityPreLoadedService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        //Task<IActionResult> Post(APICredentials credentials, Form param);

        //Task<IActionResult> Delete(APICredentials credentials, int ID);

        //Task<IActionResult> Put(APICredentials credentials, Form param);
    }

    public class LogActivityPreLoadedService : Core.Shared.Utilities, ILogActivityPreLoadedService
    {
        private readonly EMS.Workflow.Data.LogActivityPreLoaded.ILogActivityPreLoadedDBAccess _dbAccess;

        public LogActivityPreLoadedService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.LogActivityPreLoaded.ILogActivityPreLoadedDBAccess dBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarLogActivityPreLoaded> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Name = x.Name,
                DateCreated = x.DateCreated
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.LogActivityPreLoaded.LogActivityPreLoaded result = await _dbAccess.GetByID(ID);
            IEnumerable<Data.LogActivityPreLoaded.LogActivityPreLoadedItems> _preloadedItemsList = await _dbAccess.GetPreLoadedItems(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Name = result.Name,
                    PreLoadedItemsList = _preloadedItemsList.Select(x => new Transfer.LogActivityPreLoaded.PreLoadedItems
                    {
                        Module = x.Module,
                        Type = x.Type,
                        SubType = x.SubType,
                        Title = x.Title,
                        Description = x.Description
                    }).ToList(),
                    CreatedBy = result.CreatedBy
                });
        }
    }
}
