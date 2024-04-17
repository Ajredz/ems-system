using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.RecruiterTask;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.RecruiterTask
{
    public interface IRecruiterTaskDBAccess
    {
        Task<IEnumerable<TableVarRecruiterTask>> GetList(GetListInput input, int userID, int rowStart);
        Task<IEnumerable<TableVarPendingTask>> GetPendingList(GetPendingListInput input, int recruiterID, int rowStart);
        Task<RecruiterTask> GetByID(int ID);
        Task<IEnumerable<RecruiterTask>> GetByIDs(List<int> Ids);
        Task<IEnumerable<RecruiterTask>> GetByDetails(int RecruiterID, int ApplicantID, string Description);
        Task<bool> Post(RecruiterTask param);
        Task<bool> Delete(int ID);
        Task<bool> Put(RecruiterTask param);
        Task<bool> BatchUpdate(List<RecruiterTask> param);
    }

    public class RecruiterTaskDBAccess :  IRecruiterTaskDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public RecruiterTaskDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarRecruiterTask>> GetList(GetListInput input, int userID, int rowStart)
        {
            return await _dbContext.TableVarRecruiterTask
                .FromSqlRaw(@"CALL sp_recruiter_task_get_list(
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
                    )", userID
                      , input.ID ?? 0
                      , input.Recruiter ?? ""
                      , input.Applicant ?? ""
                      , input.Description ?? ""
                      , input.StatusDelimited ?? ""
                      , input.DateCreatedFrom ?? ""
                      , input.DateCreatedTo ?? ""
                      , input.DateModifiedFrom ?? ""
                      , input.DateModifiedTo ?? ""
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarPendingTask>> GetPendingList(GetPendingListInput input, int recruiterID, int rowStart)
        {
            return await _dbContext.TableVarPendingTask
                .FromSqlRaw(@"CALL sp_pending_task_get_list(
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
                    )", recruiterID
                      , input.ID ?? 0
                      , input.Applicant ?? ""
                      , input.Description ?? ""
                      , input.StatusDelimited ?? ""
                      , input.DateCreatedFrom ?? ""
                      , input.DateCreatedTo ?? ""
                      , input.DateModifiedFrom ?? ""
                      , input.DateModifiedTo ?? ""
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RecruiterTask> GetByID(int ID)
        {
            return await _dbContext.RecruiterTask.FindAsync(ID);
        }

        public async Task<IEnumerable<RecruiterTask>> GetByIDs(List<int> Ids)
        {
            return await _dbContext.RecruiterTask.AsNoTracking()
                .Where(x => Ids.Contains(x.ID))
                .ToListAsync();
        }

        public async Task<IEnumerable<RecruiterTask>> GetByDetails(int RecruiterID, int ApplicantID, string Description)
        {
            return await _dbContext.RecruiterTask.AsNoTracking()
                .Where(x => x.RecruiterUserId == RecruiterID && x.ApplicantId == ApplicantID 
                        && x.Description == Description)
                .ToListAsync();
        }

        public async Task<bool> Post(RecruiterTask param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.RecruiterTask.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Delete(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.RecruiterTask.Remove(new RecruiterTask() { ID = ID });

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(RecruiterTask param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> BatchUpdate(List<RecruiterTask> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                param.Select(x => 
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}
