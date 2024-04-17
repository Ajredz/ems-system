using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS_SecurityService.DBContexts;
using EMS_SecurityServiceModel.SystemURL;
using Microsoft.Extensions.Configuration;
using EMS_SecurityService.SharedClasses;
using Utilities.API;

namespace EMS_SecurityService.Controllers
{
    [Route("security/[controller]")]
    [ApiController]
    public class SystemURLController : SharedClasses.Utilities
    {
        public SystemURLController(SystemAccessContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
        }


        [HttpGet]
        [Route("geturl")]
        public async Task<IActionResult> GetSystemURL([FromQuery] int userid)
        {
            _userID = userid;
            var SystemPage = await _dbContext.SystemURL.AsNoTracking()
                .Join(_dbContext.SystemPage.AsNoTracking().Where(x => !x.IsHidden), 
                    x => x.PageID,
                    y => y.ID, 
                    (x, y) => new { x, y })
                .Join(_dbContext.SystemRolePage.AsNoTracking().Where(x => !x.IsHidden), 
                    x => new { x.x.PageID, x.x.FunctionType },  
                    y => new { y.PageID, y.FunctionType }, 
                    (x, y) => new { x, y })
                .Join(_dbContext.SystemUserRole.AsNoTracking(), 
                    x => x.y.RoleID, 
                    y => y.RoleID, 
                    (x, y) => new { x, y })
                .Where(x => x.y.UserID == userid)
                .Select(x => new SystemURL {
                    PageID = x.x.x.x.PageID,
                    URL = x.x.x.x.URL ?? ""
                })
                .Union(
                    _dbContext.SystemPage.AsNoTracking().Where(x => !x.IsHidden)
                    .Join(_dbContext.SystemRolePage.AsNoTracking().Where(x => !x.IsHidden), 
                        x => x.ID, 
                        y => y.PageID, 
                        (x, y) => new { x, y })
                    .Join(_dbContext.SystemUserRole.AsNoTracking(), 
                        x => x.y.RoleID, 
                        y => y.RoleID, 
                        (x, y) => new { x, y })
                    .Where(x => x.y.UserID == userid && 
                    !string.IsNullOrEmpty(x.x.x.URL) && 
                    !x.x.y.IsHidden)
                    .Select(x => new SystemURL {
                        PageID = x.x.x.ID,
                        URL = x.x.x.URL ?? ""
                    })
                ).Distinct().ToListAsync();

            if (SystemPage.Count > 0)
                return new OkObjectResult(SystemPage);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
        }


    }
}
