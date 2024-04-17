using EMS.Manpower.Data.Applicant;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.Applicant
{
    public interface IApplicantDBAccess
    {
        Task<IEnumerable<TableVarApplicant>> GetList(GetListInput input, int rowStart);
        
        Task<IEnumerable<TableVarApplicant>> GetApplicantPickerList(GetApplicantPickerListInput input, int rowStart);

        Task<IEnumerable<TableVarApplicationApproval>> GetApprovalList(GetListInput input, int rowStart);

        Task<bool> Post(Applicant param, List<ApplicantAttachment> attachments);

        Task<Applicant> GetByID(int ID);

        Task<IEnumerable<TableVarApplicantName>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<ApplicantAttachment>> GetAttachmentsByApplicantID(int ID);

        Task<IEnumerable<TableVarApplicantHistory>> GetHistory(int ID);

        Task<bool> Delete(Applicant applicant);

        Task<bool> Put(Applicant applicant,
            List<ApplicantAttachment> toAddAttachment,
            List<ApplicantAttachment> toDeleteAttachment,
            List<ApplicantAttachment> toUpdateAttachment
            );

        Task<bool> AddWorkflowTransaction(int ApproverID, ApproverResponse param);

        Task<bool> UpdateApplicants(List<Applicant> Applicants);

        Task<IEnumerable<Applicant>> GetByIDs(List<int> IDs);

        Task<bool> PostAttachment(List<ApplicantAttachment> addAttachment,
            List<ApplicantAttachment> updateAttachment,
            List<ApplicantAttachment> deleteAttachment);

        Task<bool> UploadInsert(List<Applicant> param);

        Task<IEnumerable<Applicant>> GetByUnique(string LastName, string FirstName, string MiddleName, DateTime BirthDate);

        Task<bool> Put(Applicant param);
        Task<Applicant> GetLastApplicant(string FirstName, string LastName, string Birthday, string Email);
        Task<IEnumerable<TableVarApplicantLegalProfile>> GetApplicantLegalProfile(int ApplicantId);
        Task<bool> PostApplicantLegalProfile(List<ApplicantLegalProfile> param);
        Task<bool> PostApplicantHistory(ApplicantHistory param);
    }

    public class ApplicantDBAccess : IApplicantDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public ApplicantDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarApplicant>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarApplicant
               .FromSqlRaw(@"CALL sp_applicant_get_list(
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
                        )", input.ID ?? 0
                            , input.LastName ?? ""
                            , input.FirstName ?? ""
                            , input.MiddleName ?? ""
                            , input.Suffix ?? ""
                            , input.ApplicationSourceDelimited ?? ""
                            , input.MRFTransactionID ?? ""
                            , input.CurrentStepDelimited ?? ""
                            , input.DateScheduledFrom ?? ""
                            , input.DateScheduledTo ?? ""
                            , input.DateCompletedFrom ?? ""
                            , input.DateCompletedTo ?? ""
                            , input.ApproverRemarks ?? ""
                            //, input.CurrentStepDelimited ?? ""
                            //, input.WorkflowDelimited ?? ""
                            , input.PositionRemarks ?? ""
                            , input.Course ?? ""
                            , input.CurrentPositionTitle ?? ""
                            , input.ExpectedSalaryFrom ?? 0
                            , input.ExpectedSalaryTo ?? 0
                            , input.DateAppliedFrom ?? ""
                            , input.DateAppliedTo ?? ""
                            , input.ScopeOrgType ?? ""
                            , input.ScopeOrgGroupDelimited ?? ""
                            //, input.WorkflowStatusDelimited ?? ""
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarApplicant>> GetApplicantPickerList(GetApplicantPickerListInput input, int rowStart)
        {
            return await _dbContext.TableVarApplicant
               .FromSqlRaw(@"CALL sp_applicant_get_picker_list(
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
                            , {27}
                            , {28}
                        )"
                            , input.SelectedIDDelimited ?? "0"
                            , input.IsHired
                            , input.IsTaggedToMRF
                            , input.ID ?? 0
                            , input.LastName ?? ""
                            , input.FirstName ?? ""
                            , input.MiddleName ?? ""
                            , input.Suffix ?? ""
                            , input.ApplicationSourceDelimited ?? ""
                            , input.MRFTransactionID ?? ""
                            , input.CurrentStepDelimited ?? ""
                            , input.DateScheduledFrom ?? ""
                            , input.DateScheduledTo ?? ""
                            , input.DateCompletedFrom ?? ""
                            , input.DateCompletedTo ?? ""
                            , input.ApproverRemarks ?? ""
                            //, input.CurrentStepDelimited ?? ""
                            //, input.WorkflowDelimited ?? ""
                            , input.PositionRemarks ?? ""
                            , input.Course ?? ""
                            , input.CurrentPositionTitle ?? ""
                            , input.ExpectedSalaryFrom ?? 0
                            , input.ExpectedSalaryTo ?? 0
                            , input.DateAppliedFrom ?? ""
                            , input.DateAppliedTo ?? ""
                            , input.ScopeOrgType ?? ""
                            , input.ScopeOrgGroupDelimited ?? ""
                            //, input.WorkflowStatusDelimited ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarApplicationApproval>> GetApprovalList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarApplicationApproval
               .FromSqlRaw(@"CALL sp_application_approval_get_list(
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
                            //, {14}
                        )"  , input.RoleDelimited
                            , input.ID ?? 0
                            //, input.ApplicantName ?? ""
                            , input.ApplicationSourceDelimited ?? ""
                            //, input.CurrentStepDelimited ?? ""
                            //, input.WorkflowDelimited ?? ""
                            , input.PositionRemarks ?? ""
                            , input.Course ?? ""
                            , input.CurrentPositionTitle ?? ""
                            , input.ExpectedSalaryFrom ?? 0
                            , input.ExpectedSalaryTo ?? 0
                            , input.DateAppliedFrom ?? ""
                            , input.DateAppliedTo ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<bool> Post(Applicant param, List<ApplicantAttachment> attachments)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Applicant.AddAsync(param);
                // Save Applicant record to get the Auto increment ID
                await _dbContext.SaveChangesAsync();
                await _dbContext.ApplicantAttachment.AddRangeAsync(
                    attachments.Select(x =>
                    {
                        x.ApplicantID = param.ID;
                        return x;
                    }).ToList());
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<Applicant> GetByID(int ID)
        {
            return await _dbContext.Applicant.AsNoTracking().Where(x=> x.ID == ID).FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<Applicant>> GetByIDs(List<int> IDs)
        {
            return await _dbContext.Applicant.AsNoTracking().Where(x=> IDs.Contains(x.ID)).ToListAsync();
        }

        public async Task<IEnumerable<ApplicantAttachment>> GetAttachmentsByApplicantID(int ID)
        {
            return await _dbContext.ApplicantAttachment.AsNoTracking()
                .Where(x => x.ApplicantID == ID).ToListAsync();
        }

        public async Task<IEnumerable<TableVarApplicantName>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.TableVarApplicantName
                .FromSqlRaw("CALL sp_applicant_name_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarApplicantHistory>> GetHistory(int ID)
        {
            return await _dbContext.TableVarApplicantHistory
               .FromSqlRaw(@"CALL sp_applicant_get_history({0})", ID)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<bool> Delete(Applicant applicant)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(applicant).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(Applicant applicant, 
            List<ApplicantAttachment> toAddAttachment,
            List<ApplicantAttachment> toDeleteAttachment,
            List<ApplicantAttachment> toUpdateAttachment
            )
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(applicant).State = EntityState.Modified;

                // Execute filtered records to their respective actions.
                _dbContext.ApplicantAttachment.RemoveRange(toDeleteAttachment);
                _dbContext.ApplicantAttachment.AddRange(toAddAttachment);
                toUpdateAttachment.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();


                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> AddWorkflowTransaction(int ApproverID, ApproverResponse param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.WorkflowStepApprover
               .FromSqlRaw("CALL sp_application_approval_add_transaction({0},{1},{2},{3})"
               , ApproverID
               , param.RecordID
               , param.Result.ToString()
               , param.Remarks)
               .AsNoTracking().ToListAsync();

                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> UpdateApplicants(List<Applicant> Applicants)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                Applicants.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> PostAttachment(List<ApplicantAttachment> addAttachment,
            List<ApplicantAttachment> updateAttachment,
            List<ApplicantAttachment> deleteAttachment)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (deleteAttachment?.Count > 0)
                {
                    _dbContext.ApplicantAttachment.RemoveRange(deleteAttachment);
                }
                await _dbContext.ApplicantAttachment.AddRangeAsync(addAttachment);
                updateAttachment.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        
        public async Task<bool> UploadInsert(List<Applicant> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Applicant.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<Applicant>> GetByUnique(string LastName, string FirstName, string MiddleName, DateTime BirthDate)
        {
            return await _dbContext.Applicant.AsNoTracking().Where(x => 
                x.LastName.Equals(LastName, StringComparison.OrdinalIgnoreCase) &
                x.FirstName.Equals(FirstName, StringComparison.OrdinalIgnoreCase) &
                x.MiddleName.Equals(MiddleName, StringComparison.OrdinalIgnoreCase) &
                x.BirthDate == BirthDate
            ).ToListAsync();
        }

        public async Task<bool> Put(Applicant param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }


        public async Task<Applicant> GetLastApplicant(string FirstName, string LastName, string Birthday, string Email)
        {
                return await _dbContext.Applicant.AsNoTracking()
                .Where(x => x.FirstName.Equals(FirstName) 
                && x.LastName.Equals(LastName)
                && x.BirthDate.Equals(Convert.ToDateTime(Birthday))
                && x.Email.Equals(Email))
                .OrderByDescending(x=>x.ID).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TableVarApplicantLegalProfile>> GetApplicantLegalProfile(int ApplicantId)
        {
            return await _dbContext.TableVarApplicantLegalProfile
               .FromSqlRaw(@"CALL sp_applicant_legal_profile({0})", ApplicantId)
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<bool> PostApplicantLegalProfile(List<ApplicantLegalProfile> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ApplicantLegalProfile.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PostApplicantHistory(ApplicantHistory param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ApplicantHistory.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

    }
}