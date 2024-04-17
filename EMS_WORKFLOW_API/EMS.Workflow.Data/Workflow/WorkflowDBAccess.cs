using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Workflow.Data.Workflow
{
    public interface IWorkflowDBAccess
    {
        Task<IEnumerable<TableVarWorkflow>> GetList(GetListInput input, int rowStart);

        Task<Workflow> GetByID(int ID);

        Task<IEnumerable<Workflow>> GetByCode(string Code);

        Task<IEnumerable<WorkflowStep>> GetWorkflowStep(int ID);

        Task<IEnumerable<WorkflowStep>> GetWorkflowStepByCode(List<string> lstCode);

        Task<IEnumerable<WorkflowStepApprover>> GetWorkflowStepApprover(int ID);

        Task<IEnumerable<WorkflowStepApprover>> GetWorkflowStepApproverByCode(List<string> lstCode);

        Task<bool> Post(Workflow workflow, List<WorkflowStep> lstWorkflowStep, List<WorkflowStepApprover> lstWorkflowStepApprover);

        Task<bool> Delete(int ID);

        Task<bool> Put(Workflow workflow, List<WorkflowStep> toDeleteWorkflowSteps, List<WorkflowStepApprover> toDeleteWorkflowStepApprovers
                    , List<WorkflowStep> toAddWorkflowSteps, List<WorkflowStepApprover> toAddWorkflowStepApprovers
                    , List<WorkflowStep> toUpdateWorkflowSteps, List<WorkflowStepApprover> toUpdateWorkflowStepApprovers);

        Task<IEnumerable<Workflow>> GetAll();

        Task<IEnumerable<Workflow>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<WorkflowStep>> GetWorkflowStepAutoComplete(GetWorkflowStepAutoCompleteInput param);

        Task<IEnumerable<TableVarTransaction>> GetTransactionByRecordID(GetTransactionByRecordIDInput param);

        Task<TableVarCurrentWorkflowStep> AddTransaction(int ApproverID, AddWorkflowTransaction param);
        Task<IEnumerable<TableVarTransactionLastUpdate>> GetLastStatusUpdateByRecordIDs(List<int> IDs);

        Task<IEnumerable<WorkflowStep>> GetWorkflowStepByWorkflowID(int WorkflowID);

        Task<IEnumerable<WorkflowStep>> GetWorkflowStepByWorkflowCode(int WorkflowID);

        Task<WorkflowStep> GetWorkflowStepByWorkflowCodeAndCode(int WorkflowID, string Code);

        Task<IEnumerable<TableVarWorkflowGetNextWorkflowStep>> GetNextWorkflowStep(GetNextWorkflowStepInput param);

        Task<IEnumerable<TableVarWorkflowGetWorkflowByRoleStep>> GetWorkflowStepByRole(GetWorkflowStepByRoleInput param);

        Task<IEnumerable<TableVarWorkflowGetAllWorkflowStep>> GetAllWorkflowStep(GetAllWorkflowStepInput param);

        Task<IEnumerable<TableVarWorkflowStepApprover>> GetRolesByWorkflowStepCode(GetRolesByWorkflowStepCodeInput param);
    }

    public class WorkflowDBAccess : IWorkflowDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public WorkflowDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarWorkflow>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarWorkflow
                .FromSqlRaw(@"CALL sp_workflow_get_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                    )", input.ID ?? 0
                      , input.Code ?? ""
                      , input.Description ?? ""
                      , input.IsExport
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Workflow> GetByID(int ID)
        {
            return await _dbContext.Workflow.FindAsync(ID);
        }

        public async Task<IEnumerable<Workflow>> GetByCode(string Code)
        {
            return await _dbContext.Workflow.AsNoTracking()
                .Where(x => x.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStep>> GetWorkflowStep(int ID)
        {
            return await _dbContext.WorkflowStep.AsNoTracking()
                .Where(x => x.WorkflowId == ID)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStep>> GetWorkflowStepByCode(List<string> lstCode)
        {
            return await _dbContext.WorkflowStep.AsNoTracking()
                .Where(x => lstCode.Contains(x.Code))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStepApprover>> GetWorkflowStepApprover(int ID)
        {
            return await _dbContext.WorkflowStepApprover.AsNoTracking()
                .Where(x => x.WorkflowId == ID)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStepApprover>> GetWorkflowStepApproverByCode(List<string> lstCode)
        {
            return await _dbContext.WorkflowStepApprover.AsNoTracking()
                .Where(x => lstCode.Contains(x.StepCode))
                .ToListAsync();
        }

        public async Task<bool> Post(Workflow workflow, List<WorkflowStep> lstWorkflowStep, List<WorkflowStepApprover> lstWorkflowStepApprover)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Workflow.AddAsync(workflow);
                await _dbContext.SaveChangesAsync();

                if (lstWorkflowStep != null)
                {
                    await _dbContext.WorkflowStep.AddRangeAsync(lstWorkflowStep
                        .OrderByDescending(y => y.Order)
                        .Select(x => { x.WorkflowId = workflow.ID; return x; }));
                }

                if (lstWorkflowStepApprover != null)
                {
                    await _dbContext.WorkflowStepApprover.AddRangeAsync(lstWorkflowStepApprover.Select(x => { x.WorkflowId = workflow.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Delete(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                IEnumerable<WorkflowStep> WorkflowStepList = await _dbContext.WorkflowStep.AsNoTracking()
                    .Where(x => x.WorkflowId == ID)
                    .ToListAsync();

                IEnumerable<WorkflowStepApprover> WorkflowStepApproverList = await _dbContext.WorkflowStepApprover.AsNoTracking()
                    .Where(x => x.WorkflowId == ID)
                    .ToListAsync();

                if (WorkflowStepApproverList != null)
                {
                    _dbContext.WorkflowStepApprover.RemoveRange(WorkflowStepApproverList);
                }

                if (WorkflowStepList != null)
                {
                    _dbContext.WorkflowStep.RemoveRange(WorkflowStepList);
                }

                _dbContext.Workflow.Remove(new Workflow() { ID = ID });

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(Workflow workflow
            , List<WorkflowStep> toDeleteWorkflowSteps, List<WorkflowStepApprover> toDeleteWorkflowStepApprovers
            , List<WorkflowStep> toAddWorkflowSteps, List<WorkflowStepApprover> toAddWorkflowStepApprovers
            , List<WorkflowStep> toUpdateWorkflowSteps, List<WorkflowStepApprover> toUpdateWorkflowStepApprovers)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(workflow).State = EntityState.Modified;

                // Execute filtered records to their respective actions.
                _dbContext.WorkflowStep.RemoveRange(toDeleteWorkflowSteps);
                _dbContext.WorkflowStepApprover.RemoveRange(toDeleteWorkflowStepApprovers);

                _dbContext.WorkflowStep.AddRange(toAddWorkflowSteps);
                _dbContext.WorkflowStepApprover.AddRange(toAddWorkflowStepApprovers);

                toUpdateWorkflowSteps.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                toUpdateWorkflowStepApprovers.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<Workflow>> GetAll()
        {
            return await _dbContext.Workflow.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Workflow>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Workflow
                .FromSqlRaw("CALL sp_workflow_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStep>> GetWorkflowStepAutoComplete(GetWorkflowStepAutoCompleteInput param)
        {
            return await _dbContext.WorkflowStep
                .FromSqlRaw("CALL sp_workflow_step_autocomplete({0},{1},{2})", param.Term ?? "", param.TopResults, param.WorkflowCode)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarTransaction>> GetTransactionByRecordID(GetTransactionByRecordIDInput param)
        {
            return await _dbContext.TableVarTransaction
                    .FromSqlRaw("CALL sp_workflow_get_transaction_by_record_id({0},{1})", param.WorkflowID, param.RecordID)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<TableVarCurrentWorkflowStep> AddTransaction(int ApproverID, AddWorkflowTransaction param)
        {
            List<TableVarCurrentWorkflowStep> result = new List<TableVarCurrentWorkflowStep>();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                result = await _dbContext.TableVarCurrentWorkflowStep
              .FromSqlRaw(@"CALL sp_workflow_add_transaction(
                {0}
                ,{1}
                ,{2}
                ,{3}
                ,{4}
                ,{5}
                ,{6}
                ,{7}
                ,{8}
                )"
              , param.WorkflowCode
              //, param.RequestType
              , param.CurrentStepCode
              , ApproverID
              , param.RecordID
              , param.Result.ToString()
              , param.Remarks
              , param.DateScheduled
              , param.DateCompleted
              , param.StartDatetime
              )
              .AsNoTracking().ToListAsync();

                transaction.Commit();
            }
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TableVarTransactionLastUpdate>> GetLastStatusUpdateByRecordIDs(List<int> IDs)
        {
            return await _dbContext.TableVarTransactionLastUpdate
                    .FromSqlRaw("CALL sp_get_workflow_transaction_by_ids({0})"
                    , string.Join(",", IDs)
                    )
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStep>> GetWorkflowStepByWorkflowID(int WorkflowID)
        {
            return await _dbContext.WorkflowStep.AsNoTracking()
                .Where(x => x.WorkflowId == WorkflowID).ToListAsync();
        }

        public async Task<IEnumerable<WorkflowStep>> GetWorkflowStepByWorkflowCode(int WorkflowID)
        {
            return await _dbContext.WorkflowStep.AsNoTracking()
                .Where(x => x.WorkflowId == WorkflowID).ToListAsync();
        }

        public async Task<WorkflowStep> GetWorkflowStepByWorkflowCodeAndCode(int WorkflowID, string Code)
        {
            return await _dbContext.WorkflowStep.AsNoTracking()
                .Where(x => x.WorkflowId == WorkflowID & x.Code.Equals(Code)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TableVarWorkflowGetNextWorkflowStep>> GetNextWorkflowStep(GetNextWorkflowStepInput param)
        {
            return await _dbContext.TableVarWorkflowGetNextWorkflowStep
                    .FromSqlRaw("CALL sp_workflow_get_next_workflow_step({0},{1},{2})"
                    , param.WorkflowCode
                    , param.CurrentStepCode
                    , param.RoleIDDelimited
                    )
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<TableVarWorkflowGetWorkflowByRoleStep>> GetWorkflowStepByRole(GetWorkflowStepByRoleInput param)
        {
            return await _dbContext.TableVarWorkflowGetWorkflowByRoleStep
                    .FromSqlRaw("CALL sp_workflow_get_workflow_step_by_role({0},{1})"
                    , param.WorkflowCode
                    , param.RoleIDDelimited
                    )
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<TableVarWorkflowGetAllWorkflowStep>> GetAllWorkflowStep(GetAllWorkflowStepInput param)
        {
            return await _dbContext.TableVarWorkflowGetAllWorkflowStep
                    .FromSqlRaw("CALL sp_workflow_get_all_workflow_step({0})"
                    , param.WorkflowCode
                    )
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<TableVarWorkflowStepApprover>> GetRolesByWorkflowStepCode(GetRolesByWorkflowStepCodeInput param)
        {
            return await _dbContext.TableVarWorkflowStepApprover
                    .FromSqlRaw("CALL sp_workflow_get_roles_by_workflow_step_code({0},{1})", param.WorkflowCode, param.StepCode)
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}