using Dapper;
using EMS.Plantilla.Data.Position;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.H2Pay.Data.Position
{
    public interface IDBAccess
    {
        Task<int> Sync(SharedUtilities.UNIT_OF_TIME unit, int value);
    }

    public class DBAccess : Utilities, IDBAccess
    {

        private readonly IPositionDBAccess _dbAccess;

        public DBAccess(IConfiguration iconfiguration, IPositionDBAccess dbAccess) : base(iconfiguration) {
            _dbAccess = dbAccess;
        }

        public async Task<int> Sync(SharedUtilities.UNIT_OF_TIME unit, int value)
        {

            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            var TableVar = (await _dbAccess.GetLastModified(From, To))
                .Select(x => new EMS.H2Pay.Data.Position.tblTypePositionSync
                { 
                    Code = x.Code,
                    Description = x.Title,
                    IsActive = x.IsActive
                }).ToList()
                .ToDataTable();
    
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                int affectedRows = 0;

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {

                    affectedRows = await connection.ExecuteAsync("spPositionSync", new { @TableVar = TableVar }, 
                        commandType: CommandType.StoredProcedure, 
                        transaction: transaction);
                    transaction.Commit();
                }

                return affectedRows;
            }
        }

    }

}
