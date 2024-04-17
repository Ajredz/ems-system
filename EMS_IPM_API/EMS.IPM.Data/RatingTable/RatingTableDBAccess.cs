using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.RatingTable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.RatingTable
{
    public interface IRatingTableDBAccess
    {
        Task<IEnumerable<RatingTable>> GetAll();
    }

    public class RatingTableDBAccess : IRatingTableDBAccess
    {
        private readonly IPMContext _dbContext;

        public RatingTableDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<RatingTable>> GetAll()
        {
            return await _dbContext.RatingTable.AsNoTracking().ToListAsync();
        }

    }
}