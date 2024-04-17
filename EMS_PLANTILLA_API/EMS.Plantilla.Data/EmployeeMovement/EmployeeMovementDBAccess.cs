using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.Employee;
using EMS.Plantilla.Transfer;
using EMS.Plantilla.Transfer.EmployeeMovement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.EmployeeMovement
{
    // For movement checker 2, 20, 63-68, 86-91, 121, 129, 193, 207, 313, 327, 541-569, 572-581, 583-587, 591-599
    public interface IEmployeeMovementDBAccess
    {

        Task<EmployeeMovement> GetByID(long ID);
        Task<List<EmployeeMovement>> GetByIDs(List<long> ID);

        Task<IEnumerable<TableVarEmployeeMovement>> GetList(GetListInput input, int rowStart);

        Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> GetAutoComplete(GetAutoCompleteByMovementTypeInput input);

        Task<List<EmployeeMovement>> Post(Form input);

        Task<IEnumerable<EmployeeMovement>> GetLatestByEmployeeIDAndEmployeeField(int EmployeeID, string EmployeeField);

        Task<IEnumerable<EmployeeMovement>> GetEmployeeMovementList(int EmployeeID, string EmployeeField);

        Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> ValidateMovementNewValue(string EmployeeField, string MovementType, string NewValue);

        Task<IEnumerable<TableVarEmployeeMovementGetPrint>> GetPrint(GetPrintInput param);

        Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> GetAutoPopulate(GetAutoPopulateByMovementTypeInput input);

        Task<bool> Put(EmployeeMovement param);

        Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> GetDescription(string EmployeeField, string NewValue);

        Task<List<EmployeeMovement>> Post(List<Form> input);

        Task<IEnumerable<EmployeeMovement>> SyncMovement();

        Task<IEnumerable<EmployeeMovement>> GetLastModified(DateTime? From, DateTime? To);

        Task<IEnumerable<EmployeeMovementMapping>> GetEmployeeFieldList(string type);

        Task<IEnumerable<EmployeeMovementMapping>> GetMovementType(string MovementType);

        Task<bool> AddMovementEmployeeField(EmployeeMovementMapping param);

        Task<IEnumerable<EmployeeMovementMapping>> GetEmployeeMovementField(EmployeeFieldForm param);

        Task<bool> BulkRemoveEmployeeField(List<int> IDs);

        Task<bool> Delete(int UserId, List<long> IDs);

        //ORIG
        Task<bool> Update(Form input);
        //Task<List<EmployeeMovement>> Update(Form input);
        Task<bool> PutEmployeeMovement(List<EmployeeMovement> param);
        Task<bool> PostEmployeeRoving(List<EmployeeRoving> param);
        Task<bool> PutEmployeeRoving(List<EmployeeRoving> param);

        Task<bool> PostEmployeeMovementStatusHistory(List<EmployeeMovementStatusHistory> param);
        Task<IEnumerable<EmployeeRoving>> GetEmployeeRoving(int EmployeeID, int OrgID, int PosID);
        Task<IEnumerable<TableVarEmployeeMovementByEmployeeIDs>> GetEmployeeMovementByEmployeeIDs(List<int> EmployeeIDs);
    }

    public class EmployeeMovementDBAccess : IEmployeeMovementDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public EmployeeMovementDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeMovement> GetByID(long ID)
        {
            return await _dbContext.EmployeeMovement.AsNoTracking()
                .Where(x => x.ID == ID).FirstOrDefaultAsync();
        }
        public async Task<List<EmployeeMovement>> GetByIDs(List<long> ID)
        {
            return await _dbContext.EmployeeMovement.AsNoTracking()
                .Where(x => ID.Contains(x.ID))
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeMovement>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeMovement
               .FromSqlRaw(@"CALL sp_employee_movement_get_list(
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
                        )",
                              input.EmployeeID
                            , input.EmployeeName ?? ""
                            , input.OldEmployeeID ?? ""
                            , input.OrgGroupDelimited ?? ""
                            , input.EmployeeFieldDelimited ?? ""
                            , input.MovementTypeDelimited ?? ""
                            , input.StatusDelimited ?? ""
                            , input.From ?? ""
                            , input.To ?? ""
                            , input.DateEffectiveFromFrom ?? ""
                            , input.DateEffectiveFromTo ?? ""
                            , input.DateEffectiveToFrom ?? ""
                            , input.DateEffectiveToTo ?? ""
                            , input.CreatedDateFrom ?? ""
                            , input.CreatedDateTo ?? ""
                            , input.CreatedByDelimited ?? ""
                            , input.Reason ?? ""
                            , input.HRDComments ?? ""
                            , input.IsShowActiveOnly
                            , input.HasConfidentialView
                            , input.IsExport
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> GetAutoComplete(GetAutoCompleteByMovementTypeInput input)
        {
            return await _dbContext.TableVarEmployeeMovementAutoComplete
               .FromSqlRaw(@"CALL sp_employee_movement_autocomplete({0}, {1}, {2},{3})"
                        , input.Term ?? "", input.TopResults, input.MovementType ?? "",input.OrgGroup ?? "")
               .AsNoTracking()
               .ToListAsync();
        }

        //public async Task<> GetEmployeeOrgGroupByID(int employeeId)
        //{
        //    return await _dbContext.Employee
               //.FromSqlRaw(@"CALL sp_employee_movement_autocomplete({0}, {1}, {2})"
               //         , input.Term ?? "", input.TopResults, input.MovementType ?? "")
               //.AsNoTracking()
               //.ToListAsync();
        //}

        //this pass data
        public async Task<List<EmployeeMovement>> Post(Form input)
        {
            List<EmployeeMovement> spResult = new List<EmployeeMovement>();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                spResult = await _dbContext.EmployeeMovement
                .FromSqlRaw(@"CALL sp_employee_movement_add(
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
                        )"
                            , input.EmployeeField ?? ""
                            , input.MovementType ?? ""
                            , input.EmployeeID
                            , input.NewValue ?? ""
                            , input.NewValueID ?? ""
                            , input.EffectiveDateFrom
                            , input.EffectiveDateTo
                            , input.Reason ?? ""
                            , input.HRDComments ?? ""
                            , input.CreatedBy
                            , input.FromValue ?? ""
                            , input.FromValueID ?? ""
                            , input.Status ?? ""
                            , input.Details ?? ""
                        )
                .AsNoTracking()
                .ToListAsync();
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return spResult;
        }

        public async Task<IEnumerable<EmployeeMovement>> GetLatestByEmployeeIDAndEmployeeField(int EmployeeID, string EmployeeField)
        {
            return await _dbContext.EmployeeMovement.AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID & x.EmployeeField.Equals(EmployeeField, StringComparison.OrdinalIgnoreCase)
                & x.IsLatest).Where(y => y.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeMovement>> GetEmployeeMovementList(int EmployeeID, string EmployeeField)
        {
            return await _dbContext.EmployeeMovement.AsNoTracking()
                .Where(x => x.EmployeeID == EmployeeID & x.EmployeeField.Equals(EmployeeField, StringComparison.OrdinalIgnoreCase))
                .Where(y => y.IsActive).OrderByDescending(x => x.DateEffectiveFrom).ToListAsync();
        }
        

        public async Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> ValidateMovementNewValue(string EmployeeField, string MovementType, string NewValue)
        {
            return await _dbContext.TableVarEmployeeMovementAutoComplete
               .FromSqlRaw(@"CALL sp_validate_movement_new_value(
                              {0}
                            , {1}
                            , {2}
                        )",
                              EmployeeField ?? ""
                            , MovementType ?? ""
                            , NewValue ?? ""
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeMovementGetPrint>> GetPrint(GetPrintInput param)
        {
            return await _dbContext.TableVarEmployeeMovementGetPrint
               .FromSqlRaw(@"CALL sp_employee_movement_get_print({0}, {1})", param.IDDelimited, param.HasConfidentialView)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> GetAutoPopulate(GetAutoPopulateByMovementTypeInput input)
        {
            return await _dbContext.TableVarEmployeeMovementAutoComplete
               .FromSqlRaw(@"CALL sp_employee_movement_autopopulate({0})", input.MovementType ?? "")
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<bool> Put(EmployeeMovement param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarEmployeeMovementAutoComplete>> GetDescription(string EmployeeField, string NewValue)
        {
            return await _dbContext.TableVarEmployeeMovementAutoComplete
               .FromSqlRaw(@"CALL sp_employee_movement_get_description(
                              {0}
                            , {1}
                        )",
                              EmployeeField ?? ""
                            , NewValue ?? ""
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<List<EmployeeMovement>> Post(List<Form> input)
        {
            List<EmployeeMovement> spResult = new List<EmployeeMovement>();
            try
            {
                var transaction = _dbContext.Database.BeginTransaction();
                foreach (var item in input.OrderBy(x => x.EmployeeID).ThenBy(x => DateTime.ParseExact(x.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture)))
                {
                    var sample = (await _dbContext.EmployeeMovement
                           .FromSqlRaw(@"CALL sp_employee_movement_add(
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
                            )"
                                    , item.EmployeeField ?? ""
                                    , item.MovementType ?? ""
                                    , item.EmployeeID
                                    , item.NewValue ?? ""
                                    , item.NewValueID ?? ""
                                    , item.EffectiveDateFrom
                                    , item.EffectiveDateTo
                                    , item.Reason ?? ""
                                    , item.HRDComments ?? ""
                                    , item.CreatedBy
                                    , item.FromValue ?? ""
                                    , item.FromValueID ?? ""
                                    , item.Status ?? ""
                                    )
                           .AsNoTracking()
                           .ToListAsync()).ToList();
                    spResult.AddRange(sample);
                    await _dbContext.SaveChangesAsync();
                }
                transaction.Commit();

            }
            catch (Exception ex)
            {

            }
            return spResult;
        }

        public async Task<IEnumerable<EmployeeMovement>> SyncMovement()
        {
            return await _dbContext.EmployeeMovement
               .FromSqlRaw(@"CALL sp_sync_employee_movement()").AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeMovement>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.EmployeeMovement.AsNoTracking().Where(x =>
                 (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                 &
                 (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeMovementMapping>> GetMovementType(string MovementType)
        {
            return await _dbContext.EmployeeMovementMapping.AsNoTracking()
                .Where(x => x.MovementType.Equals(MovementType))
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeMovementMapping>> GetEmployeeFieldList(string type)
        {
            return await _dbContext.EmployeeMovementMapping.AsNoTracking()
                .Where(x => x.MovementType.Equals(type, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }

        public async Task<bool> AddMovementEmployeeField(EmployeeMovementMapping param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeMovementMapping.AddAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<EmployeeMovementMapping>> GetEmployeeMovementField(EmployeeFieldForm param)
        {
            return await _dbContext.EmployeeMovementMapping.AsNoTracking()
                .Where(x => x.EmployeeField.Equals(param.EmployeeField)).Where(x => x.MovementType.Equals(param.MovementType))
                .ToListAsync();
        }

        public async Task<bool> BulkRemoveEmployeeField(List<int> IDs)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeMovementMapping.RemoveRange(_dbContext.EmployeeMovementMapping.Where(x => IDs.Contains(x.ID)));

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Delete(int UserID, List<long> IDs)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeMovement
            .Where(p => IDs.Contains(p.ID))
            .ToList()
            .ForEach(x => { x.IsActive = false; x.ModifiedDate = DateTime.Now; x.ModifiedBy = UserID; });

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Update(Form input)
        {
            // 1. Get old value of EmployeeMovement using ID - you can create an SP for this
            // 2. Select equivalent from EmployeeRoving using the result from #1
            // 3. Update EmployeeRoving using the new value after substring-ed value
            var emMovement = _dbContext.EmployeeMovement.Where(x => input.ID == x.ID).FirstOrDefault();
            string olddata_EmployeeField = "";
            string olddata_SecondaryDesig = "";

            bool result = false;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                olddata_EmployeeField = emMovement.EmployeeField;
                olddata_SecondaryDesig = emMovement.To;
                bool z = true;
                if (olddata_EmployeeField.Equals("SECONDARY_DESIG"))
                {
                    _dbContext.EmployeeMovement
                   .Where(p => p.ID == input.ID)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.ModifiedDate = DateTime.Now;
                       x.ModifiedBy = input.CreatedBy;
                       x.Reason = input.Reason;
                       x.DateEffectiveFrom = DateTime.ParseExact(input.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                       x.DateEffectiveTo = string.IsNullOrEmpty(input.EffectiveDateTo) ? (DateTime?)null : DateTime.ParseExact(input.EffectiveDateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                       x.HRDComments = input.HRDComments;
                       x.From = input.FromValue;
                       x.FromID = input.FromValueID;
                       x.To = input.NewValue;
                       x.ToID = input.NewValueID;
                   });

                    _dbContext.EmployeeRoving
                            .Where(p =>
                            p.EmployeeID == input.EmployeeID &&
                            p.OrgGroupID == Int32.Parse(olddata_SecondaryDesig.Substring(0, olddata_SecondaryDesig.IndexOf(','))) &&
                            p.PositionID == Int32.Parse((olddata_SecondaryDesig.Substring(olddata_SecondaryDesig.IndexOf(",") + 1)).Substring(0, olddata_SecondaryDesig.Substring(olddata_SecondaryDesig.IndexOf(",") + 1).IndexOf(",")))
                            //Test added

                            )
                            .ToList()
                            .ForEach(x =>
                        {

                            x.ModifiedDate = DateTime.Now;
                            x.ModifiedBy = input.CreatedBy;
                            x.OrgGroupID = Int32.Parse(input.NewValue.Substring(0, input.NewValue.IndexOf(',')));
                            x.PositionID = Int32.Parse((input.NewValue.Substring(input.NewValue.IndexOf(",") + 1)).Substring(0, input.NewValue.Substring(input.NewValue.IndexOf(",") + 1).IndexOf(",")));
                            x.IsActive = z;

                            if (input.EffectiveDateTo is null)
                            {
                                z = true;
                            }
                            else if (DateTime.ParseExact(input.EffectiveDateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.Now)
                            {
                                z = true;
                            }
                            else
                            {
                                z = false;
                            }

                        });
                }
                else
                {
                    _dbContext.EmployeeMovement
                    .Where(p => p.ID == input.ID)
                    .ToList()
                    .ForEach(x =>
                    {
                        x.ModifiedDate = DateTime.Now;
                        x.ModifiedBy = input.CreatedBy;
                        x.Reason = input.Reason;
                        x.DateEffectiveFrom = DateTime.ParseExact(input.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        x.DateEffectiveTo = string.IsNullOrEmpty(input.EffectiveDateTo) ? (DateTime?)null : DateTime.ParseExact(input.EffectiveDateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        x.HRDComments = input.HRDComments;
                        x.From = input.FromValue;
                        x.FromID = input.FromValueID;
                        x.To = input.NewValue;
                        x.ToID = input.NewValueID;
                        x.Details = input.Details;
                    });
                }

                /*//THIS IS FOR CURRENT DATE
                if (input.UseCurrent)
                {
                    if (input.EmployeeField == "EMPLOYMENT_STATUS")
                    {
                        var employee = _dbContext.Employee.Where(x => x.ID.Equals(input.EmployeeID)).FirstOrDefault();
                        employee.EmploymentStatus = input.NewValueID;

                        if (input.NewValueID.Equals(Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                            input.NewValueID.Equals(Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                            input.NewValueID.Equals(Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                            input.NewValueID.Equals(Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                            input.NewValueID.Equals(Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                            employee.IsActive = false;
                        else
                            employee.IsActive = true;

                        _dbContext.Update(employee);
                    }
                    if(input.EmployeeField == "ORG_GROUP")
                    {
                        var employee = _dbContext.Employee.Where(x => x.ID.Equals(input.EmployeeID)).FirstOrDefault();
                        employee.OrgGroupID = Convert.ToInt32(input.NewValueID);

                        _dbContext.Update(employee);
                    }

                    if (input.EmployeeField == "POSITION")
                    {
                        var employee = _dbContext.Employee.Where(x => x.ID.Equals(input.EmployeeID)).FirstOrDefault();
                        employee.PositionID = Convert.ToInt32(input.NewValueID);

                        _dbContext.Update(employee);
                    }
                }*/

                await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    result = true;
            }
            return result;
        }

        public async Task<bool> PutEmployeeMovement(List<EmployeeMovement> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeMovement.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> PostEmployeeRoving(List<EmployeeRoving> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeRoving.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> PutEmployeeRoving(List<EmployeeRoving> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeRoving.UpdateRange(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> PostEmployeeMovementStatusHistory(List<EmployeeMovementStatusHistory> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeMovementStatusHistory.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }


        public async Task<IEnumerable<EmployeeRoving>> GetEmployeeRoving(int EmployeeID, int OrgID, int PosID)
        {
            return await _dbContext.EmployeeRoving.AsNoTracking()
                .Where(x => x.EmployeeID.Equals(EmployeeID) && x.OrgGroupID.Equals(OrgID) && x.PositionID.Equals(PosID) && x.IsActive)
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeMovementByEmployeeIDs>> GetEmployeeMovementByEmployeeIDs(List<int> EmployeeIDs)
        {
            return await _dbContext.TableVarEmployeeMovementByEmployeeIDs
                .FromSqlRaw(@"CALL sp_get_employee_movement_by_employee_id(
                              {0}
                    )"
                    , string.Join(",",EmployeeIDs) )
               .AsNoTracking()
               .ToListAsync();
        }
    }
}

