using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.H2Pay.Data.SystemUser
{
    public interface IDBAccess
    {
        Task<IEnumerable<spSystemUserGetAllSync_Result>> GetSync(SharedUtilities.UNIT_OF_TIME unit, int value);
    }

    public class DBAccess : Utilities, IDBAccess
    {

        public DBAccess(IConfiguration iconfiguration) : base(iconfiguration) {}

        public async Task<IEnumerable<spSystemUserGetAllSync_Result>> GetSync(SharedUtilities.UNIT_OF_TIME unit, int value)
        {

            var (From, To) = SharedUtilities.GetDateRange(unit, value);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                
                connection.Open();
                
                return await connection.QueryAsync<spSystemUserGetAllSync_Result>("spSystemUserGetAllSync"
                        , new { @DateFrom = From, @DateTo = To },
                        commandType: CommandType.StoredProcedure);

            }
        }

    }

}
