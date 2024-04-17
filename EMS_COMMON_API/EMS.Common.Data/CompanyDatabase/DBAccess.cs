using EMS.Common.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API; 

namespace EMS.Common.Data.CompanyDatabase
{
    public interface IDBAccess
    {
        Task<IEnumerable<vwCompanyDatabase>> GetByCompanyID(string CompanyID);
        Task<IEnumerable<vwCompanyDatabase>> GetByCompanyCode(string CompanyCode);
        Task<IEnumerable<vwCompanyDatabase>> GetByModuleCode(string ModuleCode);
        Task<IEnumerable<vwCompanyDatabase>> GetAll();
    }

    public class DBAccess : IDBAccess
    {

        private readonly SystemAccessContext _dbContext;
        
        public DBAccess(SystemAccessContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<vwCompanyDatabase>> GetByCompanyID(string CompanyID)
        {
            //return await _dbContext.vwCompanyDatabase.AsNoTracking()
            //    .Where(x => x.CompanyId.Equals(CompanyCode))
            //    .ToListAsync();

            List<vwCompanyDatabase> result = new List<vwCompanyDatabase>();
            try
            {
                result = await _dbContext.vwCompanyDatabase.AsNoTracking()
                    .Where(x => x.CompanyID.Equals(CompanyID))
                    .ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<IEnumerable<vwCompanyDatabase>> GetByCompanyCode(string CompanyCode)
        {
            //return await _dbContext.vwCompanyDatabase.AsNoTracking()
            //    .Where(x => x.CompanyCode.Equals(CompanyCode))
            //    .ToListAsync();

            List<vwCompanyDatabase> result = new List<vwCompanyDatabase>();
            try
            {
                result = await _dbContext.vwCompanyDatabase.AsNoTracking()
                    .Where(x => x.CompanyCode.Equals(CompanyCode))
                    .ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<IEnumerable<vwCompanyDatabase>> GetByModuleCode(string ModuleCode)
        {
            return await _dbContext.vwCompanyDatabase.AsNoTracking()
                .Where(x => x.ModuleCode.Equals(ModuleCode))
                .ToListAsync();
        }

        public async Task<IEnumerable<vwCompanyDatabase>> GetAll()
        {
            return await _dbContext.vwCompanyDatabase.AsNoTracking().ToListAsync();

            //List<vwCompanyDatabase> result = new List<vwCompanyDatabase>();
            //try 
            //{
            //    result = await _dbContext.vwCompanyDatabase.AsNoTracking().ToListAsync();
            //} 
            //catch (Exception ex) 
            //{ 

            //}
            //return result;
        }
    }
}