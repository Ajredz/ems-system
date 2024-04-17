using EMS.Common.Data.CompanyDatabase;
using EMS.Common.Data.DBContexts;
using EMS.Common.Transfer;
using EMS.Common.Transfer.CompanyDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Common.Core.CompanyDatabase
{
    public interface ICompanyDatabaseService
    {
        Task<IActionResult> GetByCompanyID(APICredentials credentials, string CompanyID);
        Task<IActionResult> GetByCompanyCode(APICredentials credentials, string CompanyCode);
        Task<IActionResult> GetByModuleCode(APICredentials credentials, string ModuleCode);
        Task<IActionResult> GetAll(APICredentials credentials);
    }

    public class CompanyDatabaseService : Core.Shared.Utilities, ICompanyDatabaseService
    {
        private readonly IDBAccess _dbAccess;


        public CompanyDatabaseService(SystemAccessContext dbContext, IConfiguration iconfiguration,
            IDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetByCompanyID(APICredentials credentials, string CompanyID)
        {
            //return new OkObjectResult(
            //    (await _dbAccess.GetByCompanyCode(CompanyID))
            //    .Select(x => new GetByCompanyCodeOutput
            //    {
            //       CompanyCode = x.CompanyCode,
            //        CompanyID = x.CompanyID,
            //        ModuleCode = x.ModuleCode,
            //       ConnectionString = x.ConnectionString
            //    }));

            var result = await _dbAccess.GetByCompanyID(CompanyID);

            List<GetAllOutput> getCompanyOutput = new List<GetAllOutput>();

            if (result != null)
            {
                getCompanyOutput = result.Select(x => new GetAllOutput
                {
                    CompanyID = x.CompanyID,
                    CompanyCode = x.CompanyCode,
                    ModuleCode = x.ModuleCode,
                    EncryptedConnectionString = x.ConnectionString
                }).ToList();

                return new OkObjectResult(getCompanyOutput);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }

        }

        public async Task<IActionResult> GetByCompanyCode(APICredentials credentials, string CompanyCode)
        {
            //return new OkObjectResult(
            //    (await _dbAccess.GetByCompanyCode(CompanyCode))
            //    .Select(x => new GetByCompanyCodeOutput
            //    {
            //       CompanyCode = x.CompanyCode,
            //        CompanyID = x.CompanyID,
            //        ModuleCode = x.ModuleCode,
            //       ConnectionString = x.ConnectionString
            //    }));

            var result = await _dbAccess.GetByCompanyCode(CompanyCode);

            List<GetAllOutput> getCompanyOutput = new List<GetAllOutput>();

            if (result != null)
            {
                getCompanyOutput = result.Select(x => new GetAllOutput
                {
                    CompanyID = x.CompanyID,
                    CompanyCode = x.CompanyCode,
                    ModuleCode = x.ModuleCode,
                    EncryptedConnectionString = x.ConnectionString
                }).ToList();

                return new OkObjectResult(getCompanyOutput);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }

        }
        
        public async Task<IActionResult> GetByModuleCode(APICredentials credentials, string ModuleCode)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByModuleCode(ModuleCode))
                .Select(x => new GetByModuleCodeOutput
                {
                   CompanyCode = x.CompanyCode,
                   CompanyID = x.CompanyID,
                   CompanyName = x.CompanyName,
                   ModuleCode = x.ModuleCode,
                   ConnectionString = x.ConnectionString
                }));
        }

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            //return new OkObjectResult(
            //    (await _dbAccess.GetAll())
            //    .Select(x => new GetAllOutput
            //    {
            //        CompanyCode = x.CompanyCode,
            //        CompanyID = x.CompanyID,
            //        ModuleCode = x.ModuleCode,
            //        ConnectionString = x.ConnectionString
            //    }));

            var result = await _dbAccess.GetAll();

            List<GetAllOutput> getAllOutput = new List<GetAllOutput>();

            if (result != null)
            {
                getAllOutput = result.Select(x => new GetAllOutput
                {
                    CompanyID = x.CompanyID,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    ModuleCode = x.ModuleCode,
                    EncryptedConnectionString = x.ConnectionString
                }).ToList() ;

                return new OkObjectResult(getAllOutput);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INCORRECT_LOGIN);
            }

        }

    }
}
