using EMS.IPM.Data.DataDuplication.Employee;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.DataDuplication
{
    public interface IEmployeeService
    {
        Task<IActionResult> Sync(List<Employee> param);
        
        Task<IActionResult> SyncRoving(List<EmployeeRoving> param);

        Task<List<Employee>> GetBySyncIDs(List<int> IDs);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetFilteredIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);
    }

    public class EmployeeService : Core.Shared.Utilities, IEmployeeService
    {
        private readonly IEmployeeDBAccess _dbAccess;

        public EmployeeService(IPMContext dbContext, IConfiguration iconfiguration,
            IEmployeeDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Sync(List<Employee> param)
        {
            static List<Employee> GetToAdd(List<Employee> left, List<Employee> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { SyncID = x.ID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Employee
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        Code = x.newSet.newSet.Code,
                        OldEmployeeID = x.newSet.newSet.OldEmployeeID,
                        FirstName = x.newSet.newSet.FirstName,
                        MiddleName = x.newSet.newSet.MiddleName,
                        LastName = x.newSet.newSet.LastName,
                        Suffix = x.newSet.newSet.Suffix ?? String.Empty,
                        Nickname = x.newSet.newSet.Nickname ?? String.Empty,
                        OrgGroupID = x.newSet.newSet.OrgGroupID,
                        PositionID = x.newSet.newSet.PositionID,
                        //PSGCRegion = x.newSet.newSet.PSGCRegion,
                        //PSGCCity = x.newSet.newSet.PSGCCity,
                        Gender = x.newSet.newSet.Gender ?? String.Empty,
                        EmploymentStatus = x.newSet.newSet.EmploymentStatus ?? String.Empty,
                        DateHired = x.newSet.newSet.DateHired,
                        SystemUserID = x.newSet.newSet.SystemUserID,
                        IsActive = x.newSet.newSet.IsActive,
                        CreatedDate = x.newSet.newSet.CreatedDate,
                    })
                .ToList();
            }

            static List<Employee> GetToUpdate(List<Employee> left, List<Employee> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                    || !x.oldSet.OldEmployeeID.Equals(x.newSet.OldEmployeeID)
                    || !x.oldSet.FirstName.Equals(x.newSet.FirstName)
                    || !x.oldSet.MiddleName.Equals(x.newSet.MiddleName)
                    || !x.oldSet.LastName.Equals(x.newSet.LastName)
                    || !(x.oldSet.Suffix ?? String.Empty).Equals(x.newSet.Suffix)
                    || !(x.oldSet.Nickname ?? String.Empty).Equals(x.newSet.Nickname)
                    || x.oldSet.OrgGroupID != x.newSet.OrgGroupID
                    || x.oldSet.PositionID != x.newSet.PositionID
                    //|| x.oldSet.PSGCRegion != x.newSet.PSGCRegion
                    //|| x.oldSet.PSGCCity != x.newSet.PSGCCity
                    || !x.oldSet.Gender.Equals(x.newSet.Gender ?? String.Empty)
                    || !(x.oldSet.EmploymentStatus ?? "").Equals(x.newSet.EmploymentStatus ?? String.Empty)
                    || x.oldSet.DateHired != x.newSet.DateHired
                    || x.oldSet.SystemUserID != x.newSet.SystemUserID
                    || x.oldSet.IsActive != x.newSet.IsActive
                    || x.oldSet.CreatedDate != x.newSet.CreatedDate
                )
                .Select(y => new Employee
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    Code = y.newSet.Code,
                    OldEmployeeID = y.newSet.OldEmployeeID,
                    FirstName = y.newSet.FirstName,
                    MiddleName = y.newSet.MiddleName,
                    LastName = y.newSet.LastName,
                    Suffix = y.newSet.Suffix,
                    Nickname = y.newSet.Nickname,
                    OrgGroupID = y.newSet.OrgGroupID,
                    PositionID = y.newSet.PositionID,
                    //PSGCRegion = y.newSet.PSGCRegion,
                    //PSGCCity = y.newSet.PSGCCity,
                    Gender = y.newSet.Gender,
                    EmploymentStatus = y.newSet.EmploymentStatus,
                    DateHired = y.newSet.DateHired,
                    SystemUserID = y.newSet.SystemUserID,
                    IsActive = y.newSet.IsActive,
                    CreatedDate = y.newSet.CreatedDate
                })
                .ToList();
            }

            List<Employee> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<Employee> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<Employee> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<Employee>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added Employee record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated Employee record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("Employee ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<IActionResult> SyncRoving(List<EmployeeRoving> param)
        {
            static List<EmployeeRoving> GetToAdd(List<EmployeeRoving> left, List<EmployeeRoving> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { SyncID = x.ID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new EmployeeRoving
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        EmployeeID = x.newSet.newSet.EmployeeID,
                        OrgGroupID = x.newSet.newSet.OrgGroupID,
                        PositionID = x.newSet.newSet.PositionID,
                        IsActive = x.newSet.newSet.IsActive,
                        CreatedBy = x.newSet.newSet.CreatedBy,
                        CreatedDate = x.newSet.newSet.CreatedDate,
                        ModifiedBy = x.newSet.newSet.ModifiedBy,
                        ModifiedDate = x.newSet.newSet.ModifiedDate
                    })
                .ToList();
            }

            static List<EmployeeRoving> GetToUpdate(List<EmployeeRoving> left, List<EmployeeRoving> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.EmployeeID != x.newSet.EmployeeID
                    || x.oldSet.OrgGroupID != x.newSet.OrgGroupID
                    || x.oldSet.PositionID != x.newSet.PositionID
                    || x.oldSet.IsActive != x.newSet.IsActive
                    || x.oldSet.CreatedDate != x.newSet.CreatedDate
                    || x.oldSet.ModifiedDate != x.newSet.ModifiedDate
                )
                .Select(y => new EmployeeRoving
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    EmployeeID = y.newSet.EmployeeID,
                    OrgGroupID = y.newSet.OrgGroupID,
                    PositionID = y.newSet.PositionID,
                    IsActive = y.newSet.IsActive,
                    CreatedBy = y.newSet.CreatedBy,
                    CreatedDate = y.newSet.CreatedDate,
                    ModifiedBy = y.newSet.ModifiedBy,
                    ModifiedDate = y.newSet.ModifiedDate
                })
                .ToList();
            }

            List<EmployeeRoving> localCopy = (await _dbAccess.GetRovingBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<EmployeeRoving> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<EmployeeRoving> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.SyncRoving(new List<EmployeeRoving>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added Employee Roving record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated Employee Roving record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("EmployeeRoving ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<Employee>> GetBySyncIDs(List<int> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "Code", "Title")
                );
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.SyncID,
                    Description = string.Concat(x.LastName, ", ", x.FirstName, " ", x.MiddleName, x.Suffix == null ? "" : x.Suffix, " (", x.Code, ")")
                })
            );
        }

        public async Task<IActionResult> GetFilteredIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetFilteredIDByAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.SyncID,
                    Description = string.Concat(x.LastName, ", ", x.FirstName, " ", x.MiddleName, " (", x.Code, ")")
                })
            );
        }
    }
}