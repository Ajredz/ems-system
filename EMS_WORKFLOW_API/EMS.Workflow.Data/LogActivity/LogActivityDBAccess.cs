using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Reference;
using EMS.Workflow.Transfer.LogActivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EMS.Workflow.Data.LogActivity
{
    public interface ILogActivityDBAccess
    {
        Task<IEnumerable<TableVarApplicantLogActivity>> GetApplicantLogActivityByApplicantID(int ApplicantID);

        Task<IEnumerable<TableVarEmployeeLogActivity>> GetEmployeeLogActivityByEmployeeID(int EmployeeID);

        Task<IEnumerable<TableVarLogActivity>> GetList(GetListInput input, int rowStart);

        Task<LogActivity> GetByID(int ID);

        Task<IEnumerable<LogActivity>> GetActivityByModuleTypeTitle(string Module, string Type, string SubType, string Title);

        Task<bool> Post(LogActivity logActivity);

        Task<bool> Put(LogActivity logActivity);

        Task<IEnumerable<TableVarApplicantLogActivityStatusHistory>> GetApplicantLogActivityStatusHistory(int ApplicantLogActivityID);
        
        Task<IEnumerable<TableVarEmployeeLogActivityStatusHistory>> GetEmployeeLogActivityStatusHistory(int EmployeeLogActivityID);

        Task<bool> AddApplicantActivity(ApplicantLogActivity logActivity, ApplicantLogActivityStatusHistory logActivityHistory);
        
        Task<bool> AddEmployeeActivity(EmployeeLogActivity logActivity, EmployeeLogActivityStatusHistory logActivityHistory);

        Task<IEnumerable<LogActivity>> GetLogActivityByModuleAndType(GetLogActivityByModuleTypeInput param);

        Task<LogActivity> GetLogActivityByID(int ID);

        Task<ApplicantLogActivity> GetApplicantLogActivityByID(int ID);
        
        Task<EmployeeLogActivity> GetEmployeeLogActivityByID(int ID);

        Task<bool> AddApplicantActivityStatusHistory(ApplicantLogActivity applicantLogActivity, ApplicantLogActivityStatusHistory logActivityHistory);
        
        Task<bool> AddEmployeeActivityStatusHistory(EmployeeLogActivity EmployeeLogActivity, EmployeeLogActivityStatusHistory logActivityHistory);

        Task<IEnumerable<TableVarAssignedActivities>> GetAssignedActivitiesList(GetAssignedActivitiesListInput input, int rowStart, int UserID);

        Task<bool> PostApplicantComments(ApplicantLogActivityComments param);
        
        Task<bool> PostEmployeeComments(EmployeeLogActivityComments param);

        Task<IEnumerable<ApplicantLogActivityComments>> GetApplicantComments(int ApplicantLogActivityID);
        
        Task<IEnumerable<EmployeeLogActivityComments>> GetEmployeeComments(int EmployeeLogActivityID);

        Task<bool> PostApplicantAttachment(List<ApplicantLogActivityAttachment> addAttachment, List<ApplicantLogActivityAttachment> updateAttachment, List<ApplicantLogActivityAttachment> deleteAttachment);
        
        Task<bool> PostEmployeeAttachment(List<EmployeeLogActivityAttachment> addAttachment, List<EmployeeLogActivityAttachment> updateAttachment, List<EmployeeLogActivityAttachment> deleteAttachment);

        Task<IEnumerable<ApplicantLogActivityAttachment>> GetApplicantAttachment(int ApplicantLogActivityID);
        
        Task<IEnumerable<EmployeeLogActivityAttachment>> GetEmployeeAttachment(int EmployeeLogActivityID);

        Task<ApplicantLogActivityAttachment> GetApplicantAttachmentByServerFile(string ServerFile);
        
        Task<EmployeeLogActivityAttachment> GetEmployeeAttachmentByServerFile(string ServerFile);

        Task<IEnumerable<LogActivityPreloaded>> GetAllLogActivityPreloaded();

        Task<IEnumerable<LogActivity>> GetLogActivityByPreloadedID(int LogActivityPreloadedID);

        Task<IEnumerable<ApplicantLogActivityStatusHistory>> AddApplicantPreLoadedActivities(AddApplicantPreLoadedActivitiesInput param, int UserID);

        Task<IEnumerable<EmployeeLogActivityStatusHistory>> AddEmployeePreLoadedActivities(AddEmployeePreLoadedActivitiesInput param, int UserID);

        Task<IEnumerable<ReferenceValue>> GetLogActivitySubType();

        Task<IEnumerable<TableVarLogActivityPreLoaded>> GetLogActivityPreloadedList(GetPreLoadedListInput input, int rowStart);
        
        Task<IEnumerable<LogActivityPreloaded>> GetByPreloadedName(string name);

        Task<LogActivityPreloaded> GetLogActivityPreloadedByID(int ID);

        Task<IEnumerable<LogActivity>> GetPreloadedItemsByID(int ID);

        Task<bool> AddLogActivityPreloaded(LogActivityPreloaded preloaded, List<LogActivity> lstLogActivity);

        Task<bool> EditLogActivityPreloaded(LogActivityPreloaded preloaded, List<LogActivity> toDeleteLogActivity,
                                            List<LogActivity> toAddLogActivity, List<LogActivity> toUpdateLogActivity);

        Task<IEnumerable<ApplicantLogActivity>> GetApplicantLogActivityPendingEmail();

        Task<IEnumerable<EmployeeLogActivity>> GetEmployeeLogActivityPendingEmail();

        Task<bool> UpdateApplicantLogActivityPendingEmail(ApplicantLogActivity param);
        
        Task<bool> UpdateEmployeeLogActivityPendingEmail(EmployeeLogActivity param);

        Task<IEnumerable<TableVarChecklist>> GetChecklistList(GetChecklistListInput input, int rowStart);

        Task<IEnumerable<EmployeeLogActivity>> GetEmployeeLogActivityByIDs(List<int> IDs);

        Task<bool> UpdateEmployeeLogActivityAssignedUser(UpdateEmployeeLogActivityAssignedUserForm param);

        Task<bool> UpdateApplicantLogActivityAssignedUser(UpdateApplicantLogActivityAssignedUserForm param);

        Task<IEnumerable<TableVarApplicantLogActivityList>> GetApplicantLogActivityList(GetApplicantLogActivityListInput input, int rowStart);

        Task<IEnumerable<TableVarChecklist>> GetEmployeeLogActivityList(GetChecklistListInput input, int rowStart);

        Task<IEnumerable<ApplicantLogActivity>> GetApplicantLogActivityByIDs(List<int> IDs);

        Task<bool> BatchUpdateApplicantLogActivity(List<ApplicantLogActivity> applicantLogActivityList, List<ApplicantLogActivityStatusHistory> applicantLogActivityHistoryList);

        Task<bool> BatchUpdateEmployeeLogActivity(List<EmployeeLogActivity> employeeLogActivityList, List<EmployeeLogActivityStatusHistory> employeeLogActivityHistoryList);

        Task<IEnumerable<EmployeeLogActivity>> GetByUniqueLogActivity(int EmployeeID, string Type, string Title, int? OrgGroupID);
        Task<bool> UploadLogActivityInsert(List<UploadLogActivityFile> param);
    }

    public class LogActivityDBAccess : ILogActivityDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public LogActivityDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarApplicantLogActivity>> GetApplicantLogActivityByApplicantID(int ApplicantID)
        {
            return await _dbContext.TableVarApplicantLogActivity
                .FromSqlRaw("CALL sp_applicant_log_activity_get({0})", ApplicantID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeLogActivity>> GetEmployeeLogActivityByEmployeeID(int EmployeeID)
        {
            return await _dbContext.TableVarEmployeeLogActivity
                .FromSqlRaw("CALL sp_employee_log_activity_get({0})", EmployeeID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVarLogActivity>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarLogActivity
                .FromSqlRaw(@"CALL sp_log_activity_get_list(
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
                    )", input.ModuleDelimited ?? ""
                      , input.TypeDelimited ?? ""
                      , input.SubTypeDelimited ?? ""
                      , input.Title ?? ""
                      , input.Description ?? ""
                      , input.IsPassFail ?? ""
                      , input.IsAssignment ?? ""
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LogActivity> GetByID(int ID)
        {
            return await _dbContext.LogActivity.FindAsync(ID);
        }

        public async Task<IEnumerable<LogActivity>> GetActivityByModuleTypeTitle(string Module, string Type, string SubType, string Title)
        {
            return await _dbContext.LogActivity.AsNoTracking()
                .Where(x => x.Module == Module && x.Type == Type && x.SubType == SubType && x.Title == Title)
                .ToListAsync();
        }

        public async Task<bool> Post(LogActivity logActivity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.LogActivity.AddAsync(logActivity);
                await _dbContext.SaveChangesAsync();
				
                transaction.Commit();
            }
            return true;
        }
		
		public async Task<bool> Put(LogActivity logActivity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(logActivity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
		}
		
        public async Task<IEnumerable<TableVarApplicantLogActivityStatusHistory>> GetApplicantLogActivityStatusHistory(int ApplicantLogActivityID)
        {
            return await _dbContext.TableVarApplicantLogActivityStatusHistory
               .FromSqlRaw("CALL sp_applicant_log_activity_status_history_get({0})", ApplicantLogActivityID)
               .AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<TableVarEmployeeLogActivityStatusHistory>> GetEmployeeLogActivityStatusHistory(int EmployeeLogActivityID)
        {
            return await _dbContext.TableVarEmployeeLogActivityStatusHistory
               .FromSqlRaw("CALL sp_employee_log_activity_status_history_get({0})", EmployeeLogActivityID)
               .AsNoTracking().ToListAsync();

        }

        public async Task<bool> AddApplicantActivity(ApplicantLogActivity logActivity, ApplicantLogActivityStatusHistory logActivityHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ApplicantLogActivity.Add(logActivity);
                await _dbContext.SaveChangesAsync();

                logActivityHistory.ApplicantLogActivityID = logActivity.ID;
                _dbContext.ApplicantLogActivityStatusHistory.Add(logActivityHistory);

                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
		
        public async Task<bool> AddEmployeeActivity(EmployeeLogActivity logActivity, EmployeeLogActivityStatusHistory logActivityHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeLogActivity.Add(logActivity);
                await _dbContext.SaveChangesAsync();

                logActivityHistory.EmployeeLogActivityID = logActivity.ID;
                _dbContext.EmployeeLogActivityStatusHistory.Add(logActivityHistory);

                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
		
        public async Task<IEnumerable<LogActivity>> GetLogActivityByModuleAndType(GetLogActivityByModuleTypeInput param)
        {
            List<string> modules = param.Modules.Select(y => y.ToString()).ToList();
            return await _dbContext.LogActivity.AsNoTracking()
                .Where(x => modules.Contains(x.Module))
                .Where(x => x.IsActive & x.Type.Equals(param.Type)).ToListAsync();
        }

        public async Task<LogActivity> GetLogActivityByID(int ID)
        {
            return await _dbContext.LogActivity.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<ApplicantLogActivity> GetApplicantLogActivityByID(int ID)
        {
            return await _dbContext.ApplicantLogActivity.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }
        
        public async Task<EmployeeLogActivity> GetEmployeeLogActivityByID(int ID)
        {
            return await _dbContext.EmployeeLogActivity.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<bool> AddApplicantActivityStatusHistory(ApplicantLogActivity applicantLogActivity, ApplicantLogActivityStatusHistory logActivityHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ApplicantLogActivity.Update(applicantLogActivity);
                if (!string.IsNullOrEmpty(logActivityHistory.Status))
                {
                    _dbContext.ApplicantLogActivityStatusHistory.Add(logActivityHistory); 
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        
        public async Task<bool> AddEmployeeActivityStatusHistory(EmployeeLogActivity EmployeeLogActivity, EmployeeLogActivityStatusHistory logActivityHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeLogActivity.Update(EmployeeLogActivity);
                if (!string.IsNullOrEmpty(logActivityHistory.Status))
                {
                    _dbContext.EmployeeLogActivityStatusHistory.Add(logActivityHistory); 
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarAssignedActivities>> GetAssignedActivitiesList(GetAssignedActivitiesListInput input, int rowStart, int UserID)
        {
            return await _dbContext.TableVarAssignedActivities
                .FromSqlRaw(@"CALL sp_assigned_activities_get_list(
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
                    )"
                    , UserID
                    , input.ID
                    , input.EmployeeDelimited ?? ""
                    , input.ApplicantDelimited ?? ""
                    , input.TypeDelimited ?? ""
                    , input.SubTypeDelimited ?? ""
                      , input.Title ?? ""
                      , input.Description ?? ""
                      , input.CurrentStatusDelimited ?? ""
                      , input.CurrentTimstampFrom ?? ""
                      , input.CurrentTimstampTo ?? ""
                      , input.DueDateFrom ?? ""
                      , input.DueDateTo ?? ""
                      , input.AssignedByDelimited ?? ""
                      , input.OrgGroupDelimited ?? ""
                      , input.Remarks ?? ""
                      , input.IsAdminAccess
                      , input.IsExport
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> PostApplicantComments(ApplicantLogActivityComments param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ApplicantLogActivityComments.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        
        public async Task<bool> PostEmployeeComments(EmployeeLogActivityComments param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeLogActivityComments.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<ApplicantLogActivityComments>> GetApplicantComments(int ApplicantLogActivityID)
        {
            return await _dbContext.ApplicantLogActivityComments.AsNoTracking()
                .Where(x => x.ApplicantLogActivityID == ApplicantLogActivityID).ToListAsync();
        }
        
        public async Task<IEnumerable<EmployeeLogActivityComments>> GetEmployeeComments(int EmployeeLogActivityID)
        {
            return await _dbContext.EmployeeLogActivityComments.AsNoTracking()
                .Where(x => x.EmployeeLogActivityID == EmployeeLogActivityID).ToListAsync();
        }
        
        public async Task<bool> PostApplicantAttachment(List<ApplicantLogActivityAttachment> addAttachment, 
            List<ApplicantLogActivityAttachment> updateAttachment, 
            List<ApplicantLogActivityAttachment> deleteAttachment)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (deleteAttachment?.Count > 0)
                {
                    _dbContext.ApplicantLogActivityAttachment.RemoveRange(deleteAttachment); 
                }
                await _dbContext.ApplicantLogActivityAttachment.AddRangeAsync(addAttachment);
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
        
        public async Task<bool> PostEmployeeAttachment(List<EmployeeLogActivityAttachment> addAttachment, 
            List<EmployeeLogActivityAttachment> updateAttachment, 
            List<EmployeeLogActivityAttachment> deleteAttachment)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (deleteAttachment?.Count > 0)
                {
                    _dbContext.EmployeeLogActivityAttachment.RemoveRange(deleteAttachment); 
                }
                await _dbContext.EmployeeLogActivityAttachment.AddRangeAsync(addAttachment);
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

        public async Task<ApplicantLogActivityAttachment> GetApplicantAttachmentByServerFile(string ServerFile)
        {
            return await _dbContext.ApplicantLogActivityAttachment.AsNoTracking()
                .Where(x => x.ServerFile.Equals(ServerFile)).FirstOrDefaultAsync();
        }
        
        public async Task<EmployeeLogActivityAttachment> GetEmployeeAttachmentByServerFile(string ServerFile)
        {
            return await _dbContext.EmployeeLogActivityAttachment.AsNoTracking()
                .Where(x => x.ServerFile.Equals(ServerFile)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicantLogActivityAttachment>> GetApplicantAttachment(int ApplicantLogActivityID)
        {
            return await _dbContext.ApplicantLogActivityAttachment.AsNoTracking()
                .Where(x => x.ApplicantLogActivityID == ApplicantLogActivityID).ToListAsync();
        }
        
        public async Task<IEnumerable<EmployeeLogActivityAttachment>> GetEmployeeAttachment(int EmployeeLogActivityID)
        {
            return await _dbContext.EmployeeLogActivityAttachment.AsNoTracking()
                .Where(x => x.EmployeeLogActivityID == EmployeeLogActivityID).ToListAsync();
        }

        public async Task<IEnumerable<LogActivityPreloaded>> GetAllLogActivityPreloaded()
        {
            return await _dbContext.LogActivityPreloaded.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<LogActivity>> GetLogActivityByPreloadedID(int LogActivityPreloadedID)
        {
            return await _dbContext.LogActivity
                .FromSqlRaw("CALL sp_log_activity_get_by_log_activity_preloaded_id({0})", LogActivityPreloadedID)
                .AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<ReferenceValue>> GetLogActivitySubType()
        {
            return await _dbContext.ReferenceValue
                .FromSqlRaw("CALL sp_applicant_log_activity_get_used_sub_type()")
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ApplicantLogActivityStatusHistory>> AddApplicantPreLoadedActivities(AddApplicantPreLoadedActivitiesInput param, int UserID)
        {
            return await _dbContext.ApplicantLogActivityStatusHistory
                .FromSqlRaw(@"CALL sp_log_activity_add_preloaded_to_applicant(
                    {0}
                    , {1}
                    , {2}
                    )", string.Join(",", param.LogActivityPreloadedIDs)
                    , param.ApplicantID
                    , UserID)
                .AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<EmployeeLogActivityStatusHistory>> AddEmployeePreLoadedActivities(AddEmployeePreLoadedActivitiesInput param, int UserID)
        {
            return await _dbContext.EmployeeLogActivityStatusHistory
                .FromSqlRaw(@"CALL sp_log_activity_add_preloaded_to_employee(
                    {0}
                    , {1}
                    , {2}
                    )", string.Join(",", param.LogActivityPreloadedIDs)
                    , param.EmployeeID
                    , UserID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVarLogActivityPreLoaded>> GetLogActivityPreloadedList(GetPreLoadedListInput input, int rowStart)
        {
            return await _dbContext.TableVarLogActivityPreLoaded
                .FromSqlRaw(@"CALL sp_log_activity_preloaded_get_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                            , {8}
                    )", input.ID ?? 0
                      , input.PreloadName ?? ""
                      , input.DateCreatedFrom ?? ""
                      , input.DateCreatedTo ?? ""
                      , input.IsExport
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LogActivityPreloaded>> GetByPreloadedName(string name)
        {
            return await _dbContext.LogActivityPreloaded.AsNoTracking()
                .Where(x => x.IsActive && x.PreloadName == name)
                .ToListAsync();
        }

        public async Task<LogActivityPreloaded> GetLogActivityPreloadedByID(int ID)
        {
            return await _dbContext.LogActivityPreloaded.FindAsync(ID);
        }

        public async Task<IEnumerable<LogActivity>> GetPreloadedItemsByID(int ID)
        {
            return await _dbContext.LogActivity.AsNoTracking()
                .Where(x => x.IsActive && x.LogActivityPreloadedID == ID)
                .ToListAsync();
        }

        public async Task<bool> AddLogActivityPreloaded(LogActivityPreloaded preloaded, List<LogActivity> lstLogActivity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.LogActivityPreloaded.AddAsync(preloaded);
                await _dbContext.SaveChangesAsync();

                if (lstLogActivity != null)
                {
                    await _dbContext.LogActivity.AddRangeAsync(lstLogActivity
                        .Select(x => { x.LogActivityPreloadedID = preloaded.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> EditLogActivityPreloaded(LogActivityPreloaded preloaded, List<LogActivity> toDeleteLogActivity,
                                            List<LogActivity> toAddLogActivity, List<LogActivity> toUpdateLogActivity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(preloaded).State = EntityState.Modified;

                // Execute filtered records to their respective actions.
                if (toAddLogActivity != null)
                {
                    _dbContext.LogActivity.AddRange(toAddLogActivity);
                }
                
                if (toUpdateLogActivity != null)
                {
                    toUpdateLogActivity.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }
                
                if (toDeleteLogActivity != null)
                {
                    toDeleteLogActivity.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<ApplicantLogActivity>> GetApplicantLogActivityPendingEmail()
        {
            return await _dbContext.ApplicantLogActivity.AsNoTracking()
                .Where(x => x.IsActive & x.IsEmailSent != true & x.AssignedUserID > 0 & !string.IsNullOrEmpty(x.Email) 
                & !string.IsNullOrEmpty(x.ApplicantName)).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeLogActivity>> GetEmployeeLogActivityPendingEmail()
        {
            return await _dbContext.EmployeeLogActivity.AsNoTracking()
                .Where(x => x.IsActive & x.IsEmailSent != true & x.AssignedUserID > 0 & !string.IsNullOrEmpty(x.Email)
                & !string.IsNullOrEmpty(x.EmployeeName)).ToListAsync();
        }

        public async Task<bool> UpdateApplicantLogActivityPendingEmail(ApplicantLogActivity param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> UpdateEmployeeLogActivityPendingEmail(EmployeeLogActivity param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarChecklist>> GetChecklistList(GetChecklistListInput input, int rowStart)
        {
            return await _dbContext.TableVarChecklist
                .FromSqlRaw(@"CALL sp_checklist_get_list(
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
                    )"
                    , input.EmployeeID
                    , input.TypeDelimited ?? ""
                    , input.SubTypeDelimited ?? ""
                      , input.Title ?? ""
                      , input.Description ?? ""
                      , input.CurrentStatusDelimited ?? ""
                      , input.CurrentTimestampFrom ?? ""
                      , input.CurrentTimestampTo ?? ""
                      , input.DueDateFrom ?? ""
                      , input.DueDateTo ?? ""
                      , input.AssignedByDelimited ?? ""
                      , input.AssignedToDelimited ?? ""
                      , input.Remarks ?? ""
                      , input.IsExport
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeLogActivity>> GetEmployeeLogActivityByIDs(List<int> IDs)
        {
            return await _dbContext.EmployeeLogActivity.AsNoTracking()
                .Where(x => IDs.Contains(x.ID)).Where(x => x.IsActive).ToListAsync();
        }

        public async Task<bool> UpdateEmployeeLogActivityAssignedUser(UpdateEmployeeLogActivityAssignedUserForm param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var UpdateLogActivity = (await _dbContext.EmployeeLogActivity.Where(x => param.LogActivityIDs.Contains(x.ID))
                    .ToListAsync())
                    .Select(x =>
                    {
                        //x.AssignedUserID = param.AssignedUserID;
                        //x.DueDate = param.DueDate;
                        x.ModifiedBy = param.ModifiedBy;
                        x.ModifiedDate = DateTime.Now;
                        return x;
                    });

                if (param.AssignedUserID != 0)
                {
                    UpdateLogActivity
                    .Select(x =>
                    {
                        x.AssignedUserID = param.AssignedUserID;
                        return x;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(param.DueDate.ToString()))
                {
                    UpdateLogActivity
                    .Select(x =>
                    {
                        x.DueDate = param.DueDate;
                        return x;
                    }).ToList();
                }

                UpdateLogActivity
                .Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> UpdateApplicantLogActivityAssignedUser(UpdateApplicantLogActivityAssignedUserForm param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var UpdateLogActivity = (await _dbContext.ApplicantLogActivity.Where(x => param.LogActivityIDs.Contains(x.ID))
                    .ToListAsync())
                    .Select(x =>
                    {
                        //x.AssignedUserID = param.AssignedUserID;
                        x.ModifiedBy = param.ModifiedBy;
                        x.ModifiedDate = DateTime.Now;
                        return x;
                    });

                if (param.AssignedUserID != 0)
                {
                    UpdateLogActivity
                    .Select(x =>
                    {
                        x.AssignedUserID = param.AssignedUserID;
                        return x;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(param.DueDate.ToString()))
                {
                    UpdateLogActivity
                    .Select(x =>
                    {
                        x.DueDate = param.DueDate;
                        return x;
                    }).ToList();
                }

                UpdateLogActivity
                .Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarApplicantLogActivityList>> GetApplicantLogActivityList(GetApplicantLogActivityListInput input, int rowStart)
        {
            return await _dbContext.TableVarApplicantLogActivityList
                .FromSqlRaw(@"CALL sp_applicant_log_activity_get_list(
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
                    )"
                    , input.ApplicantID
                    , input.TypeDelimited ?? ""
                    , input.SubTypeDelimited ?? ""
                      , input.Title ?? ""
                      , input.Description ?? ""
                      , input.CurrentStatusDelimited ?? ""
                      , input.CurrentTimestampFrom ?? ""
                      , input.CurrentTimestampTo ?? ""
                      , input.DueDateFrom ?? ""
                      , input.DueDateTo ?? ""
                      , input.AssignedByDelimited ?? ""
                      , input.AssignedToDelimited ?? ""
                      , input.Remarks ?? ""
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarChecklist>> GetEmployeeLogActivityList(GetChecklistListInput input, int rowStart)
        {
            return await _dbContext.TableVarChecklist
                .FromSqlRaw(@"CALL sp_employee_log_activity_get_list(
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
                    )"
                    , input.EmployeeID
                    , input.TypeDelimited ?? ""
                    , input.SubTypeDelimited ?? ""
                      , input.Title ?? ""
                      , input.Description ?? ""
                      , input.CurrentStatusDelimited ?? ""
                      , input.CurrentTimestampFrom ?? ""
                      , input.CurrentTimestampTo ?? ""
                      , input.DueDateFrom ?? ""
                      , input.DueDateTo ?? ""
                      , input.AssignedByDelimited ?? ""
                      , input.AssignedToDelimited ?? ""
                      , input.Remarks ?? ""
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicantLogActivity>> GetApplicantLogActivityByIDs(List<int> IDs)
        {
            return await _dbContext.ApplicantLogActivity.AsNoTracking()
                .Where(x => IDs.Contains(x.ID)).Where(x => x.IsActive).ToListAsync();
        }
        public async Task<bool> BatchUpdateApplicantLogActivity(List<ApplicantLogActivity> applicantLogActivityList, List<ApplicantLogActivityStatusHistory> applicantLogActivityHistoryList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                applicantLogActivityList.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                _dbContext.ApplicantLogActivityStatusHistory.AddRange(applicantLogActivityHistoryList);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> BatchUpdateEmployeeLogActivity(List<EmployeeLogActivity> employeeLogActivityList, List<EmployeeLogActivityStatusHistory> employeeLogActivityHistoryList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employeeLogActivityList.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                _dbContext.EmployeeLogActivityStatusHistory.AddRange(employeeLogActivityHistoryList);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<EmployeeLogActivity>> GetByUniqueLogActivity(int EmployeeID, string Type, string Title, int? OrgGroupID)
        {
            return await _dbContext.EmployeeLogActivity.AsNoTracking().Where(x =>
                x.EmployeeID == EmployeeID &
                x.Type.Equals(Type, StringComparison.OrdinalIgnoreCase) &
                x.Title.Equals(Title, StringComparison.OrdinalIgnoreCase) &
                x.AssignedOrgGroupID == OrgGroupID
            ).ToListAsync();
        }
        public async Task<bool> UploadLogActivityInsert(List<UploadLogActivityFile> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                List<EmployeeLogActivity> logActivity = param.Select(x => new EmployeeLogActivity
                {
                    EmployeeID = x.EmployeeID,
                    EmployeeName = x.EmployeeName,
                    AssignedUserID = x.AssignedUserId,
                    Type = x.Type,
                    Title = x.Title,
                    Email = x.Email,
                    Description = x.Description,
                    AssignedOrgGroupID = x.AssignedOrgGroupID,
                    CurrentStatus = x.CurrentStatus,
                    DueDate = x.DueDate,
                    IsActive = true,
                    IsVisible = true,
                    SubType = "",
                    CreatedBy = x.CreatedBy,
                    CurrentTimestamp = DateTime.Now
                }).ToList();

                await _dbContext.EmployeeLogActivity.AddRangeAsync(logActivity);
                await _dbContext.SaveChangesAsync();

                await _dbContext.EmployeeLogActivityStatusHistory.AddRangeAsync(logActivity
                    .Join(param,
                    x => new { x.EmployeeID, x.Type, x.Title, x.AssignedOrgGroupID },
                    y => new { y.EmployeeID, y.Type, y.Title, y.AssignedOrgGroupID },
                    (x, y) => new { logActivity = x, param = y })
                    .Select(y => new EmployeeLogActivityStatusHistory
                    {
                        EmployeeLogActivityID = y.logActivity.ID,
                        Status = y.param.CurrentStatus,
                        Remarks = y.param.Remarks,
                        UserID = y.param.CreatedBy,
                        Timestamp = DateTime.Now
                    }).ToList());

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}