using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.Reference;
using EMS.IPM.Data.Shared;
using EMS.IPM.Transfer.EmployeeScore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    public interface IEmployeeScoreDBAccess
    {
        Task<IEnumerable<TableVarEmployeeScoreOnly>> GetList(GetListInput input, int rowStart);

        Task<IEnumerable<TableVarEmployeeScore>> GetKeyInApprovalList(APICredentials credentials, GetListInput input, int rowStart);

        Task<bool> Post(List<EmployeeScore> param);

        Task<bool> Put(List<EmployeeScore> toAdd, List<EmployeeScore> toUpdate, List<EmployeeScore> toDelete);

        Task<bool> UpdateEmployeeScore(TransEmployeeScore EmployeeScoreToUpdate, List<TransEmployeeScoreDetails> EmployeeScoreDetailsToUpdate);

        Task<bool> Delete(int ID);

        //Task<bool> BulkDelete(List<int> IDs);

        Task<bool> BulkDeleteEmployeeScore(List<TransEmployeeScore> employeeScoreList);

        Task<bool> BulkVoidEmployeeScore(BulkVoidForm employeeScoreList);

        Task<bool> BulkApprovedEmployeeScore(BulkApprovedForm employeeScoreList);

        Task<IEnumerable<ReferenceValue>> GetRefCodes(List<string> Code);

        Task<IEnumerable<TableVarEmployeeScoreGetByID>> GetByID(GetByIDInput input, int UserID);

        Task<TransEmployeeScore> GetEmployeeScoreByID(int ID);

        Task<IEnumerable<TransEmployeeScore>> GetEmployeeScoreListByID(List<int> IDs);

        Task<List<TransEmployeeScoreDetails>> GetEmployeeScoreDetailsByID(int ID);

        Task<IEnumerable<TransEmployeeScore>> GetAll();

        Task<IEnumerable<TableVarRunEmployeeScore>> RunScores(RunScoreForm param);

        Task<IEnumerable<TableVarAverageScore>> GetAverageScore(int OrgGroup, string KPI);

        Task<IEnumerable<TableVarEmployeeScoreCountByPosition>> GetEmployeeScoreCountByPosition(GetEmployeeScoreDashboardInput input, int rowStart);

        Task<IEnumerable<TableVarEmployeeScoreCountByBranch>> GetEmployeeScoreCountByBranch(GetEmployeeScoreDashboardInput input, int rowStart);

        Task<IEnumerable<TableVarEmployeeScoreCountByRegion>> GetEmployeeScoreCountByRegion(GetEmployeeScoreDashboardInput input, int rowStart);

        //Task<bool> EmployeeScoreApproval(int ApproverID, EmployeeScoreApprovalResponse param);

        Task<IEnumerable<TransEmployeeScore>> RunTransScores(RunScoreForm param, int CreatedBy);

        Task<IEnumerable<TransEmployeeScoreSummary>> GetTransEmployeeScoreProgress(int CreatedBy);
       
        Task<IEnumerable<int>> GetEmployeeOnStaging(int CreatedBy);

        Task<bool> BatchUpdateEmployeeScore(List<TransEmployeeScore> employeeScoreList);

        Task<bool> BatchUpdateEmployeesScore(List<TransEmployeeScore> employeeScoreList);

        Task<IEnumerable<TableVarTransEmployeeScoreSummary>> GetTransEmployeeScoreSummary(int TransSummaryID);

        Task<IEnumerable<TableVariableAutoComplete>> GetSummaryAutoComplete(GetSummaryAutoCompleteInput param);

        Task<bool> UpdateRunDescription(TransEmployeeScoreSummary param);

        Task<IEnumerable<TransEmployeeScoreSummary>> GetTransEmployeeScoreSummaryByID(int ID);

        Task<IEnumerable<TableVarEmployeeKPIScore>> EmployeeKPIScoreGetList(EmployeeKPIScoreGetListInput input, int rowStart);

        Task<IEnumerable<TransEmployeeScoreSummary>> RerunTransScores(RerunForm param, int CreatedBy);

        Task<IEnumerable<TransEmployeeScoreStagingTest>> RunTransScoreInitialize(RunScoreForm param, int CreatedBy);

        Task<IEnumerable<TransEmployeeScoreStagingTest>> CreateTransScoreSummary(RunScoreForm param, int CreatedBy);

        Task<IEnumerable<TransEmployeeScoreSummary>> RunTransScoreFinalize(RunScoreForm param, int CreatedBy);
        Task<IEnumerable<TableVarResult>> GetIPMRaterByTransID(int ID);
        Task<IEnumerable<TransEmployeeFinalScore>> GetIPMFinalScore(int CreatedBy,int RunID);
        Task<IEnumerable<TableVarEmployeeFinalScore>> GetFinalScoreList(GetFinalScoreListInput input, int rowStart);
        Task<IEnumerable<TransEmployeeScoreSummary>> GetRunIDDropDown();
    }

    public class EmployeeScoreDBAccess : IEmployeeScoreDBAccess
    {
        private readonly IPMContext _dbContext;

        public EmployeeScoreDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarEmployeeScoreOnly>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeScoreOnly
              .FromSqlRaw(@"CALL sp_employee_score_get_list(
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
             	, {18}
             	, {19}
             	, {20}
                , {21}
                , {22}
                , {23}
                , {24}
                , {25}
             )"
                           , input.TransSummaryIDDelimited ?? ""
                           , input.Description ?? ""
                           , input.IsActiveDelimited ?? ""
                           , input.NameDelimited ?? ""
                           , input.ParentOrgGroup ?? ""
                           , input.OrgGroupDelimited ?? ""
                           , input.PositionDelimited ?? ""
                           , input.DateFromFrom ?? ""
                           , input.DateFromTo ?? ""
                           , input.DateToFrom ?? ""
                           , input.DateToTo ?? ""
                           , input.DateEffectiveFromFrom ?? ""
                           , input.DateEffectiveFromTo ?? ""
                           , input.DateEffectiveToFrom ?? ""
                           , input.DateEffectiveToTo ?? ""
                           , input.ScoreFrom ?? -1
                           , input.ScoreTo ?? -1
                           , input.StatusDelimited ?? ""
                           , input.ShowForEvaluation
                           , input.ShowNoScore
                           , input.IsExport
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                           , input.isShowAll
                       )
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeScore>> GetKeyInApprovalList(APICredentials credentials, GetListInput input, int rowStart)
        {

            List<TableVarEmployeeScore> results = new List<TableVarEmployeeScore>();

            try 
            {

                results = await _dbContext.TableVarEmployeeScore
               .FromSqlRaw(@"CALL sp_employee_score_get_keyin_approval_list(
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
                        , {18}
                        , {19}
                        , {20}
                        , {21}
					)", input.TransSummaryIDDelimited ?? ""
                           , input.Description ?? ""
                           , input.NameDelimited ?? ""
                           , input.ParentOrgGroup ?? ""
                           , input.OrgGroupDelimited ?? ""
                           , input.PositionDelimited ?? ""
                           , input.DateFromFrom ?? ""
                           , input.DateFromTo ?? ""
                           , input.DateToFrom ?? ""
                           , input.DateToTo ?? ""
                           , input.ScoreFrom ?? -1
                           , input.ScoreTo ?? -1
                           , input.StatusDelimited ?? ""
                           , input.ShowForEvaluation
                           , input.ShowNoScore
                           , input.IsExport
                           , credentials.UserID
                           , input.isApproval
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                       )
               .AsNoTracking()
               .ToListAsync();
            } 
            catch (Exception ex) 
            {
            
            }

            return results;
             //       return await _dbContext.TableVarEmployeeScore
             //           .FromSqlRaw(@"CALL sp_employee_score_get_keyin_approval_list(
             //                     {0}
             //                   , {1}
             //                   , {2}
             //                   , {3}
             //                   , {4}
             //                   , {5}
             //                   , {6}
             //                   , {7}
             //                   , {8}
             //                   , {9}
             //                   , {10}
             //                   , {11}
             //                   , {12}
             //                   , {13}
             //                   , {14}
             //                   , {15}
             //                   , {16}
             //                   , {17}
             //                   , {18}
             //                   , {19}
             //                   , {20}
             //                   , {21}
             //)", input.TransSummaryIDDelimited ?? ""
             //                       , input.Description ?? ""
             //                       , input.NameDelimited ?? ""
             //                       , input.ParentOrgGroup ?? ""
             //                       , input.OrgGroupDelimited ?? ""
             //                       , input.PositionDelimited ?? ""
             //                       , input.DateFromFrom ?? ""
             //                       , input.DateFromTo ?? ""
             //                       , input.DateToFrom ?? ""
             //                       , input.DateToTo ?? ""
             //                       , input.ScoreFrom ?? -1
             //                       , input.ScoreTo ?? -1
             //                       , input.StatusDelimited ?? ""
             //                       , input.ShowForEvaluation
             //                       , input.ShowNoScore
             //                       , input.IsExport
             //                       , credentials.UserID
             //                       , input.isApproval
             //                       , input.sidx ?? ""
             //                       , input.sord ?? ""
             //                       , rowStart
             //                       , input.rows
             //                   )
             //           .AsNoTracking()
             //           .ToListAsync();
        }

        public async Task<bool> Post(List<EmployeeScore> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeScore.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> Put(List<EmployeeScore> toAdd, List<EmployeeScore> toUpdate, List<EmployeeScore> toDelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (toAdd != null)
                {
                    _dbContext.EmployeeScore.AddRange(toAdd);
                }

                if (toUpdate != null)
                {
                    foreach (var e in toUpdate)
                    {
                        var score = _dbContext.EmployeeScore.First(x => x.ID == e.ID);
                        _dbContext.Entry(score).CurrentValues.SetValues(e);
                    }
                    //toUpdate
                    //.Select(x =>
                    //{
                    //    _dbContext.Entry(x).State = EntityState.Modified;
                    //    return x;
                    //}).ToList();
                }

                if (toDelete != null)
                {
                    _dbContext.EmployeeScore.RemoveRange(toDelete);
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> UpdateEmployeeScore(TransEmployeeScore EmployeeScoreToUpdate, List<TransEmployeeScoreDetails> EmployeeScoreDetailsToUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(EmployeeScoreToUpdate).State = EntityState.Modified;

                if (EmployeeScoreDetailsToUpdate != null)
                {
                    foreach (var e in EmployeeScoreDetailsToUpdate)
                    {
                        var score = _dbContext.TransEmployeeScoreDetails.First(x => x.ID == e.ID);
                        _dbContext.Entry(score).CurrentValues.SetValues(e);
                    }
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            //update trans employee score summary
            await _dbContext.TableVarTransEmployeeScoreSummaryID
                .FromSqlRaw("CALL sp_update_trans_employee_score_summary({0})", EmployeeScoreToUpdate.TransSummaryID)
                .AsNoTracking()
                .ToListAsync();

            return true;
        }

        public async Task<bool> Delete(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeScore.RemoveRange(_dbContext.EmployeeScore.Where(x => x.TID == ID));

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        //public async Task<bool> BulkDelete(List<int> IDs)
        //{
        //    using (var transaction = _dbContext.Database.BeginTransaction())
        //    {
        //        _dbContext.TransEmployeeScore.RemoveRange(_dbContext.TransEmployeeScore.Where(x => IDs.Contains(x.ID) && x.Status == "NEW"));

        //        await _dbContext.SaveChangesAsync();
        //        transaction.Commit();
        //    }

        //    return true;
        //}

        public async Task<bool> BulkVoidEmployeeScore(BulkVoidForm employeeScoreList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TransEmployeeScore.Where(w => employeeScoreList.IDs.Contains(w.ID)).ToList()
                    .ForEach(x => x.Status = "VOID");

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> BulkApprovedEmployeeScore(BulkApprovedForm employeeScoreList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TransEmployeeScore.Where(w => employeeScoreList.IDs.Contains(w.ID)).ToList()
                    .ForEach(x => x.Status = "APPROVED");

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> BulkDeleteEmployeeScore(List<TransEmployeeScore> employeeScoreList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employeeScoreList.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;

                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarEmployeeScoreGetByID>> GetByID(GetByIDInput input, int UserID)
        {
            return await _dbContext.TableVarEmployeeScoreGetByID
                .FromSqlRaw("CALL sp_employee_score_get_by_id({0},{1},{2})", input.ID, input.RoleIDs, UserID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TransEmployeeScore> GetEmployeeScoreByID(int ID)
        {
            return await _dbContext.TransEmployeeScore.FindAsync(ID);
        }

        public async Task<IEnumerable<TransEmployeeScore>> GetEmployeeScoreListByID(List<int> IDs)
        {
            return await _dbContext.TransEmployeeScore.AsNoTracking()
                .Where(x => IDs.Contains(x.ID)).Where(x => x.isActive).ToListAsync();
        }

        public async Task<List<TransEmployeeScoreDetails>> GetEmployeeScoreDetailsByID(int ID)
        {
            return await _dbContext.TransEmployeeScoreDetails.AsNoTracking()
                                            .Where(x => x.TransID == ID)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransEmployeeScore>> GetAll()
        {
            return await _dbContext.TransEmployeeScore.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVarRunEmployeeScore>> RunScores(RunScoreForm param)
        {
            return await _dbContext.TableVarRunEmployeeScore
                .FromSqlRaw("CALL sp_run_employee_score({0}, {1}, {2}, {3}, {4}, {5})", param.Filter ?? "",
                                                                                        param.IDs ?? "",
                                                                                        param.Employees ?? "",
                                                                                        param.DateFrom,
                                                                                        param.DateTo,
                                                                                        param.UseCurrent)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TransEmployeeScore>> RunTransScores(RunScoreForm param, int CreatedBy)
        {
            List<TransEmployeeScore> results = new List<TransEmployeeScore>();
            try 
            {

                results = await _dbContext.TransEmployeeScore
                .FromSqlRaw(@"CALL sp_run_trans_employee_score_process_by_batch(
                             {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            )",
                             param.DateFrom,
                             param.DateTo,
                             param.TransSummaryID,
                             CreatedBy,
                             param.strEmployeeIDList,
                             param.Pk

                         )
             .AsNoTracking()
             .ToListAsync();


            }
            catch (Exception ex) 
            { 
            
            }
            
            return results;

            //return await _dbContext.TransEmployeeScore
            //.FromSqlRaw(@"CALL sp_run_trans_employee_score_process_by_batch(
            //                 {0}
            //                , {1}
            //                , {2}
            //                , {3}
            //                , {4}
            //                , {5}
            //                )",
            //                 param.DateFrom,
            //                 param.DateTo,
            //                 param.TransSummaryID,
            //                 CreatedBy,
            //                 param.strEmployeeIDList,
            //                 param.Pk

            //             )
            // .AsNoTracking()
            // .ToListAsync();
        }

        public async Task<IEnumerable<TableVarAverageScore>> GetAverageScore(int OrgGroup, string KPI)
        {
            return await _dbContext.TableVarAverageScore
                .FromSqlRaw("CALL sp_get_average_scores({0}, {1})", OrgGroup, KPI)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeScoreCountByPosition>> GetEmployeeScoreCountByPosition(GetEmployeeScoreDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeScoreCountByPosition
               .FromSqlRaw(@"CALL sp_employee_score_get_count_by_position(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                        )",
                            input.Dashboard1Input.Position ?? ""
                          , input.Dashboard1Input.CountMin
                          , input.Dashboard1Input.CountMax
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeScoreCountByBranch>> GetEmployeeScoreCountByBranch(GetEmployeeScoreDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeScoreCountByBranch
               .FromSqlRaw(@"CALL sp_employee_score_get_count_by_branch(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Dashboard2Input.Branch ?? ""
                          , input.Dashboard2Input.Position ?? ""
                          , input.Dashboard2Input.CountMin
                          , input.Dashboard2Input.CountMax
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeScoreCountByRegion>> GetEmployeeScoreCountByRegion(GetEmployeeScoreDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeScoreCountByRegion
               .FromSqlRaw(@"CALL sp_employee_score_get_count_by_region(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Dashboard3Input.Region ?? ""
                          , input.Dashboard3Input.Position ?? ""
                          , input.Dashboard3Input.CountMin
                          , input.Dashboard3Input.CountMax
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        //public async Task<bool> EmployeeScoreApproval(int ApproverID, EmployeeScoreApprovalResponse param)
        //{
        //    await _dbContext.TableVarEmployeeScoreApproval
        //    .FromSqlRaw(@"CALL sp_employee_score_approval(
        //                    {0}
        //                , {1}
        //                , {2}
        //                , {3}
        //            )",
        //                ApproverID
        //                , param.TID
        //                , param.Status.ToString()
        //                , param.Remarks
        //        )
        //    .AsNoTracking()
        //    .ToListAsync();
        //    return true;
        //}

        public async Task<IEnumerable<TransEmployeeScoreSummary>> GetTransEmployeeScoreProgress(int CreatedBy)
        {
            // GET IF THERE ARE ACTIVE RUN OR RE RUN
            return await _dbContext.TransEmployeeScoreSummary.AsNoTracking().Where(y => !y.IsDone).ToListAsync();
        }

        public async Task<IEnumerable<int>> GetEmployeeOnStaging(int CreatedBy)
        {
            return await _dbContext.StagingTransEmployeeScore.Select(x => x.EmployeeID).Distinct().ToListAsync();
        }

        public async Task<bool> BatchUpdateEmployeeScore(List<TransEmployeeScore> employeeScoreList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employeeScoreList.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> BatchUpdateEmployeesScore(List<TransEmployeeScore> employeeScoreList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employeeScoreList.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarTransEmployeeScoreSummary>> GetTransEmployeeScoreSummary(int TransSummaryID)
        {
            return await _dbContext.TableVarTransEmployeeScoreSummary
                .FromSqlRaw("CALL sp_trans_employee_score_summary_get_by_id({0})", TransSummaryID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVariableAutoComplete>> GetSummaryAutoComplete(GetSummaryAutoCompleteInput param)
        {
            return await _dbContext.TableVariableAutoComplete
                .FromSqlRaw("CALL sp_trans_employee_score_summary_autocomplete({0},{1},{2})", (param.Term ?? ""), param.TopResults, param.IsAdmin)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UpdateRunDescription(TransEmployeeScoreSummary param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (param != null)
                {
                    _dbContext.Entry(param).State = EntityState.Modified;
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }
        public async Task<IEnumerable<ReferenceValue>> GetRefCodes(List<string> RefCodes)
        {
            return await _dbContext.ReferenceValue.AsNoTracking()
                .Where(x => RefCodes.Contains(x.RefCode))
                .Distinct().ToListAsync();
        }

        public async Task<IEnumerable<TransEmployeeScoreSummary>> GetTransEmployeeScoreSummaryByID(int ID)
        {
            return await _dbContext.TransEmployeeScoreSummary
                .AsNoTracking().Where(x => x.ID == ID).ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeKPIScore>> EmployeeKPIScoreGetList(EmployeeKPIScoreGetListInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeKPIScore
              .FromSqlRaw(@"CALL sp_employee_kpi_score_get_list_by_id(
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
						, {18}
						, {19}
						, {20}
                        , {21}
					)", input.ID ?? 0
                           , input.KRAGroup ?? ""
                           , input.KPICode ?? ""
                           , input.KPIName ?? ""
                           , input.KPIDescription ?? ""
                           , input.KPIGuidelines ?? ""
                           , input.WeightMin ?? -1
                           , input.WeightMax ?? -1
                           , input.TargetMin ?? -1
                           , input.TargetMax ?? -1
                           , input.ActualMin ?? -1
                           , input.ActualMax ?? -1
                           , input.RateMin ?? -1
                           , input.RateMax ?? -1
                           , input.TotalMin ?? -1
                           , input.TotalMax ?? -1
                           , input.GradeDelimited ?? ""
                           , input.SourceTypeDelimited ?? ""
                           , input.sidx ?? ""
                           , input.sord ?? ""
                           , rowStart
                           , input.rows
                       )
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<IEnumerable<TransEmployeeScoreSummary>> RerunTransScores(RerunForm param, int CreatedBy)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TransEmployeeScoreSummary.Where(w => w.ID == param.RunID).ToList()
                    .ForEach(x => {
                        x.IsDone = false;
                        x.ProcessedEmployees = 0;
                        x.TotalNumOfEmployees = 0;
                        x.EmployeesWithIPM = 0;
                    });

                //.ForEach(x => x.IsDone = false);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return await _dbContext.TransEmployeeScoreSummary.Where(w => w.ID == param.RunID).ToListAsync();
        }

        public async Task<IEnumerable<TransEmployeeScoreStagingTest>> RunTransScoreInitialize(RunScoreForm param, int CreatedBy)
        {
            List<TransEmployeeScoreStagingTest> results = new List<TransEmployeeScoreStagingTest>();

            try 
            {
                results = await _dbContext.TransEmployeeScoreStagingTest
               .FromSqlRaw(@"CALL sp_run_trans_employee_score_initialize(
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
                                )", param.Description ?? "",
                             param.Filter ?? "",
                             param.IDs ?? "",
                             param.Employees ?? "",
                             param.DateFrom,
                             param.DateTo,
                             param.UseCurrent,
                             param.RegularOnly,
                             param.IncludeAllLevelsBelow,
                             param.Override,
                             param.TransSummaryID,
                             param.IncludeSecDesig,
                             param.RoleIDs,
                             CreatedBy

                         )
                 .AsNoTracking()
                 .ToListAsync();
            } catch (Exception ex) 
            { 
            
            }
           

            return results;

            //return await _dbContext.TransEmployeeScoreStagingTest
            //    .FromSqlRaw(@"CALL sp_run_trans_employee_score_initialize(
            //                {0}
            //                , {1}
            //                , {2}
            //                , {3}
            //                , {4}
            //                , {5}
            //                , {6}
            //                , {7}
            //                , {8}
            //                , {9}
            //                , {10}
            //                , {11}
            //                , {12}
            //                , {13}
            //                )", param.Description ?? "",
            //                  param.Filter ?? "",
            //                  param.IDs ?? "",
            //                  param.Employees ?? "",
            //                  param.DateFrom,
            //                  param.DateTo,
            //                  param.UseCurrent,
            //                  param.RegularOnly,
            //                  param.IncludeAllLevelsBelow,
            //                  param.Override,
            //                  param.TransSummaryID,
            //                  param.IncludeSecDesig,
            //                  param.RoleIDs,
            //                  CreatedBy

            //              )
            //  .AsNoTracking()
            //  .ToListAsync();
        }

        public async Task<IEnumerable<TransEmployeeScoreStagingTest>> CreateTransScoreSummary(RunScoreForm param, int CreatedBy)
        {


            List<TransEmployeeScoreStagingTest> results = new List<TransEmployeeScoreStagingTest>();

            try
            {
                results = await _dbContext.TransEmployeeScoreStagingTest
                  .FromSqlRaw(@"CALL sp_create_trans_employee_score_summary(
                                            {0}
                                           ,{1}
                                           ,{2}
                                           ,{3}
                                           ,{4}
                                           ,{5}
                                           ,{6}
                                           ,{7}
                                           ,{8}
                                           ,{9}
                                            )",
                              param.Description ?? ""
                            , param.Filter ?? ""
                            , param.IDs ?? ""
                            , param.Employees ?? ""
                            , param.DateFrom
                            , param.DateTo
                            , param.UseCurrent
                            , param.IncludeAllLevelsBelow
                            , param.Override
                            , CreatedBy

                            )
                  .AsNoTracking()
                  .ToListAsync();

                
            }
            catch (Exception ex)
            {

            }
            return results;

            //return await _dbContext.TransEmployeeScoreStagingTest
            //      .FromSqlRaw(@"CALL sp_create_trans_employee_score_summary(
            //                {0}
            //               ,{1}
            //               ,{2}
            //               ,{3}
            //               ,{4}
            //               ,{5}
            //               ,{6}
            //               ,{7}
            //               ,{8}
            //               ,{9}
            //                )",
            //                  param.Description ?? ""
            //                , param.Filter ?? ""
            //                , param.IDs ?? ""
            //                , param.Employees ?? ""
            //                , param.DateFrom
            //                , param.DateTo
            //                , param.UseCurrent
            //                , param.IncludeAllLevelsBelow
            //                , param.Override
            //                , CreatedBy

            //                )
            //      .AsNoTracking()
            //      .ToListAsync();

        }

        public async Task<IEnumerable<TransEmployeeScoreSummary>> RunTransScoreFinalize(RunScoreForm param, int CreatedBy)
        {
            List<TransEmployeeScoreSummary> results = new List<TransEmployeeScoreSummary>();

            try
            {
                results = await _dbContext.TransEmployeeScoreSummary
                .FromSqlRaw(@"CALL sp_run_trans_employee_score_finalize(
                            {0}
                            ,{1}
                            ,{2}
                            ,{3}
                            )",
                       param.TransSummaryID,
                       param.Override,
                       param.RoleIDs,
                       CreatedBy
                       )
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception ex) 
            { 
            
            }

           
              

            return results;

            //return await _dbContext.TransEmployeeScoreSummary
            //      .FromSqlRaw(@"CALL sp_run_trans_employee_score_finalize(
            //                {0}
            //                ,{1}
            //                ,{2}
            //                ,{3}
            //                )",
            //                param.TransSummaryID,
            //                param.Override,
            //                param.RoleIDs,
            //                CreatedBy
            //                )
            //      .AsNoTracking()
            //      .ToListAsync();

        }
        public async Task<IEnumerable<TableVarResult>> GetIPMRaterByTransID(int ID)
        {
            return await _dbContext.TableVarResult
              .FromSqlRaw(@"CALL sp_get_ipm_rater({0})", ID).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TransEmployeeFinalScore>> GetIPMFinalScore(int RunID, int CreatedBy)
        {
            return await _dbContext.TransEmployeeFinalScore
              .FromSqlRaw(@"CALL sp_run_get_final_score({0},{1})", RunID,CreatedBy).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeFinalScore>> GetFinalScoreList(GetFinalScoreListInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeFinalScore
                .FromSqlRaw(@"CALL sp_employee_final_score_get_list(
                    {0}
                    ,{1}
                    ,{2}
                    ,{3}
                    ,{4}
                    ,{5}
                    ,{6}
                    ,{7}
                    ,{8}
                    ,{9}
                    ,{10}
                    ,{11}
                    ,{12}
                    ,{13}
                    ,{14}
                    )"
                    , input.ID
                    , input.RunIDDelimited ?? ""
                    , input.EmployeeIDDelimited ?? ""
                    , input.IPMCount ?? ""
                    , input.IPMMonths ?? ""
                    , input.FinalScoreFrom ?? -1
                    , input.FinalScoreTo ?? -1
                    , input.CreatedBy ?? ""
                    , input.CreatedDateFrom ?? ""
                    , input.CreatedDateTo ?? ""
                    , input.IsExport

                    , input.sidx ?? ""
                    , input.sord ?? ""
                    , rowStart
                    , input.rows
                )
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TransEmployeeScoreSummary>> GetRunIDDropDown()
        {
            return await _dbContext.TransEmployeeScoreSummary.AsNoTracking().ToListAsync();
        }
    }
}