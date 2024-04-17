using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.EmployeeScoreDashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScoreDashboard
{
    public interface IEmployeeScoreDashboardDBAccess
    {
        Task<IEnumerable<TableVarDashboardSummaryForEvaluation>> GetListSummaryForEvaluation(GetListSummaryForEvaluationInput input, int rowStart);
        
        Task<IEnumerable<TableVarDashboardSummaryForApproval>> GetListSummaryForApproval(GetListSummaryForApprovalInput input, int rowStart);

        Task<IEnumerable<TableVarDashboardRegionalWithPosition>> GetListRegionalWithPosition(GetListRegionalWithPositionInput input, int rowStart);

        Task<IEnumerable<TableVarDashboardBranchesWithPosition>> GetListBranchesWithPosition(GetListBranchesWithPositionInput input, int rowStart);

        Task<IEnumerable<TableVarDashboardPositionOnly>> GetListPositionOnly(GetListPositionOnlyInput input, int rowStart);

        Task<IEnumerable<TableVarDashboardSummaryForApprovalBRN>> GetListSummaryForApprovalBRN(GetListSummaryForApprovalBRNInput input, int rowStart);

        Task<IEnumerable<TableVarDashboardSummaryForApprovalCLU>> GetListSummaryForApprovalCLU(GetListSummaryForApprovalCLUInput input, int rowStart);
    }

    public class EmployeeScoreDashboardDBAccess : IEmployeeScoreDashboardDBAccess
    {
        private readonly IPMContext _dbContext;

        public EmployeeScoreDashboardDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarDashboardSummaryForEvaluation>> GetListSummaryForEvaluation(GetListSummaryForEvaluationInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardSummaryForEvaluation
              .FromSqlRaw(@"CALL sp_dashboard_summary_for_evaluation(
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
					)", input.RunIDDelimited ?? ""
						   , input.RegionDelimited ?? ""
						   , input.WithCompleteScoreMin ?? -1
						   , input.WithCompleteScoreMax ?? -1
                           , input.WithMissingScoreMin ?? -1
						   , input.WithMissingScoreMax ?? -1
                           , input.NoScoreMin ?? -1
						   , input.NoScoreMax ?? -1
                           , input.OnGoingEvalMin ?? -1
						   , input.OnGoingEvalMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVarDashboardSummaryForApproval>> GetListSummaryForApproval(GetListSummaryForApprovalInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardSummaryForApproval
              .FromSqlRaw(@"CALL sp_dashboard_summary_for_approval(
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
					)", input.RunIDDelimited ?? ""
						   , input.RegionDelimited ?? ""
                           , input.NoKeyInMin ?? -1
						   , input.NoKeyInMax ?? -1
						   , input.ForApprovalMin ?? -1
						   , input.ForApprovalMax ?? -1
                           , input.FinalizedMin ?? -1
						   , input.FinalizedMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVarDashboardSummaryForApprovalBRN>> GetListSummaryForApprovalBRN(GetListSummaryForApprovalBRNInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardSummaryForApprovalBRN
              .FromSqlRaw(@"CALL sp_dashboard_summary_for_approval_brn(
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
					)", input.RunIDDelimited ?? ""
						   , input.RegionDelimited ?? ""
						   , input.BranchDelimited ?? ""
                           , input.NoKeyInMin ?? -1
						   , input.NoKeyInMax ?? -1
						   , input.ForApprovalMin ?? -1
						   , input.ForApprovalMax ?? -1
                           , input.FinalizedMin ?? -1
						   , input.FinalizedMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVarDashboardSummaryForApprovalCLU>> GetListSummaryForApprovalCLU(GetListSummaryForApprovalCLUInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardSummaryForApprovalCLU
              .FromSqlRaw(@"CALL sp_dashboard_summary_for_approval_clu(
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
					)", input.RunIDDelimited ?? ""
						   , input.RegionDelimited ?? ""
						   , input.ClusterDelimited ?? ""
                           , input.NoKeyInMin ?? -1
						   , input.NoKeyInMax ?? -1
						   , input.ForApprovalMin ?? -1
						   , input.ForApprovalMax ?? -1
                           , input.FinalizedMin ?? -1
						   , input.FinalizedMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVarDashboardRegionalWithPosition>> GetListRegionalWithPosition(GetListRegionalWithPositionInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardRegionalWithPosition
              .FromSqlRaw(@"CALL sp_dashboard_regional_with_position(
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
						, {17}
					)", input.RunIDDelimited ?? ""
						   , input.RegionDelimited ?? ""
						   , input.PositionDelimited ?? ""
						   , input.KRAGroupDelimited ?? ""
						   , input.KPIDelimited ?? ""
                           , input.EEMin ?? -1
						   , input.EEMax ?? -1
                           , input.MEMin ?? -1
						   , input.MEMax ?? -1
                           , input.SBEMin ?? -1
						   , input.SBEMax ?? -1
                           , input.BEMin ?? -1
						   , input.BEMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVarDashboardBranchesWithPosition>> GetListBranchesWithPosition(GetListBranchesWithPositionInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardBranchesWithPosition
              .FromSqlRaw(@"CALL sp_dashboard_branches_with_position(
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
						, {17}
					)", input.RunIDDelimited ?? ""
						   , input.BranchDelimited ?? ""
						   , input.PositionDelimited ?? ""
						   , input.KRAGroupDelimited ?? ""
						   , input.KPIDelimited ?? ""
                           , input.EEMin ?? -1
						   , input.EEMax ?? -1
                           , input.MEMin ?? -1
						   , input.MEMax ?? -1
                           , input.SBEMin ?? -1
						   , input.SBEMax ?? -1
                           , input.BEMin ?? -1
						   , input.BEMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }

        public async Task<IEnumerable<TableVarDashboardPositionOnly>> GetListPositionOnly(GetListPositionOnlyInput input, int rowStart)
        {
			return await _dbContext.TableVarDashboardPositionOnly
              .FromSqlRaw(@"CALL sp_dashboard_position_only(
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
					)", input.RunIDDelimited ?? ""
						   , input.PositionDelimited ?? ""
						   , input.KRAGroupDelimited ?? ""
						   , input.KPIDelimited ?? ""
                           , input.EEMin ?? -1
						   , input.EEMax ?? -1
                           , input.MEMin ?? -1
						   , input.MEMax ?? -1
                           , input.SBEMin ?? -1
						   , input.SBEMax ?? -1
                           , input.BEMin ?? -1
						   , input.BEMax ?? -1
                           , input.IsExport
						   , input.sidx ?? ""
						   , input.sord ?? ""
						   , rowStart
						   , input.rows
					   )
			  .AsNoTracking()
			  .ToListAsync();
        }

    }
}