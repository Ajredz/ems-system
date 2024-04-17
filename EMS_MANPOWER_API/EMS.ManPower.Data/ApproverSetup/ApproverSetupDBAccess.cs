using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.ApproverSetup;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.ApproverSetup
{
    public interface IApproverSetupDBAccess
    {
        Task<IEnumerable<TableVarMRFDefinedApprover>> GetList(GetListInput input, int rowStart);

        Task<IEnumerable<TableVarApproverSetupGet>> GetByID(int ID);

        Task<bool> Put(List<MRFDefinedApprover> toAdd, List<MRFDefinedApprover> toUpdate, List<MRFDefinedApprover> toDelete);

        Task<IEnumerable<MRFDefinedApprover>> GetMRFDefinedApproverByOrgGroupID(int OrgGroupID);
        Task<IEnumerable<MRFDefinedApprover>> GetSetupMRFApproverInsert();
        Task<IEnumerable<MRFDefinedApprover>> GetSetupMRFApproverUpdate();
    }

    public class ApproverSetupDBAccess : IApproverSetupDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public ApproverSetupDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarMRFDefinedApprover>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarMRFDefinedApprover
               .FromSqlRaw(@"CALL sp_approver_setup_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                               input.OrgGroup ?? ""
                            , input.HasApprover ?? ""
                            , input.ModifiedDateFrom ?? ""
                            , input.ModifiedDateTo ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<bool> Put(List<MRFDefinedApprover> toAdd, List<MRFDefinedApprover> toUpdate, List<MRFDefinedApprover> toDelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.MRFDefinedApprover.AddRange(toAdd);
                toUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                await _dbContext.SaveChangesAsync();

                toDelete.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<TableVarApproverSetupGet>> GetByID(int ID)
        {
            return await _dbContext.TableVarApproverSetupGet
               .FromSqlRaw(@"CALL sp_approver_setup_get({0})", ID)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<MRFDefinedApprover>> GetMRFDefinedApproverByOrgGroupID(int OrgGroupID)
        {
            return await _dbContext.MRFDefinedApprover.AsNoTracking()
                .Where(x => x.RequestingOrgGroupID == OrgGroupID & x.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<MRFDefinedApprover>> GetSetupMRFApproverInsert()
        {
            return await _dbContext.MRFDefinedApprover
               .FromSqlRaw(@"CALL sp_setup_mrf_approver_insert()")
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<MRFDefinedApprover>> GetSetupMRFApproverUpdate()
        {
            return await _dbContext.MRFDefinedApprover
               .FromSqlRaw(@"CALL sp_setup_mrf_approver_update()")
               .AsNoTracking()
               .ToListAsync();
        }
    }
}