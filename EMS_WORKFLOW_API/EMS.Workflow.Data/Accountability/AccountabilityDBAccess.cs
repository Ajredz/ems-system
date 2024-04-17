using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Question;
using EMS.Workflow.Data.Training;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Data.Accountability
{
    public interface IAccountabilityDBAccess
    {
        Task<IEnumerable<TableVarAccountability>> GetList(GetAccountabilityListInput input, int rowStart);
        
        Task<Accountability> GetByID(int ID);

        Task<IEnumerable<Accountability>> GetByName(string name);

        Task<IEnumerable<AccountabilityDetails>> GetDetailsByAccountabilityID(int ID);

        Task<bool> Post(Accountability accountability, List<AccountabilityDetails> lstAccountabilityDetails);

        Task<bool> Put(Accountability accountability, List<AccountabilityDetails> toDeleteDetails,
                         List<AccountabilityDetails> toAddDetails, List<AccountabilityDetails> toUpdateDetails);

        Task<IEnumerable<Accountability>> GetAllAccountability();

        Task<IEnumerable<TableVarEmployeeAccountability>> GetEmployeeAccountabilityByEmployeeID(int EmployeeID);

        Task<IEnumerable<AccountabilityDetails>> GetAccountabilityDetails(int AccountabilityID);

        Task<IEnumerable<EmployeeAccountabilityStatusHistory>> AddEmployeePreLoadedAccountability(AddEmployeePreLoadedAccountabilityInput param, int UserID);

        Task<bool> AddEmployeeAccountability(EmployeeAccountability employeeAccountability, EmployeeAccountabilityStatusHistory accountabilityHistory);

        Task<EmployeeAccountability> GetEmployeeAccountabilityByID(int ID);
        Task<List<EmployeeAccountability>> GetEmployeeAccountabilityByIDs(List<long> ID);

        Task<IEnumerable<TableVarEmployeeAccountabilityStatusHistory>> GetEmployeeAccountabilityStatusHistory(int EmployeeAccountabilityID);

        Task<bool> AddEmployeeAccountabilityStatusHistory(EmployeeAccountability employeeAccountability, EmployeeAccountabilityStatusHistory accountabilityHistory);
        Task<bool> UpdateEmployeeAccountability(EmployeeAccountability employeeAccountabilit);

        Task<bool> AddStatusHistory(List<EmployeeAccountabilityStatusHistory> param);
        Task<bool> UpdateEmployeeAccountability(List<EmployeeAccountability> param);

        Task<bool> PostEmployeeComments(EmployeeAccountabilityComments param);

        Task<IEnumerable<EmployeeAccountabilityComments>> GetEmployeeComments(int EmployeeAccountabilityID);

        Task<bool> PostEmployeeAttachment(List<EmployeeAccountabilityAttachment> addAttachment, List<EmployeeAccountabilityAttachment> updateAttachment, List<EmployeeAccountabilityAttachment> deleteAttachment);

        Task<IEnumerable<EmployeeAccountabilityAttachment>> GetEmployeeAttachment(int EmployeeAccountabilityID);

        Task<EmployeeAccountabilityAttachment> GetEmployeeAttachmentByServerFile(string ServerFile);

        Task<IEnumerable<TableVarMyAccountabilities>> GetMyAccountabilitiesList(GetMyAccountabilitiesListInput input, int rowStart);

        Task<bool> BatchAccountabilityAdd(List<EmployeeAccountability> employeeAccountabilitList
            , List<EmployeeAccountabilityStatusHistory> accountabilityHistoryList);

        Task<IEnumerable<EmployeeAccountability>> GetByEmployeeID(int EmployeeID);

        Task<IEnumerable<EmployeeAccountability>> GetByUnique(int EmployeeID, string Type, string Title, int? OrgGroupID);

        Task<bool> UploadInsert(List<AccountabilityUploadFile> param);
        Task<bool> BulkEmployeeAccountabilityDelete(List<EmployeeAccountability> param);
        Task<IEnumerable<EmployeeAccountabilityComments>> GetAllLastCommentByEmployeeId(int EmployeeId);
        Task<IEnumerable<EmployeeAccountability>> GetAllEmployeeAccountability();
        Task<IEnumerable<EmployeeAccountability>> GetEmployeeByEmployeeAccountabilityIDs(List<int> EmployeeAccountabilityID);
        Task<IEnumerable<TableVarEmployeeAccountabilityStatusPercentage>> GetEmployeeAccountabilityStatusPercentage(GetEmployeeAccountabilityStatusPercentageInput param);
        Task<IEnumerable<QuestionEmployeeAnswer>> GetEmployeeAccountabilityExitClearance(int ID);
        Task<IEnumerable<TableVarEmployeeAccountabilityList>> GetEmployeeAccountabilityList(GetMyAccountabilitiesListInput param, int rowStart);
        Task<IEnumerable<TableVarAccountabilityDashboard>> GetAccountabilityDashboard(GetAccountabilityDashboardInput param);
        Task<IEnumerable<tvAddClearedEmployee>> GetCheckEmployeeCleared(string EmployeeID);
        Task<bool> PostClearedEmployee(List<ClearedEmployee> paramClearedEmployee);
        Task<bool> PostClearedEmployeeStatusHistory(List<ClearedEmployeeStatusHistory> paramClearedEmployeeStatusHistory);
        Task<IEnumerable<ClearedEmployee>> GetClearedEmployeeByEmployeeID(List<int> EmployeeIDs);
        Task<IEnumerable<tvClearedEmployeeList>> GetEmployeeClearedList(ClearedEmployeeListInput param);
        Task<IEnumerable<tvClearedEmployeeByID>> GetClearedEmployeeByID(int ID);
        Task<ClearedEmployee> GetClearedEmployeeByIDDefault(int ID);
        Task<IEnumerable<ClearedEmployee>> GetClearedEmployeeByIDsDefault(List<int> IDs);
        Task<bool> PostClearedEmployeeComments(ClearedEmployeeComments param);
        Task<IEnumerable<tvClearedEmployeeComments>> GetClearedEmployeeComments(int ClearedEmployeeID);
        Task<IEnumerable<tvClearedEmployeeStatusHistory>> GetClearedEmployeeStatusHistory(int ClearedEmployeeID);
        Task<IEnumerable<tvEmployeeAccountability>> GetEmployeeAccountability(int EmployeeID);
        Task<bool> PutClearedEmployee(ClearedEmployee param);
        Task<bool> PutClearedEmployees(List<ClearedEmployee> param);
        Task<ClearedEmployee> GetClearedEmployeeByEmployeeID(int EmployeeID);
    }

    public class AccountabilityDBAccess : IAccountabilityDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public AccountabilityDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarAccountability>> GetList(GetAccountabilityListInput input, int rowStart)
        {
            return await _dbContext.TableVarAccountability
                .FromSqlRaw(@"CALL sp_accountability_get_list(
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

        public async Task<Accountability> GetByID(int ID)
        {
            return await _dbContext.Accountability.FindAsync(ID);
        }

        public async Task<IEnumerable<Accountability>> GetByName(string name)
        {
            return await _dbContext.Accountability.AsNoTracking()
                .Where(x => x.IsActive && x.PreloadName == name)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccountabilityDetails>> GetDetailsByAccountabilityID(int ID)
        {
            return await _dbContext.AccountabilityDetails.AsNoTracking()
                .Where(x => x.IsActive && x.AccountabilityID == ID)
                .ToListAsync();
        }

        public async Task<bool> Post(Accountability accountability, List<AccountabilityDetails> lstAccountabilityDetails)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Accountability.AddAsync(accountability);
                await _dbContext.SaveChangesAsync();

                if (lstAccountabilityDetails != null)
                {
                    await _dbContext.AccountabilityDetails.AddRangeAsync(lstAccountabilityDetails
                        .Select(x => { x.AccountabilityID = accountability.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(Accountability accountability, List<AccountabilityDetails> toDeleteDetails,
                         List<AccountabilityDetails> toAddDetails, List<AccountabilityDetails> toUpdateDetails)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(accountability).State = EntityState.Modified;

                // Execute filtered records to their respective actions.
                if (toAddDetails != null)
                {
                    _dbContext.AccountabilityDetails.AddRange(toAddDetails);
                }

                if (toUpdateDetails != null)
                {
                    toUpdateDetails.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                if (toDeleteDetails != null)
                {
                    toDeleteDetails.Select(x =>
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

        public async Task<IEnumerable<Accountability>> GetAllAccountability()
        {
            return await _dbContext.Accountability.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeAccountability>> GetEmployeeAccountabilityByEmployeeID(int EmployeeID)
        {
            return await _dbContext.TableVarEmployeeAccountability
                .FromSqlRaw("CALL sp_employee_accountability_get({0})", EmployeeID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<AccountabilityDetails>> GetAccountabilityDetails(int AccountabilityID)
        {
            return await _dbContext.AccountabilityDetails
                .FromSqlRaw("CALL sp_accountability_details_get({0})", AccountabilityID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAccountabilityStatusHistory>> AddEmployeePreLoadedAccountability(AddEmployeePreLoadedAccountabilityInput param, int UserID)
        {
            return await _dbContext.EmployeeAccountabilityStatusHistory
                .FromSqlRaw(@"CALL sp_accountability_add_preloaded_to_employee(
                      {0}
                    , {1}
                    , {2}
                    , {3}
                    )", string.Join(",", param.AccountabilityPreloadedIDs)
                    , param.EmployeeID
                    , UserID
                    , param.PositionID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> AddEmployeeAccountability(EmployeeAccountability employeeAccountability, EmployeeAccountabilityStatusHistory accountabilityHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeAccountability.Add(employeeAccountability);
                await _dbContext.SaveChangesAsync();

                accountabilityHistory.EmployeeAccountabilityID = employeeAccountability.ID;
                _dbContext.EmployeeAccountabilityStatusHistory.Add(accountabilityHistory);

                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }

        public async Task<EmployeeAccountability> GetEmployeeAccountabilityByID(int ID)
        {
            return await _dbContext.EmployeeAccountability.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }
        public async Task<List<EmployeeAccountability>> GetEmployeeAccountabilityByIDs(List<long> ID)
        {
            return await _dbContext.EmployeeAccountability.AsNoTracking().Where(x => ID.Contains(x.ID)).ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeAccountabilityStatusHistory>> GetEmployeeAccountabilityStatusHistory(int EmployeeAccountabilityID)
        {
            return await _dbContext.TableVarEmployeeAccountabilityStatusHistory
               .FromSqlRaw("CALL sp_employee_accountability_status_history_get({0})", EmployeeAccountabilityID)
               .AsNoTracking().ToListAsync();

        }

        public async Task<bool> AddEmployeeAccountabilityStatusHistory(EmployeeAccountability employeeAccountability
            , EmployeeAccountabilityStatusHistory accountabilityHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeAccountability.Update(employeeAccountability);
                if (!string.IsNullOrEmpty(accountabilityHistory.Status))
                {
                    _dbContext.EmployeeAccountabilityStatusHistory.Add(accountabilityHistory);
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool>UpdateEmployeeAccountability(EmployeeAccountability employeeAccountability)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeAccountability.Update(employeeAccountability);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> AddStatusHistory(List<EmployeeAccountabilityStatusHistory> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeAccountabilityStatusHistory.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> UpdateEmployeeAccountability(List<EmployeeAccountability> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeAccountability.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> PostEmployeeComments(EmployeeAccountabilityComments param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeAccountabilityComments.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<EmployeeAccountabilityComments>> GetEmployeeComments(int EmployeeAccountabilityID)
        {
            return await _dbContext.EmployeeAccountabilityComments.AsNoTracking()
                .Where(x => x.EmployeeAccountabilityID == EmployeeAccountabilityID).ToListAsync();
        }

        public async Task<bool> PostEmployeeAttachment(List<EmployeeAccountabilityAttachment> addAttachment,
            List<EmployeeAccountabilityAttachment> updateAttachment,
            List<EmployeeAccountabilityAttachment> deleteAttachment)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (deleteAttachment?.Count > 0)
                {
                    _dbContext.EmployeeAccountabilityAttachment.RemoveRange(deleteAttachment);
                }
                await _dbContext.EmployeeAccountabilityAttachment.AddRangeAsync(addAttachment);
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

        public async Task<EmployeeAccountabilityAttachment> GetEmployeeAttachmentByServerFile(string ServerFile)
        {
            return await _dbContext.EmployeeAccountabilityAttachment.AsNoTracking()
                .Where(x => x.ServerFile.Equals(ServerFile)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeAccountabilityAttachment>> GetEmployeeAttachment(int EmployeeAccountabilityID)
        {
            return await _dbContext.EmployeeAccountabilityAttachment.AsNoTracking()
                .Where(x => x.EmployeeAccountabilityID == EmployeeAccountabilityID).ToListAsync();
        }

        public async Task<IEnumerable<TableVarMyAccountabilities>> GetMyAccountabilitiesList(GetMyAccountabilitiesListInput input, int rowStart)
        {
            return await _dbContext.TableVarMyAccountabilities
                .FromSqlRaw(@"CALL sp_my_accountabilities_get_list(
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
                    )"
                      , input.sidx ?? ""
                      , input.sord ?? ""
                      , rowStart
                      , input.rows

                      , input.ID
                      , input.CreatedDateFrom ?? ""
                      , input.CreatedDateTo ?? ""
                      , input.EmployeeIDDelimited ?? ""
                      , input.Title ?? ""
                      , input.StatusDelimited ?? ""
                      , input.StatusUpdatedDateFrom ?? ""
                      , input.StatusUpdatedDateTo ?? ""
                      , input.EmployeeOrgDelimited ?? ""
                      , input.EmployeePosDelimited ?? ""
                      , input.ClearingOrgDelimited ?? ""
                      , input.StatusUpdatedByDelimited ?? ""
                      , input.StatusRemarks ?? ""
                      , input.LastComment ?? ""
                      , input.LastCommentDateFrom ?? ""
                      , input.LastCommentDateTo ?? ""

                      , input.OpenStatusOnly
                      , input.IsAdminAccess
                      , input.IsClearance
                      , input.IsExport
                      , input.OrgGroupDescendantAccess ?? ""
                      , input.PositionAccess ?? ""
                      , input.EmployeeIDDescendantAccess ?? ""
                      , input.MyEmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> BatchAccountabilityAdd(List<EmployeeAccountability> employeeAccountabilitList
            , List<EmployeeAccountabilityStatusHistory> accountabilityHistoryList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employeeAccountabilitList.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                await _dbContext.EmployeeAccountabilityStatusHistory.AddRangeAsync(accountabilityHistoryList);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<EmployeeAccountability>> GetByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmployeeAccountability.AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAccountability>> GetByUnique(int EmployeeID, string Type, string Title, int? OrgGroupID)
        {
            return await _dbContext.EmployeeAccountability.AsNoTracking().Where(x =>
                x.EmployeeID == EmployeeID &
                x.Type.Equals(Type, StringComparison.OrdinalIgnoreCase) &
                x.Title.Equals(Title, StringComparison.OrdinalIgnoreCase) &
                x.OrgGroupID == OrgGroupID
            ).ToListAsync();
        }

        public async Task<bool> UploadInsert(List<AccountabilityUploadFile> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                List<EmployeeAccountability> accountability = param.Select(x => new EmployeeAccountability
                {
                    EmployeeID = x.EmployeeID,
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    OrgGroupID = x.OrgGroupID,
                    Status = x.Status,
                    StatusUpdatedDate = x.StatusUpdatedDate,
                    IsActive = true,
                    CreatedBy = x.UploadedBy
                }).ToList();

                await _dbContext.EmployeeAccountability.AddRangeAsync(accountability);
                await _dbContext.SaveChangesAsync();

                await _dbContext.EmployeeAccountabilityStatusHistory.AddRangeAsync(accountability
                    .Join(param,
                    x => new { x.EmployeeID, x.Type, x.Title, x.OrgGroupID },
                    y => new { y.EmployeeID, y.Type, y.Title, y.OrgGroupID },
                    (x, y) => new { accountability = x, param = y })
                    .Select(y => new EmployeeAccountabilityStatusHistory
                    {
                        EmployeeAccountabilityID = y.accountability.ID,
                        Status = y.param.Status,
                        Remarks = y.param.Remarks,
                        UserID = y.param.UploadedBy,
                        Timestamp = y.param.StatusUpdatedDate
                    }).ToList());

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> BulkEmployeeAccountabilityDelete(List<EmployeeAccountability> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeAccountability.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<EmployeeAccountabilityComments>> GetAllLastCommentByEmployeeId(int EmployeeId)
        {
            return await _dbContext.EmployeeAccountabilityComments
                .FromSqlRaw(@"CALL sp_get_all_last_comment_by_employee_id(
                              {0}
                    )"
                    , EmployeeId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAccountability>> GetAllEmployeeAccountability()
        {
            return await _dbContext.EmployeeAccountability.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeAccountability>> GetEmployeeByEmployeeAccountabilityIDs(List<int> EmployeeAccountabilityID)
        {
            return await _dbContext.EmployeeAccountability.AsNoTracking()
                .Where(x => x.IsActive && EmployeeAccountabilityID.Contains(x.ID)).ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeAccountabilityStatusPercentage>> GetEmployeeAccountabilityStatusPercentage(GetEmployeeAccountabilityStatusPercentageInput param)
        {
            return await _dbContext.TableVarEmployeeAccountabilityStatusPercentage
                .FromSqlRaw(@"CALL sp_employee_accountability_get_status_percentage(
                              {0}
                            , {1}
                    )"
                    , string.Join(",", param.EmployeeIDs)
                    , param.ClearingOrgIDDelimited ?? "")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<QuestionEmployeeAnswer>> GetEmployeeAccountabilityExitClearance(int ID)
        {
            return await _dbContext.QuestionEmployeeAnswer.AsNoTracking()
                .Where(x => x.IsActive && x.EmployeeID.Equals(ID)).ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeAccountabilityList>> GetEmployeeAccountabilityList(GetMyAccountabilitiesListInput param, int rowStart)
        {
            return await _dbContext.TableVarEmployeeAccountabilityList
                .FromSqlRaw(@"CALL sp_test(
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
                    )"
                      , param.sidx ?? ""
                      , param.sord ?? ""
                      , rowStart
                      , param.rows

                      , param.ID
                      , param.CreatedDateFrom ?? ""
                      , param.CreatedDateTo ?? ""
                      , param.EmployeeIDDelimited ?? ""
                      , param.Title ?? ""
                      , param.StatusDelimited ?? ""
                      , param.StatusUpdatedDateFrom ?? ""
                      , param.StatusUpdatedDateTo ?? ""
                      , param.EmployeeOrgDelimited ?? ""
                      , param.EmployeePosDelimited ?? ""
                      , param.ClearingOrgDelimited ?? ""
                      , param.StatusUpdatedByDelimited ?? ""
                      , param.StatusRemarks ?? ""
                      , param.LastComment ?? ""
                      , param.LastCommentDateFrom ?? ""
                      , param.LastCommentDateTo ?? ""

                      , param.OpenStatusOnly
                      , param.IsAdminAccess
                      , param.IsClearance
                      , param.IsExport
                      , param.OrgGroupDescendantAccess ?? ""
                      , param.PositionAccess ?? ""
                      , param.EmployeeIDDescendantAccess ?? ""
                      , param.MyEmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarAccountabilityDashboard>> GetAccountabilityDashboard(GetAccountabilityDashboardInput param)
        {
            return await _dbContext.TableVarAccountabilityDashboard
                .FromSqlRaw(@"CALL sp_accountability_dashboard(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                    )"
                      , param.DashboardData ?? ""
                      , param.OrgGroupID ?? ""
                      , param.PositionID ?? ""
                      , param.EmploymentStatus ?? ""
                      , param.DateFilter ?? ""
                      , param.DateFrom ?? ""
                      , param.DateTo ?? ""
                      , param.AccessOrg ?? "")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<tvAddClearedEmployee>> GetCheckEmployeeCleared(string EmployeeID)
        {
            return await _dbContext.tvAddClearedEmployee
                .FromSqlRaw(@"CALL sp_add_cleared_employee(
                              {0}
                    )"
                      , EmployeeID ?? "")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> PostClearedEmployee(List<ClearedEmployee> paramClearedEmployee)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ClearedEmployee.AddRangeAsync(paramClearedEmployee);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PostClearedEmployeeStatusHistory(List<ClearedEmployeeStatusHistory> paramClearedEmployeeStatusHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ClearedEmployeeStatusHistory.AddRangeAsync(paramClearedEmployeeStatusHistory);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<ClearedEmployee>> GetClearedEmployeeByEmployeeID(List<int> EmployeeIDs)
        {
            return await _dbContext.ClearedEmployee.AsNoTracking().Where(x=>EmployeeIDs.Contains(x.EmployeeID) && x.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<tvClearedEmployeeList>> GetEmployeeClearedList(ClearedEmployeeListInput param)
        {
            return await _dbContext.tvClearedEmployeeList
                .FromSqlRaw(@"CALL sp_cleared_employee_list(
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
                      , param.ID
                      , param.EmployeeID ?? ""
                      , param.OrgGroupID ?? ""
                      , param.PositionID ?? ""
                      , param.Status ?? ""
                      , param.StatusUpdatedByID ?? ""
                      , param.StatusUpdatedDateFrom ?? ""
                      , param.StatusUpdatedDateTo ?? ""
                      , param.StatusRemarks ?? ""
                      , param.Computation ?? ""
                      , param.LastComment ?? ""
                      , param.LastCommentFrom ?? ""
                      , param.LastCommentTo ?? ""
                      
                      , param.IsExport

                      , param.sidx ?? ""
                      , param.sord ?? ""
                      , param.pageNumber
                      , param.rows)
                .AsNoTracking()
                .ToListAsync();

        }
        public async Task<IEnumerable<tvClearedEmployeeByID>> GetClearedEmployeeByID(int ID)
        {
            return await _dbContext.tvClearedEmployeeByID
                .FromSqlRaw(@"CALL sp_cleared_employee_by_id(
                              {0}
                    )"
                      , ID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ClearedEmployee> GetClearedEmployeeByIDDefault(int ID)
        {
            return await _dbContext.ClearedEmployee.AsNoTracking().Where(x => x.ID.Equals(ID) && x.IsActive).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ClearedEmployee>> GetClearedEmployeeByIDsDefault(List<int> IDs)
        {
            return await _dbContext.ClearedEmployee.AsNoTracking().Where(x => IDs.Contains(x.ID) && x.IsActive).ToListAsync();
        }
        public async Task<bool> PostClearedEmployeeComments(ClearedEmployeeComments param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.ClearedEmployeeComments.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<IEnumerable<tvClearedEmployeeComments>> GetClearedEmployeeComments(int ClearedEmployeeID)
        {
            return await _dbContext.tvClearedEmployeeComments
                .FromSqlRaw(@"CALL sp_cleared_employee_comments(
                              {0}
                    )"
                      , ClearedEmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<tvClearedEmployeeStatusHistory>> GetClearedEmployeeStatusHistory(int ClearedEmployeeID)
        {
            return await _dbContext.tvClearedEmployeeStatusHistory
                .FromSqlRaw(@"CALL sp_cleared_employee_status_history(
                              {0}
                    )"
                      , ClearedEmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<tvEmployeeAccountability>> GetEmployeeAccountability(int EmployeeID)
        {
            return await _dbContext.tvEmployeeAccountability
                .FromSqlRaw(@"CALL sp_employee_accountability(
                              {0}
                    )"
                      , EmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> PutClearedEmployee(ClearedEmployee param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ClearedEmployee.Update(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PutClearedEmployees(List<ClearedEmployee> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ClearedEmployee.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<ClearedEmployee> GetClearedEmployeeByEmployeeID(int EmployeeID)
        {
            return await _dbContext.ClearedEmployee.AsNoTracking().Where(x => x.EmployeeID.Equals(EmployeeID) && x.IsActive).FirstOrDefaultAsync();
        }
    }
}
