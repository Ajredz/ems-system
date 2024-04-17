using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.MyKPIScores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Data.MyKPIScores
{
    public interface IMyKPIScoresDBAccess
    {
        Task<IEnumerable<TableVarMyKPIScores>> GetList(APICredentials credentials, GetListInput input, int rowStart);

        Task<IEnumerable<TableVarMyKPIScoresGetByID>> GetByID(GetByIDInput input, int UserID);
    }

    public class MyKPIScoresDBAccess : IMyKPIScoresDBAccess
    {
        private readonly IPMContext _dbContext;

        public MyKPIScoresDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarMyKPIScores>> GetList(APICredentials credentials, GetListInput input, int rowStart)
        {
			return await _dbContext.TableVarMyKPIScores
			  .FromSqlRaw(@"CALL sp_my_kpi_scores_get_list(
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
						, {13}
						, {14}
						, {15}
						, {16}
					)", input.ID ?? 0
						   , input.TransSummaryIDDelimited ?? ""
						   , input.Description ?? ""
						   , input.OrgGroupDelimited ?? ""
						   , input.PositionDelimited ?? ""
						   , input.DateFromFrom ?? ""
						   , input.DateFromTo ?? ""
						   , input.DateToFrom ?? ""
						   , input.DateToTo ?? ""
						   , input.ScoreFrom ?? -1
						   , input.ScoreTo ?? -1
                           , input.IsExport
						   , credentials.UserID
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }

        public async Task<IEnumerable<TableVarMyKPIScoresGetByID>> GetByID(GetByIDInput input, int UserID)
        {
            return await _dbContext.TableVarMyKPIScoresGetByID
                .FromSqlRaw("CALL sp_employee_score_get_by_id({0},{1},{2})", input.ID, input.RoleIDs, UserID)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}