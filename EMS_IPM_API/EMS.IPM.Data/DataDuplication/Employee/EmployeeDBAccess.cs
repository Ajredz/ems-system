using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.Employee;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.DataDuplication.Employee
{
    public interface IEmployeeDBAccess
    {
        Task<IEnumerable<Employee>> GetBySyncIDs(List<int> IDs);
        Task<IEnumerable<EmployeeRoving>> GetRovingBySyncIDs(List<int> IDs);

        Task<IEnumerable<Employee>> GetAutoComplete(GetAutoCompleteInput param);

        Task<bool> Sync(List<Employee> toDelete,
            List<Employee> toAdd,
            List<Employee> toUpdate);

        Task<bool> SyncRoving(List<EmployeeRoving> toDelete,
            List<EmployeeRoving> toAdd,
            List<EmployeeRoving> toUpdate);

        Task<IEnumerable<Employee>> GetAll();

        Task<IEnumerable<Employee>> GetFilteredIDByAutoComplete(GetAutoCompleteInput param);
    }

    public class EmployeeDBAccess : IEmployeeDBAccess
    {
        private readonly IPMContext _dbContext;

        public EmployeeDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Sync(List<Employee> toDelete,
            List<Employee> toAdd,
            List<Employee> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.Employee.RemoveRange(toDelete);
                _dbContext.Employee.AddRange(toAdd);
                toUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }
        
        public async Task<bool> SyncRoving(List<EmployeeRoving> toDelete,
            List<EmployeeRoving> toAdd,
            List<EmployeeRoving> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.EmployeeRoving.RemoveRange(toDelete);
                _dbContext.EmployeeRoving.AddRange(toAdd);
                toUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<Employee>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.Employee.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeRoving>> GetRovingBySyncIDs(List<int> IDs)
        {
            return await _dbContext.EmployeeRoving.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _dbContext.Employee.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Employee
                .FromSqlRaw("CALL sp_employee_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetFilteredIDByAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Employee
                .FromSqlRaw("CALL sp_filtered_employee_autocomplete({0},{1},{2},{3})", (param.Term ?? ""), param.TopResults, (param.Filter ?? ""), (param.FilterID ?? ""))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}