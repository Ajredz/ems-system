using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.MRF;
using EMS.Manpower.Data.Workflow;
using EMS.Manpower.Transfer;
using EMS.Manpower.Transfer.MRF;
using EMS.Manpower.Transfer.MRFApproval;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.MRF
{
    public interface IMRFDBAccess
    {
        Task<IEnumerable<TableVarMRF>> GetList(GetListInput input, int userID, int rowStart);
        
        Task<IEnumerable<TableVarMRF>> GetApprovalList(GetApprovalListInput input, int rowStart);

        Task<MRF> GetByID(int ID);

        Task<IEnumerable<TableVarMRFFormSignatories>> GetMRFSignatoriesStatus(int ID);

        Task<TableVarMRFTransID> GetNewTransactionID();

        Task<bool> Post(MRF param);

        Task<bool> Put(MRF param);

        Task<bool> AddMRFApprovalHistory(int ApproverID, ApproverResponse param);

        Task<IEnumerable<MRFApplicant>> GetApplicantByMRFIDAndStatus(int MRFID, bool IsActive);

        Task<MRFApplicant> GetApplicantByMRFIDAndID(int MRFID, int ApplicantID);

        Task<bool> UpdateStatusApplicant(List<MRFApplicant> applicants);

        Task<List<MRFApplicant>> AddApplicant(List<MRFApplicant> applicants);

        Task<IEnumerable<MRF>> GetByOrgGroupPositionStatus(int OrgGroupID, int PositionID, string Status);

        Task<bool> UpdateApplicant(MRFApplicant param);

        Task<IEnumerable<TableVarMRFApprovalHistory>> GetApprovalHistory(MRFApprovalHistoryForm param);

        Task<bool> PostComments(MRFComments param);
        
        Task<bool> PostApplicantComments(MRFApplicantComments param);

        Task<IEnumerable<MRFComments>> GetComments(int MRFID);

        Task<IEnumerable<MRFApplicantComments>> GetApplicantComments(int MRFID);

        Task<List<MRFApplicant>> GetApplicantByApplicantID(int ApplicantID);

        Task<IEnumerable<TableVarMRFExistingApplicant>> GetMRFExistingApplicantList(GetMRFExistingApplicantListInput input, int rowStart);

        Task<MRFApplicant> GetMRFApplicantByID(long ID);

        Task<List<MRFApplicant>> GetMRFIDDropdownByApplicantID(int ApplicantID);

        Task<MRF> GetByMRFTransactionID(string MRFTransactionID);

        Task<List<MRF>> GetAll();

        //API FOR GET ONLINE MRF
        Task<IEnumerable<TableVarMRFOnline>> GetListMrfOnline();
        Task<MRFApplicant> GetLastApplicant(string FirstName, string LastName);
        Task<List<KickoutQuestion>> GetKickoutQuestion();
        Task<bool> PostApplicantKickoutQuestion(List<ApplicantKickoutQuestion> param);
        Task<bool> PostKickoutQuestion(KickoutQuestion param);
        Task<KickoutQuestion> GetKickoutQuestionByID(int ID);
        Task<List<KickoutQuestion>> GetKickoutQuestionByIDs(List<int> IDs);
        Task<bool> EditKickoutQuestion(KickoutQuestion param);
        Task<bool> AddKickoutQuestionToMRF(MRFKickoutQuestion param);
        Task<List<MRFKickoutQuestion>> GetMRFKickoutQuestionByMRFID(int ID);
        Task<MRFKickoutQuestion> GetMRFKickoutQuestionByID(int ID);
        Task<List<MRFKickoutQuestion>> GetMRFKickoutQuestionByIDs(List<int> IDs);
        Task<bool> EditKickoutQuestionToMRF(MRFKickoutQuestion param);
        Task<bool> EditKickoutQuestionToMRFs(List<MRFKickoutQuestion> param);
        Task<List<KickoutQuestion>> GetKickoutQuestionAutoComplete(GetByKickoutQuestionAutoCompleteInput param);
        Task<IEnumerable<MRF>> GetMRFAutoCancelled();
        Task<IEnumerable<MRF>> GetMRFAutoCancelledReminder();
        Task<bool> AddMRFStatusHistory(MRFStatusHistory param);
    }

    public class MRFDBAccess : IMRFDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public MRFDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarMRF>> GetList(GetListInput input, int userID, int rowStart)
        {   
            return await _dbContext.TableVarMRF
               .FromSqlRaw(@"CALL sp_mrf_get_list(
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
                            , {26}
                        )", userID
                            , input.ID ?? 0
                            , input.MRFTransactionID ?? ""
                            , input.OrgGroupDelimited ?? ""
                            , input.ScopeOrgType ?? ""
                            , input.ScopeOrgGroupDelimited ?? ""
                            , input.PositionLevelDelimited ?? ""
                            , input.PositionDelimited ?? ""
                            , input.NatureOfEmploymentDelimited ?? ""
                            , input.NoOfApplicantMin ?? 0
                            , input.NoOfApplicantMax ?? 0
                            , input.StatusDelimited ?? ""
                            , input.DateCreatedFrom ?? ""
                            , input.DateCreatedTo ?? ""
                            , input.DateApprovedFrom ?? ""
                            , input.DateApprovedTo ?? ""
                            , input.DateHiredFrom ?? ""
                            , input.DateHiredTo ?? ""
                            , input.AgeMin ?? 0
                            , input.AgeMax ?? 0
                            , input.IsAdmin
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                            , input.OrgDescendant ?? "")
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarMRF>> GetApprovalList(GetApprovalListInput input, int rowStart)
        {
            return await _dbContext.TableVarMRF
               .FromSqlRaw(@"CALL sp_mrf_approval_get_list(
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
                        )", input.ApproverPositionID
                            , input.ApproverOrgGroupID
                            , input.RovingPositionDelimited ?? "0"
                            , input.RovingOrgGroupDelimited ?? "0"
                            , input.ApproverID
                            , input.ID ?? 0
                            , input.MRFTransactionID ?? ""
                            , input.OrgGroupDelimited ?? ""
                            , input.PositionLevelDelimited ?? ""
                            , input.PositionDelimited ?? ""
                            , input.NatureOfEmploymentDelimited ?? ""
                            , input.NoOfApplicantMin ?? 0
                            , input.NoOfApplicantMax ?? 0
                            , input.StatusDelimited ?? ""
                            , input.DateCreatedFrom ?? ""
                            , input.DateCreatedTo ?? ""
                            , input.DateApprovedFrom ?? ""
                            , input.DateApprovedTo ?? ""
                            , input.AgeMin ?? 0
                            , input.AgeMax ?? 0
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<MRF> GetByID(int ID)
        {
            return await _dbContext.MRF.FindAsync(ID);
        }

        public async Task<IEnumerable<TableVarMRFFormSignatories>> GetMRFSignatoriesStatus(int ID)
        { 
            return await _dbContext.TableVarMRFFormSignatories
                .FromSqlRaw("CALL sp_mrf_get_signatories({0})", ID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TableVarMRFTransID> GetNewTransactionID()
        {
            return (await _dbContext.TableVarMRFTransID
                    .FromSqlRaw("CALL sp_mrf_get_new_trans_id()")
                    .AsNoTracking()
                    .ToListAsync()).First();
        }
      
        public async Task<bool> Post(MRF param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.MRF.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(MRF param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> AddMRFApprovalHistory(int ApproverID, ApproverResponse param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.WorkflowStepApprover
               .FromSqlRaw("CALL sp_mrf_approval_history_add({0},{1},{2},{3})"
               , ApproverID
               , param.RecordID
               , param.Result.ToString()
               , param.Remarks
               )
               .AsNoTracking().ToListAsync();

                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<MRFApplicant>> GetApplicantByMRFIDAndStatus(int MRFID, bool IsActive)
        {
            return await _dbContext.MRFApplicant.AsNoTracking().Where(x => x.MRFID == MRFID & x.IsActive == IsActive).ToListAsync();
        }

        public async Task<MRFApplicant> GetApplicantByMRFIDAndID(int MRFID, int ApplicantID)
        {
            return await _dbContext.MRFApplicant.AsNoTracking()
                .Where(x => x.MRFID == MRFID & x.ApplicantID == ApplicantID).FirstOrDefaultAsync();
        }

        public async Task<List<MRFApplicant>> GetApplicantByApplicantID(int ApplicantID)
        {
            return await _dbContext.MRFApplicant.AsNoTracking()
                .Where(x => x.ApplicantID == ApplicantID).ToListAsync();
        }

        public async Task<bool> UpdateStatusApplicant(List<MRFApplicant> applicants)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                applicants.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<List<MRFApplicant>> AddApplicant(List<MRFApplicant> applicants) 
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.MRFApplicant.AddRangeAsync(applicants);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return applicants;
        }

        public async Task<bool> UpdateApplicant(MRFApplicant param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<MRF>> GetByOrgGroupPositionStatus(int OrgGroupID, int PositionID, string Status)
        {
            return await _dbContext.MRF.AsNoTracking()
                .Where(x => x.OrgGroupID == OrgGroupID & x.PositionID == PositionID & (x.Status.Equals(Status) || x.Status.Equals(Enums.MRF_STATUS.FOR_APPROVAL.ToString())) & x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<TableVarMRFApprovalHistory>> GetApprovalHistory(MRFApprovalHistoryForm param)
        {
            return await _dbContext.TableVarMRFApprovalHistory
                    .FromSqlRaw(@"CALL sp_mrf_get_approval_history({0},{1},{2},{3})"
                        , param.RequestingPositionID
                        , param.RequestingOrgGroupID
                        , param.PositionID
                        , param.MRFID
                    )
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<bool> PostComments(MRFComments param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.MRFComments.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<MRFComments>> GetComments(int MRFID)
        {
            return await _dbContext.MRFComments.AsNoTracking().Where(x => x.MRFID == MRFID).ToListAsync();
        }
        
        public async Task<bool> PostApplicantComments(MRFApplicantComments param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.MRFApplicantComments.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<MRFApplicantComments>> GetApplicantComments(int MRFID)
        {
            return await _dbContext.MRFApplicantComments.AsNoTracking().Where(x => x.MRFID == MRFID).ToListAsync();
        }

        public async Task<IEnumerable<TableVarMRFExistingApplicant>> GetMRFExistingApplicantList(GetMRFExistingApplicantListInput input, int rowStart)
        {
            return await _dbContext.TableVarMRFExistingApplicant
               .FromSqlRaw(@"CALL sp_mrf_existing_applicant_list(
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
                        )", input.MRFID
                            , input.IDDelimited ?? "0"
                            , input.ID ?? 0
                            , input.ForHiringID
                            , input.ApplicantName ?? ""
                            , input.CurrentStepDelimited ?? ""
                            , input.StatusDelimited ?? ""
                            , input.DateScheduledFrom ?? ""
                            , input.DateScheduledTo ?? ""
                            , input.DateCompletedFrom ?? ""
                            , input.DateCompletedTo ?? ""
                            , input.ApproverRemarks ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<MRFApplicant> GetMRFApplicantByID(long ID)
        {
            return await _dbContext.MRFApplicant.Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<List<MRFApplicant>> GetMRFIDDropdownByApplicantID(int ApplicantID)
        {
            return await _dbContext.MRFApplicant
               .FromSqlRaw(@"CALL sp_mrf_id_get_dropdown({0})", ApplicantID)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<MRF> GetByMRFTransactionID(string MRFTransactionID)
        {
            return await _dbContext.MRF.AsNoTracking()
                .Where(x => x.MRFTransactionID.Equals(MRFTransactionID) && x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<List<MRF>> GetAll()
        {
            return await _dbContext.MRF.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }


        //API FOR GET ONLINE MRF
        public async Task<IEnumerable<TableVarMRFOnline>> GetListMrfOnline()
        {
            return await _dbContext.TableVarMRFOnline
               .FromSqlRaw(@"CALL sp_mrf_get_list_online")
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<MRFApplicant> GetLastApplicant(string FirstName, string LastName)
        {
            return await _dbContext.MRFApplicant.AsNoTracking()
                .Where(x => x.FirstName.Equals(FirstName) && x.LastName.Equals(LastName)).OrderByDescending(x=>x.ID).FirstOrDefaultAsync();
        }

        public async Task<List<KickoutQuestion>> GetKickoutQuestion()
        {
            return await _dbContext.KickoutQuestion.AsNoTracking().Where(x=>x.IsActive).ToListAsync();
        }
        public async Task<bool> PostApplicantKickoutQuestion(List<ApplicantKickoutQuestion> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ApplicantKickoutQuestion.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PostKickoutQuestion(KickoutQuestion param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.KickoutQuestion.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<KickoutQuestion> GetKickoutQuestionByID(int ID)
        {
            return await _dbContext.KickoutQuestion.FindAsync(ID);
        }
        public async Task<List<KickoutQuestion>> GetKickoutQuestionByIDs(List<int> IDs)
        {
            return await _dbContext.KickoutQuestion.AsNoTracking().Where(x => IDs.Contains(x.ID) && x.IsActive).ToListAsync();
        }
        public async Task<bool> EditKickoutQuestion(KickoutQuestion param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.KickoutQuestion.Update(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> AddKickoutQuestionToMRF(MRFKickoutQuestion param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.MRFKickoutQuestion.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<List<MRFKickoutQuestion>> GetMRFKickoutQuestionByMRFID(int ID)
        {
            return await _dbContext.MRFKickoutQuestion.AsNoTracking().Where(x => x.MRFID.Equals(ID) && x.IsActive).ToListAsync();
        }
        public async Task<MRFKickoutQuestion> GetMRFKickoutQuestionByID(int ID)
        {
            return await _dbContext.MRFKickoutQuestion.FindAsync(ID);
        }
        public async Task<List<MRFKickoutQuestion>> GetMRFKickoutQuestionByIDs(List<int> IDs)
        {
            return await _dbContext.MRFKickoutQuestion.AsNoTracking().Where(x => IDs.Contains(x.ID) && x.IsActive).ToListAsync();
        }
        public async Task<bool> EditKickoutQuestionToMRF(MRFKickoutQuestion param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.MRFKickoutQuestion.Update(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> EditKickoutQuestionToMRFs(List<MRFKickoutQuestion> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.MRFKickoutQuestion.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<List<KickoutQuestion>> GetKickoutQuestionAutoComplete(GetByKickoutQuestionAutoCompleteInput param)
        {
            return await _dbContext.KickoutQuestion.AsNoTracking().Where(x => x.IsActive && (string.IsNullOrEmpty(param.Term) || x.Code.ToUpper().Contains(param.Term.Trim().ToUpper()))).Take(param.TopResults).ToListAsync();
        }
        public async Task<IEnumerable<MRF>> GetMRFAutoCancelled()
        {
            return await _dbContext.MRF
               .FromSqlRaw(@"CALL sp_get_mrf_auto_cancelled")
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<MRF>> GetMRFAutoCancelledReminder()
        {
            return await _dbContext.MRF
               .FromSqlRaw(@"CALL sp_get_mrf_auto_cancelled_reminder")
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<bool> AddMRFStatusHistory(MRFStatusHistory param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.MRFStatusHistory.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}