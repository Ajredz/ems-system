using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.MyKPIScores;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer;
using EMS.IPM.Transfer.MyKPIScores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.MyKPIScores
{
    public interface IMyKPIScoresService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);
    }

    public class MyKPIScoresService : Core.Shared.Utilities, IMyKPIScoresService
    {
        private readonly IMyKPIScoresDBAccess _dbAccess;

        public MyKPIScoresService(IPMContext dbContext, IConfiguration iconfiguration,
            IMyKPIScoresDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarMyKPIScores> result = await _dbAccess.GetList(credentials, input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                TransSummaryID = x.TransSummaryID,
                Description = x.Description,
                ID = x.ID,
				OrgGroup = x.OrgGroup,
                Position = x.Position,
                Score = Math.Round(x.Score, 2),
                TDateFrom = x.TDateFrom,
                TDateTo = x.TDateTo,
                PDateFrom = x.PDateFrom,
                PDateTo = x.PDateTo,
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            IEnumerable<TableVarMyKPIScoresGetByID> result = await _dbAccess.GetByID(input, credentials.UserID);
            List<MyKPIScoreForm> MyKPIScoreForm = new List<MyKPIScoreForm>();

            //List<Position> positionList = (await _positionDBAccess.GetAll()).ToList();
            //var a = positionList.Where(x => x.ParentPositionID == )

            foreach (var e in result)
            {
                MyKPIScoreForm.Add(new MyKPIScoreForm
                {
                    ID = e.ID,
                    KRAGroup = e.KRAGroup,
                    KPICode = e.KPICode,
                    KPIName = e.KPIName,
                    KPIDescription = e.KPIDescription,
                    KPIGuidelines = e.KPIGuidelines,
                    Weight = Math.Round(e.Weight, 2),
                    Target = Math.Round(e.Target ?? 0, 2),
                    Actual = Math.Round(e.Actual ?? 0, 2),
                    Rate = Math.Round(e.Rate ?? 0, 2),
                    IsEditable = e.IsEditable,
                    Requestor = e.Requestor,
                    Grade = e.Grade,
                    SourceType =e.SourceType
                });
            }

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    TID = input.ID,
                    TransSummaryID = result.First().TransSummaryID,
                    Employee = result.First().Employee,
                    EmployeeCode = result.First().EmployeeCode,
                    Position = result.First().Position,
                    OrgGroup = result.First().OrgGroup,
                    TDateFrom = result.First().TDateFrom,
                    TDateTo = result.First().TDateTo,
                    Status = result.First().Status,
                    Requestor = result.First().Requestor,
                    MyKPIScoreList = MyKPIScoreForm.OrderBy(x => x.ID).ToList()
                });
        }
    }
}