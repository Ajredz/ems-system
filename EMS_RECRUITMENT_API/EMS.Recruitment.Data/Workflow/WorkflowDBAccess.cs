using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.Workflow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.Workflow
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

        Task<IEnumerable<WorkflowStep>> GetWorkflowStepAutoComplete(GetAutoCompleteInput param);

    }

    public class WorkflowDBAccess : IWorkflowDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public WorkflowDBAccess(RecruitmentContext dbContext)
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
                    )", input.ID ?? 0
                      , input.Code ?? ""
                      , input.Description ?? ""
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

        public async Task<IEnumerable<WorkflowStep>> GetWorkflowStepAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.WorkflowStep
                .FromSqlRaw("CALL sp_workflow_step_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}