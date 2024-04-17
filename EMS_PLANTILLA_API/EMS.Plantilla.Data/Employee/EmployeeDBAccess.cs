using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.Employee
{
    public interface IEmployeeDBAccess
    {
        Task<IEnumerable<Employee>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<TableVarEmployeeGetByID>> GetByUserID(int UserID);

        Task<IEnumerable<Employee>> GetByPositionIDOrgGroupID(GetByPositionIDOrgGroupIDInput param);

        Task<IEnumerable<TableVarEmployee>> GetList(GetListInput input, int rowStart);

        Task<IEnumerable<ViewEmployeeRoving>> GetRovingByPositionIDOrgGroupID(GetRovingByPositionIDOrgGroupIDInput param);

        //Orig
        //Task<IEnumerable<EmployeeRoving>> GetRovingByEmployeeID(int EmployeeID);

        Task<IEnumerable<EmployeeRoving>> GetRovingByEmployeeID(int EmployeeID);

        //Task<IEnumerable<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement>> GetRovingByEmployeeID(int EmployeeID);

        //added start
        //Task<IEnumerable<EMS.Plantilla.Data.EmployeeMovement.TableVarEmployeeMovement>> GetListEmployeeMovement
        //(EMS.Plantilla.Transfer.EmployeeMovement.GetListInput input, int rowStart);
        //added end

        Task<Employee> GetByID(int ID);

        Task<IEnumerable<Employee>> GetByCode(string Code);

        Task<bool> Post(Employee employee, List<EmployeeRoving> employeeRoving, List<EmployeeFamily> listFamily,
            List<EmployeeWorkingHistory> listWorkingHistory, EmployeeCompensation EmployeeCompensation,
            List<EmployeeEducation> listEmployeeEducation);

        Task<bool> Put(Employee employee,
            bool IsViewedSecondaryDesignation,
            List<EmployeeRoving> employeeRovingToAdd, List<EmployeeRoving> employeeRovingToDelete,
            bool IsViewedFamilyBackground,
            List<EmployeeFamily> listFamilyToDelete, List<EmployeeFamily> listFamilyToAdd, List<EmployeeFamily> listFamilyToUpdate,
            bool IsViewedWorkingHistory,
            List<EmployeeWorkingHistory> listWorkingHistoryToDelete, List<EmployeeWorkingHistory> listWorkingHistoryToAdd,
            List<EmployeeWorkingHistory> listWorkingHistoryToUpdate,
            //EmployeeCompensation employeeCompensationToUpdate,
            //EmployeeCompensation employeeCompensationToAdd,
            bool IsViewedEducation,
            List<EmployeeEducation> listEmployeeEducationToDelete, List<EmployeeEducation> listEmployeeEducationToAdd,
            List<EmployeeEducation> listEmployeeEducationToUpdate);

        Task<bool> Put(Employee param);

        Task<IEnumerable<Employee>> GetByUserIDs(List<int> UserIDs);

        Task<IEnumerable<EmployeeFamily>> GetFamilyByEmployeeID(int EmployeeID);

        Task<EmployeeCompensation> GetCompensationByEmployeeID(int EmployeeID);

        Task<IEnumerable<EmployeeEducation>> GetEducationByEmployeeID(int EmployeeID);

        Task<IEnumerable<EmployeeWorkingHistory>> GetWorkingHistoryByEmployeeID(int EmployeeID);

        Task<IEnumerable<EmploymentStatusHistory>> GetEmploymentStatusByEmployeeID(int EmployeeID);

        Task<bool> Put(EmployeeOnboarding param);

        Task<EmployeeOnboarding> GetEmployeeOnboarding(int EmployeeID);

        Task<bool> Post(EmployeeOnboarding param);

        Task<IEnumerable<Employee>> GetEmployeeWithSystemUserAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<Employee>> GetByCodes(List<string> Codes);

        Task<IEnumerable<TableVarNewEmployeeCode>> GetNewEmployeeCode(string CompanyTag, bool GetCounterGaps = false);

        Task<Employee> AutoAdd(Employee employee, EmploymentStatusHistory employmentHistory);

        Task<IEnumerable<TableVarEmployeeETF>> GetETFList(GetListInput input, int rowStart);

        Task<bool> Put(EmployeeCompensation employeeCompensationToUpdate,
            EmployeeCompensation employeeCompensationToAdd);

        Task<IEnumerable<Employee>> GetByOldCode(string OldCode);

        Task<bool> UploadInsert(List<Employee> employees,
            List<Transfer.Employee.EmployeeCompensationForm> employeeCompensation,
            List<Transfer.Employee.EmployeeEducation> employeeEducation,
            List<Transfer.Employee.EmployeeFamily> employeeFamily,
            List<Transfer.Employee.EmployeeWorkingHistory> employeeWorkingHistory
            );

        Task<IEnumerable<Employee>> GetByNewCodes(List<string> NewCodes);

        Task<bool> Put(List<Employee> employees);

        Task<IEnumerable<TableVarEmployeeGetByID>> GetEmployeeByNewCode(string NewEmployeeCode);

        Task<IEnumerable<Employee>> GetByIDs(List<int> IDs);

        Task<IEnumerable<TableVarPrintCOE>> GetPrintCOE(int EmployeeID, int HREmployeeID);

        Task<IEnumerable<Employee>> GetOldEmployeeIDAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<Employee>> GetByOldEmployeeID(string OldEmployeeID);

        Task<IEnumerable<Employee>> GetByOldEmployeeIDs(List<string> OldIDs);

        Task<IEnumerable<Employee>> GetLastModified(DateTime? From, DateTime? To);

        Task<IEnumerable<EmployeeRoving>> GetLastModifiedRoving(DateTime? From, DateTime? To);

        Task<bool> PostEmployeeAttachment(List<EmployeeAttachment> addAttachment,
            List<EmployeeAttachment> updateAttachment,
            List<EmployeeAttachment> deleteAttachment);

        Task<IEnumerable<EmployeeAttachment>> GetEmployeeAttachment(int EmployeeID);

        Task<bool> PostEmployeeSkills(EmployeeSkills employeeSkills);
        Task<bool> PutEmployeeSkills(EmployeeSkills employeeSkills);
        Task<EmployeeSkills> GetEmployeeSkillsById(int Id);
        Task<IEnumerable<TableVarEmployeeSkills>> GetEmployeeSkillsByEmployeeId(EmployeeSkillsFormInput input, int rowStart);
        Task<IEnumerable<TableVarLoginExternal>> GetExternalEmployeeDetails(ExternalLogin param);

        Task<IEnumerable<TableVarEmployeeCorporateEmail>> GetCorporateEmailList(GetListCorporateEmailInput input, int rowStart);
        Task<IEnumerable<Employee>> GetAll();

        Task<bool> PostUpdateProfile(UpdateEmployee employee, List<UpdateEmployeeFamily> listFamily,
            List<UpdateEmployeeWorkingHistory> listWorkingHistory, List<UpdateEmployeeEducation> listEmployeeEducation);
        Task<IEnumerable<TableVarGetEmail>> GetEmail(GetEmailInput param);
        Task<bool> PutEmployees(List<Employee> param);
        Task<IEnumerable<Employee>> GetEmployeeIDDescendant(int EmployeeID);
        Task<IEnumerable<Employee>> GetEmployeeByOrgGroup(List<int> ID);
        Task<IEnumerable<Employee>> GetEmployeeByPosition(List<int> ID);
        Task<IEnumerable<TablerVarGetEmployeeLastEmploymentStatus>> GetEmployeeLastEmploymentStatus(List<int> ID);
        Task<IEnumerable<TablerVarGetEmployeeLastEmploymentStatus>> GetEmployeeLastEmploymentStatusByDate(GetEmployeeLastEmploymentStatusByDateInput param);
        Task<IEnumerable<Employee>> GetEmployeeByDateHired(GetEmployeeLastEmploymentStatusByDateInput param);
        Task<IEnumerable<TableVarEmployeeReportResult>> PostEmployeeReport(int CreatedBy);
        Task<IEnumerable<TableVarEmployeeReportGet>> GetEmployeeReportByTDate(string TDate);
        Task<IEnumerable<TableVarEmployeeReportGetOrg>> GetEmployeeReportOrgByTDate(string TDate);
        Task<IEnumerable<TableVarEmployeeReportGetOrg>> GetEmployeeReportRegionByTDate(string TDate);
        Task<IEnumerable<TableVarEmployeeEvaluation>> GetEmployeeEvaluation(string TDate);
        Task<IEnumerable<Employee>> GetEmployeeIfExist(string FName, string LName, DateTime BDate);
    }

    public class EmployeeDBAccess : IEmployeeDBAccess
    {
        private readonly PlantillaContext _dbContext;

        
        //added end
        public EmployeeDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Employee
                .FromSqlRaw("CALL sp_employee_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeGetByID>> GetByUserID(int UserID)
        {
            return await _dbContext.TableVarEmployeeGetByID
                .FromSqlRaw("CALL sp_employee_get_by_id({0})", UserID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByPositionIDOrgGroupID(GetByPositionIDOrgGroupIDInput param)
        {
            return await _dbContext.Employee
                .AsNoTracking()
                .Where(x => x.PositionID == param.PositionID & x.OrgGroupID == param.OrgGroupID & x.IsActive) //TO GET ALL ACTIVE EMPLOYEE ONLY
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployee>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployee
               .FromSqlRaw(@"CALL sp_employee_get_list(
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
                            , {29}
                            , {30}
                        )",
                              input.ID ?? 0
                            , input.Code ?? ""
                            , input.Name ?? ""
                            , input.OrgGroupDelimited ?? ""
                            , input.PositionDelimited ?? ""
                            , input.EmploymentStatusDelimited ?? ""
                            , input.CurrentStepDelimited ?? ""
                            , input.DateScheduledFrom ?? ""
                            , input.DateScheduledTo ?? ""
                            , input.DateCompletedFrom ?? ""
                            , input.DateCompletedTo ?? ""
                            , input.Remarks ?? ""
                            , input.DateStatusFrom ?? ""
                            , input.DateStatusTo ?? ""
                            , input.DateHiredFrom ?? ""
                            , input.DateHiredTo ?? ""
                            , input.BirthDateFrom ?? ""
                            , input.BirthDateTo ?? ""
                            , input.MovementDateFrom ?? ""
                            , input.MovementDateTo ?? ""
                            , input.OldEmployeeID ?? ""
                            , input.OrgGroupDelimitedClus ?? ""
                            , input.OrgGroupDelimitedArea ?? ""
                            , input.OrgGroupDelimitedReg ?? ""
                            , input.OrgGroupDelimitedZone ?? ""
                            , input.ShowActiveEmployee
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<ViewEmployeeRoving>> GetRovingByPositionIDOrgGroupID(GetRovingByPositionIDOrgGroupIDInput param)
        {
            return await _dbContext.ViewEmployeeRoving
                .AsNoTracking()
                .Where(x => x.PositionID == param.PositionID & x.OrgGroupID == param.OrgGroupID)
                .ToListAsync();
        }

        ////Orig
        //public async Task<IEnumerable<EmployeeRoving>> GetRovingByEmployeeID(int EmployeeID)
        //{
        //    return await _dbContext.EmployeeRoving
        //    .AsNoTracking()
        //    .Where(x => x.EmployeeID == EmployeeID & x.IsActive)
        //    .ToListAsync();
        //}

        public async Task<IEnumerable<EmployeeRoving>> GetRovingByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmployeeRoving
           .FromSqlRaw("CALL sp_employee_mov_rov({0})", EmployeeID)
           .AsNoTracking()
           .ToListAsync();
        }


        //public async Task<IEnumerable<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement>> GetRovingByEmployeeID(int EmployeeID)
        //{
        //    return await _dbContext.EmployeeMovement
        //    .AsNoTracking()
        //    .Where(x => x.EmployeeID == EmployeeID & x.DateEffectiveTo.Equals(null))
        //    .ToListAsync();
        //}

        public async Task<Employee> GetByID(int ID)
        {
            return await _dbContext.Employee.FindAsync(ID);
        }

        public async Task<IEnumerable<Employee>> GetByCode(string Code)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => x.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }
        
        public async Task<bool> Post(Employee employee, List<EmployeeRoving> employeeRoving, List<EmployeeFamily> listFamily,
            List<EmployeeWorkingHistory> listWorkingHistory, EmployeeCompensation employeeCompensation,
            List<EmployeeEducation> listEmployeeEducation)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Employee.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                if (employeeRoving != null)
                {
                    await _dbContext.EmployeeRoving.AddRangeAsync(employeeRoving.Select(x => { x.EmployeeID = employee.ID; return x; }));
                }

                if (listFamily != null)
                {
                    await _dbContext.EmployeeFamily.AddRangeAsync(listFamily
                       .Select(x => { x.EmployeeID = employee.ID; return x; }));
                }

                if (listWorkingHistory != null)
                {
                    await _dbContext.EmployeeWorkingHistory.AddRangeAsync(listWorkingHistory
                       .Select(x => { x.EmployeeID = employee.ID; return x; }));
                }

                employeeCompensation.EmployeeID = employee.ID;
                await _dbContext.EmployeeCompensation.AddAsync(employeeCompensation);

                 if (listEmployeeEducation != null)
                {
                    await _dbContext.EmployeeEducation.AddRangeAsync(listEmployeeEducation
                       .Select(x => { x.EmployeeID = employee.ID; return x; }));
                }
                
                await _dbContext.SaveChangesAsync();

             //   await _dbContext.EmployeeMovement
             //.FromSqlRaw(@"CALL sp_employee_movement_batch_add({0}, {1}
             //           )", employee != null ? employee.ID.ToString() : "0"
             //         , employee != null ? employee.CreatedBy : 0)
             //.AsNoTracking()
             //.ToListAsync();

             //   await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(Employee employee,
            bool IsViewedSecondaryDesignation,
            List<EmployeeRoving> employeeRovingToAdd, List<EmployeeRoving> employeeRovingToDelete,
            bool IsViewedFamilyBackground,
            List<EmployeeFamily> listFamilyToDelete, List<EmployeeFamily> listFamilyToAdd, List<EmployeeFamily> listFamilyToUpdate,
            bool IsViewedWorkingHistory,
            List<EmployeeWorkingHistory> listWorkingHistoryToDelete, List<EmployeeWorkingHistory> listWorkingHistoryToAdd,
            List<EmployeeWorkingHistory> listWorkingHistoryToUpdate,
            //EmployeeCompensation employeeCompensationToUpdate,
            //EmployeeCompensation employeeCompensationToAdd,
            bool IsViewedEducation,
            List<EmployeeEducation> listEmployeeEducationToDelete, List<EmployeeEducation> listEmployeeEducationToAdd,
            List<EmployeeEducation> listEmployeeEducationToUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(employee).State = EntityState.Modified;

                if (IsViewedSecondaryDesignation)
                {
                    await _dbContext.EmployeeRoving.AddRangeAsync(employeeRovingToAdd);
                    employeeRovingToDelete.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList(); 
                }

                if (IsViewedFamilyBackground)
                {
                    _dbContext.EmployeeFamily.RemoveRange(listFamilyToDelete);
                    await _dbContext.EmployeeFamily.AddRangeAsync(listFamilyToAdd);
                    listFamilyToUpdate.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList(); 
                }

                if (IsViewedWorkingHistory)
                {
                    _dbContext.EmployeeWorkingHistory.RemoveRange(listWorkingHistoryToDelete);
                    await _dbContext.EmployeeWorkingHistory.AddRangeAsync(listWorkingHistoryToAdd);
                    listWorkingHistoryToUpdate.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList(); 
                }

                //if (employeeCompensationToUpdate != null)
                //{
                //    _dbContext.Entry(employeeCompensationToUpdate).State = EntityState.Modified;
                //}
                //else if (employeeCompensationToAdd != null)
                //{
                //    await _dbContext.EmployeeCompensation.AddAsync(employeeCompensationToAdd);
                //}
                
                if (IsViewedEducation)
                {
                    _dbContext.EmployeeEducation.RemoveRange(listEmployeeEducationToDelete);
                    await _dbContext.EmployeeEducation.AddRangeAsync(listEmployeeEducationToAdd);
                    listEmployeeEducationToUpdate.Select(x =>
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

        public async Task<bool> Put(Employee param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<Employee>> GetByUserIDs(List<int> UserIDs)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => UserIDs.Contains(x.SystemUserID))
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeFamily>> GetFamilyByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmployeeFamily
                .AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeWorkingHistory>> GetWorkingHistoryByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmployeeWorkingHistory
                .AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmploymentStatusHistory>> GetEmploymentStatusByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmploymentStatusHistory
                .AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID)
                .ToListAsync();
        }

        public async Task<EmployeeCompensation> GetCompensationByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmployeeCompensation
                .AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID)
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<EmployeeEducation>> GetEducationByEmployeeID(int EmployeeID)
        {
            return await _dbContext.EmployeeEducation
                .AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID)
                .ToListAsync();
        }


        public async Task<bool> Put(EmployeeOnboarding param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<EmployeeOnboarding> GetEmployeeOnboarding(int EmployeeID)
        {
            return await _dbContext.EmployeeOnboarding.AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID).FirstOrDefaultAsync();
        }

        public async Task<bool> Post(EmployeeOnboarding param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeOnboarding.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<Employee>> GetEmployeeWithSystemUserAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Employee
                .FromSqlRaw("CALL sp_employee_with_system_user_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByCodes(List<string> Codes)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => Codes.Contains(x.Code))
                //.Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarNewEmployeeCode>> GetNewEmployeeCode(string CompanyTag, bool GetCounterGaps = false)
        {
            return await _dbContext.TableVarNewEmployeeCode
                .FromSqlRaw("CALL sp_employee_get_new_code({0}, {1})", CompanyTag, GetCounterGaps)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> AutoAdd(Employee employee, EmploymentStatusHistory employmentHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Employee.AddAsync(employee);
                await _dbContext.SaveChangesAsync();


                employmentHistory.EmployeeID = employee.ID;
                _dbContext.EmploymentStatusHistory.Add(employmentHistory);

                await _dbContext.SaveChangesAsync();

               // await _dbContext.EmployeeMovement
               //.FromSqlRaw(@"CALL sp_employee_movement_batch_add({0}, {1}
               //         )", employee != null ? employee.ID.ToString() : "0"
               //         , employee != null ? employee.CreatedBy : 0)
               //.AsNoTracking()
               //.ToListAsync();

               // await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return employee;
        }

        public async Task<IEnumerable<TableVarEmployeeETF>> GetETFList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeETF
               .FromSqlRaw(@"CALL sp_employee_export_etf_list(
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
                        )",
                              input.ID ?? 0
                            , input.Code ?? ""
                            , input.Name ?? ""
                            , input.OrgGroupDelimited ?? ""
                            , input.PositionDelimited ?? ""
                            , input.EmploymentStatusDelimited ?? ""
                            , input.CurrentStepDelimited ?? ""
                            , input.DateScheduledFrom ?? ""
                            , input.DateScheduledTo ?? ""
                            , input.DateCompletedFrom ?? ""
                            , input.DateCompletedTo ?? ""
                            , input.Remarks ?? ""
                            , input.DateHiredFrom ?? ""
                            , input.DateHiredTo ?? ""
                            , input.OldEmployeeID ?? ""
                            , input.OrgGroupDelimitedClus ?? ""
                            , input.OrgGroupDelimitedArea ?? ""
                            , input.OrgGroupDelimitedReg ?? ""
                            , input.OrgGroupDelimitedZone ?? ""
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
		}
		
        public async Task<bool> Put(EmployeeCompensation employeeCompensationToUpdate,
            EmployeeCompensation employeeCompensationToAdd)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (employeeCompensationToUpdate != null)
                {
                    _dbContext.Entry(employeeCompensationToUpdate).State = EntityState.Modified;
                }
                else if (employeeCompensationToAdd != null)
                {
                    await _dbContext.EmployeeCompensation.AddAsync(employeeCompensationToAdd);
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<Employee>> GetByOldCode(string OldCode)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => OldCode.Equals(x.OldEmployeeID) & x.IsActive)
                .ToListAsync();
        }

        public async Task<bool> UploadInsert(List<Employee> employees,
            List<Transfer.Employee.EmployeeCompensationForm> employeeCompensation,
            List<Transfer.Employee.EmployeeEducation> employeeEducation,
            List<Transfer.Employee.EmployeeFamily> employeeFamily,
            List<Transfer.Employee.EmployeeWorkingHistory> employeeWorkingHistory
            )
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.Employee.AddRangeAsync(employees);
                await _dbContext.SaveChangesAsync();

                List<EmployeeCompensation> EmployeeCompensationList =
                    employeeCompensation.Join(employees,
                    x => new { x.UploadInsertEmployeeCode },
                    y => new { UploadInsertEmployeeCode = y.Code },
                    (x, y) => new { x, y }).Select(x =>
                        new EmployeeCompensation { 
                            EmployeeID = x.y.ID,
                            MonthlySalary = Convert.ToDecimal(x.x.MonthlySalary),
                            DailySalary = Convert.ToDecimal(x.x.DailySalary),
                            HourlySalary = Convert.ToDecimal(x.x.HourlySalary),
                            IsActive = x.y.IsActive,
                            CreatedBy = x.y.CreatedBy,
                        }).ToList();
                await _dbContext.EmployeeCompensation.AddRangeAsync(EmployeeCompensationList);

                List<EmployeeEducation> EmployeeEducationList =
                    employeeEducation.Join(employees,
                    x => new { x.UploadInsertEmployeeCode },
                    y => new { UploadInsertEmployeeCode = y.Code },
                    (x, y) => new { x, y }).Select(x =>
                        new EmployeeEducation
                        {
                            EmployeeID = x.y.ID,
                            School = x.x.School,
                            SchoolAddress = x.x.SchoolAddress,
                            SchoolLevelCode = x.x.SchoolLevelCode,
                            Course = x.x.Course,
                            YearFrom = x.x.YearFrom,
                            YearTo = x.x.YearTo,
                            EducationalAttainmentDegreeCode = x.x.EducationalAttainmentDegreeCode,
                            EducationalAttainmentStatusCode = x.x.EducationalAttainmentStatusCode,
                            IsActive = x.y.IsActive,
                            CreatedBy = x.y.CreatedBy,
                        }).ToList();
                await _dbContext.EmployeeEducation.AddRangeAsync(EmployeeEducationList);

                List<EmployeeFamily> EmployeeFamilyList =
                   employeeFamily.Join(employees,
                   x => new { x.UploadInsertEmployeeCode },
                   y => new { UploadInsertEmployeeCode = y.Code },
                   (x, y) => new { x, y }).Select(x =>
                       new EmployeeFamily
                       {
                           EmployeeID = x.y.ID,
                           Name = x.x.Name,
                           Relationship = x.x.Relationship,
                           BirthDate = x.x.BirthDate,
                           Occupation = x.x.Occupation,
                           ContactNumber = x.x.ContactNumber,
                           SpouseEmployer = x.x.SpouseEmployer,
                           CreatedBy = x.y.CreatedBy,
                       }).ToList();
                await _dbContext.EmployeeFamily.AddRangeAsync(EmployeeFamilyList);
                
                List<EmployeeWorkingHistory> EmployeeWorkingHistory =
                   employeeWorkingHistory.Join(employees,
                   x => new { x.UploadInsertEmployeeCode },
                   y => new { UploadInsertEmployeeCode = y.Code },
                   (x, y) => new { x, y }).Select(x =>
                       new EmployeeWorkingHistory
                       {
                           EmployeeID = x.y.ID,
                           CompanyName = x.x.CompanyName,
                           Position = x.x.Position,
                           From = x.x.From,
                           To = x.x.To,
                           ReasonForLeaving = x.x.ReasonForLeaving,
                           CreatedBy = x.y.CreatedBy,
                       }).ToList();
                await _dbContext.EmployeeWorkingHistory.AddRangeAsync(EmployeeWorkingHistory);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                // OLD CODES FROM PREVIOUS DEV
                // await _dbContext.EmployeeMovement
                //.FromSqlRaw(@"CALL sp_employee_movement_batch_add({0}, {1}
                //         )", employees != null ? employees.Count() > 0 ? string.Join(",",employees.Select(x => x.ID).ToArray()) : "0" : "0"
                //         , employees != null ? employees.Count() > 0 ? employees.First().CreatedBy : 0 : 0)
                //.AsNoTracking()
                //.ToListAsync();
                // await _dbContext.SaveChangesAsync();

            }
            return true;
        }

        public async Task<IEnumerable<Employee>> GetByNewCodes(List<string> NewCodes)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => NewCodes.Contains(x.Code))
                .Where(x => x.IsActive).ToListAsync();
        }


        public async Task<bool> Put(List<Employee> employees)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (employees != null)
                {
                    employees.Select(x =>
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

        public async Task<IEnumerable<TableVarEmployeeGetByID>> GetEmployeeByNewCode(string NewEmployeeCode)
        {
            return await _dbContext.TableVarEmployeeGetByID
            .FromSqlRaw("CALL sp_employee_get_by_new_code({0})", NewEmployeeCode)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByIDs(List<int> IDs)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => IDs.Contains(x.ID)).ToListAsync();
        }

        public async Task<IEnumerable<TableVarPrintCOE>> GetPrintCOE(int EmployeeID, int HREmployeeID)
        {
            return await _dbContext.TableVarPrintCOE
                .FromSqlRaw("CALL sp_print_coe({0}, {1})", EmployeeID, HREmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetOldEmployeeIDAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Employee
                .FromSqlRaw("CALL sp_employee_old_id_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByOldEmployeeID(string OldEmployeeID)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => x.OldEmployeeID.Equals(OldEmployeeID, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByOldEmployeeIDs(List<string> OldIDs)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => OldIDs.Contains(x.OldEmployeeID))
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.Employee.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeRoving>> GetLastModifiedRoving(DateTime? From, DateTime? To)
        {
            return await _dbContext.EmployeeRoving.AsNoTracking().Where(x =>
                    (x.CreatedDate) >= (From ?? (x.CreatedDate))
                    &
                    (x.CreatedDate) <= (To ?? (x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<bool> PostEmployeeAttachment(List<EmployeeAttachment> addAttachment,
            List<EmployeeAttachment> updateAttachment,
            List<EmployeeAttachment> deleteAttachment)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (deleteAttachment?.Count > 0)
                {
                    _dbContext.EmployeeAttachment.RemoveRange(deleteAttachment);
                }
                await _dbContext.EmployeeAttachment.AddRangeAsync(addAttachment);
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

        public async Task<IEnumerable<EmployeeAttachment>> GetEmployeeAttachment(int EmployeeID)
        {
            return await _dbContext.EmployeeAttachment.AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID).ToListAsync();
        }

        public async Task<bool> PostEmployeeSkills(EmployeeSkills employeeSkills)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeSkills.AddAsync(employeeSkills);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PutEmployeeSkills(EmployeeSkills employeeSkills)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(employeeSkills).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<EmployeeSkills> GetEmployeeSkillsById(int Id)
        {
            return await _dbContext.EmployeeSkills.FindAsync(Id);
        }
        public async Task<IEnumerable<TableVarEmployeeSkills>> GetEmployeeSkillsByEmployeeId(EmployeeSkillsFormInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeSkills
                  .FromSqlRaw(@"CALL sp_employee_get_list_skills(
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
                        )",
                                 input.ID ?? 0
                               , input.EmployeeID ?? 0
                               , input.SkillsCode ?? ""
                               , input.SkillsDescription ?? ""
                               , input.Rate ?? ""
                               , input.Remarks ?? ""
                               , input.CreatedBy ?? 0
                               , input.CreatedDate ?? null
                               , input.ModifiedBy ?? 0
                               , input.ModifiedDate ?? null
                               , input.IsActive
                               , input.IsExport
                               , input.sidx ?? ""
                               , input.sord ?? ""
                               , rowStart
                               , input.rows
                           )
                  .AsNoTracking()
                  .ToListAsync();
        }
        public async Task<IEnumerable<TableVarLoginExternal>> GetExternalEmployeeDetails(ExternalLogin param)
        {
            /* return await _dbContext.Employee.AsNoTracking()
                 .Where(x => x.Code.Equals(param.EmployeeId) && x.BirthDate.Equals(param.BirthDate) && (x.LastName.Equals(param.LastName) || x.LastName.Equals(param.LastName.ToUpper())))
                 .Where(x=>x.EmploymentStatus.Equals("RESIGNED") || x.EmploymentStatus.Equals("TERMINATED") || x.EmploymentStatus.Equals("AWOL") || x.EmploymentStatus.Equals("OUTGOING")).ToListAsync();*/

            return await _dbContext.TableVarLoginExternal
               .FromSqlRaw(@"CALL sp_login_external(
                              {0}
                            , {1}
                            , {2}
                        )",
                              param.EmployeeId ?? ""
                            , param.BirthDate ?? ""
                            , param.LastName ?? ""
                        )
               .AsNoTracking()
               .ToListAsync();
        }


        public async Task<IEnumerable<TableVarEmployeeCorporateEmail>> GetCorporateEmailList(GetListCorporateEmailInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeCorporateEmail
               .FromSqlRaw(@"CALL sp_employee_corporate_email_get_list(
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
                        )",
                              input.ID ?? 0
                            , input.Code ?? ""
                            , input.Name ?? ""
                            , input.OrgGroupDelimited ?? ""
                            , input.PositionDelimited ?? ""
                            , input.EmploymentStatusDelimited ?? ""
                            , input.CorporateEmail ?? ""
                            , input.OfficeMobile ?? ""
                            , input.IsDisplayDirectory ?? ""
                            , input.OldEmployeeID ?? ""
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _dbContext.Employee.AsNoTracking().ToListAsync();
        }
        public async Task<bool> PostUpdateProfile(UpdateEmployee employee, List<UpdateEmployeeFamily> listFamily,
            List<UpdateEmployeeWorkingHistory> listWorkingHistory, List<UpdateEmployeeEducation> listEmployeeEducation)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.UpdateEmployee.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                if (listFamily != null)
                {
                    await _dbContext.UpdateEmployeeFamily.AddRangeAsync(listFamily
                       .Select(x => { x.EmployeeID = employee.ID; return x; }));
                }

                if (listWorkingHistory != null)
                {
                    await _dbContext.UpdateEmployeeWorkingHistory.AddRangeAsync(listWorkingHistory
                       .Select(x => { x.EmployeeID = employee.ID; return x; }));
                }

                if (listEmployeeEducation != null)
                {
                    await _dbContext.UpdateEmployeeEducation.AddRangeAsync(listEmployeeEducation
                       .Select(x => { x.EmployeeID = employee.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<IEnumerable<TableVarGetEmail>> GetEmail(GetEmailInput param)
        {
            return await _dbContext.TableVarGetEmail
                .FromSqlRaw("CALL sp_get_email({0}, {1})", param.ID ?? 0, param.Condition ?? "")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> PutEmployees(List<Employee> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Employee.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<IEnumerable<Employee>> GetEmployeeIDDescendant(int EmployeeID)
        {
            return await _dbContext.Employee
                .FromSqlRaw("CALL sp_get_employee_id_descendant({0})", EmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetEmployeeByOrgGroup(List<int> ID)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => ID.Contains(x.OrgGroupID)).ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetEmployeeByPosition(List<int> ID)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => ID.Contains(x.PositionID)).ToListAsync();
        }
        public async Task<IEnumerable<TablerVarGetEmployeeLastEmploymentStatus>> GetEmployeeLastEmploymentStatus(List<int> ID)
        {
            return await _dbContext.TablerVarGetEmployeeLastEmploymentStatus
               .FromSqlRaw("CALL sp_get_employee_last_employment_status({0})", string.Join(",", ID))
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<TablerVarGetEmployeeLastEmploymentStatus>> GetEmployeeLastEmploymentStatusByDate(GetEmployeeLastEmploymentStatusByDateInput param)
        {
            return await _dbContext.TablerVarGetEmployeeLastEmploymentStatus
                .FromSqlRaw(@"CALL sp_get_employee_last_employment_status_by_date(
                              {0}
                            , {1}
                    )"
                    , param.DateFrom ?? ""
                    , param.DateTo ?? "")
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetEmployeeByDateHired(GetEmployeeLastEmploymentStatusByDateInput param)
        {
            return await _dbContext.Employee
                .FromSqlRaw(@"CALL sp_get_employee_by_date_hired(
                              {0}
                            , {1}
                    )"
                    , param.DateFrom ?? ""
                    , param.DateTo ?? "")
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeReportResult>> PostEmployeeReport(int CreatedBy)
        {
            return await _dbContext.TableVarEmployeeReportResult
                .FromSqlRaw("CALL sp_employee_report_insert({0})", CreatedBy)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeReportGet>> GetEmployeeReportByTDate(string TDate)
        {
            return await _dbContext.TableVarEmployeeReportGet
                .FromSqlRaw("CALL sp_employee_report_get_by_tdate({0})", TDate)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeReportGetOrg>> GetEmployeeReportOrgByTDate(string TDate)
        {
            return await _dbContext.TableVarEmployeeReportGetOrg
                .FromSqlRaw("CALL sp_employee_report_org_get_by_tdate({0})", TDate)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeReportGetOrg>> GetEmployeeReportRegionByTDate(string TDate)
        {
            return await _dbContext.TableVarEmployeeReportGetOrg
                .FromSqlRaw("CALL sp_employee_report_region_get_by_tdate({0})", TDate)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeEvaluation>> GetEmployeeEvaluation(string TDate)
        {
            return await _dbContext.TableVarEmployeeEvaluation
                .FromSqlRaw("CALL sp_get_employee_probationary({0})", TDate)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetEmployeeIfExist(string FName, string LName, DateTime BDate)
        {
            return await _dbContext.Employee.AsNoTracking()
                .Where(x => 
                x.FirstName.ToUpper().Equals(FName.ToUpper()) &&
                x.LastName.ToUpper().Equals(LName.ToUpper()) &&
                x.BirthDate.Equals(BDate) &&
                x.IsActive
                ).ToListAsync();
        }
    }
}