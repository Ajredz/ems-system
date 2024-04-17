using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.Workflow;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.MRFSignatories
{
    public interface IMRFSignatoriesDBAccess
    {
        Task<IEnumerable<TableVarMRFSignatories>> GetByRolePosition(int RoleID, int PositionID);

        Task<bool> Delete(MRFSignatories toDeleteSignatories, Data.Workflow.Workflow toDeleteWorkflow
            , List<WorkflowStep> toDeleteWorkflowSteps, List<WorkflowStepApprover> toDeleteWorkflowStepApprovers);

        Task<bool> Put(List<WorkflowStep> toDeleteWorkflowSteps, List<WorkflowStepApprover> toDeleteWorkflowStepApprovers
            , List<WorkflowStep> toAddWorkflowSteps, List<WorkflowStepApprover> toAddWorkflowStepApprovers
            , List<WorkflowStep> toUpdateWorkflowSteps, List<WorkflowStepApprover> toUpdateWorkflowStepApprovers);

        Task<bool> Post(MRFSignatories toAddSignatories, Data.Workflow.Workflow toAddWorkflow
            , List<WorkflowStep> toAddWorkflowSteps, List<WorkflowStepApprover> toAddWorkflowStepApprovers);

        Task<IEnumerable<TableVarMRFSignatoriesAdd>> GetMRFSignatoriesAdd(int RecordID, int RequesterID, int PositionID);
    }

    public class MRFSignatoriesDBAccess : IMRFSignatoriesDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public MRFSignatoriesDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarMRFSignatories>> GetByRolePosition(int RoleID, int PositionID)
        {
            return await _dbContext.TableVarMRFSignatories
                .FromSqlRaw("CALL sp_mrf_signatories_get_by_requester_position({0},{1})", RoleID, PositionID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> Delete(MRFSignatories toDeleteSignatories, Data.Workflow.Workflow toDeleteWorkflow
           , List<WorkflowStep> toDeleteWorkflowSteps, List<WorkflowStepApprover> toDeleteWorkflowStepApprovers)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.MRFSignatories.Remove(toDeleteSignatories);
                _dbContext.Workflow.Remove(toDeleteWorkflow);
                _dbContext.WorkflowStep.RemoveRange(toDeleteWorkflowSteps);
                _dbContext.WorkflowStepApprover.RemoveRange(toDeleteWorkflowStepApprovers);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> Put(
            List<WorkflowStep> toDeleteWorkflowSteps, List<WorkflowStepApprover> toDeleteWorkflowStepApprovers
            , List<WorkflowStep> toAddWorkflowSteps, List<WorkflowStepApprover> toAddWorkflowStepApprovers
            , List<WorkflowStep> toUpdateWorkflowSteps, List<WorkflowStepApprover> toUpdateWorkflowStepApprovers
            )
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
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

        public async Task<bool> Post(MRFSignatories toAddSignatories, Data.Workflow.Workflow toAddWorkflow
            , List<WorkflowStep> toAddWorkflowSteps, List<WorkflowStepApprover> toAddWorkflowStepApprovers)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.MRFSignatories.Add(toAddSignatories);
                _dbContext.Workflow.Add(toAddWorkflow);
                await _dbContext.SaveChangesAsync();

                _dbContext.WorkflowStep.AddRange(
                    toAddWorkflowSteps.OrderByDescending(y => y.Order)
                    // Update WorkflowID from the inserted IDs of Workflow table
                    .Select(x => { x.WorkflowID = toAddWorkflow.ID; return x; })
                );
                _dbContext.WorkflowStepApprover.AddRange(
                    // Update WorkflowID from the inserted IDs of Workflow table
                    toAddWorkflowStepApprovers
                    .Select(x => { x.WorkflowID = toAddWorkflow.ID; return x; })
                    );

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<TableVarMRFSignatoriesAdd>> GetMRFSignatoriesAdd(int RecordID, int RequesterID, int PositionID)
        { 
            return await _dbContext.TableVarMRFSignatoriesAdd
                 .FromSqlRaw("CALL sp_mrf_get_signatories_add({0},{1},{2})", RecordID, RequesterID, PositionID)
                .AsNoTracking().ToListAsync();
        }
    }
}