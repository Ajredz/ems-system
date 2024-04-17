using APIUtilities;
using EMS_PlantillaService.DBContexts;
using EMS_PlantillaServiceModel.OrganizationalGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;

namespace Plantilla.Controllers
{
    [Route("plantilla/[controller]")]
    [ApiController]
    public class OrganizationalGroupController : ControllerBase
    {
        private readonly PlantillaContext _plantillaContext;

        public OrganizationalGroupController(PlantillaContext plantillaContext)
        {
            _plantillaContext = plantillaContext;
        }
         
        // GET: plantilla/OrganizationalGroup
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            #region Get List
            var OrganizationalGroup = _plantillaContext.OrganizationalGroup
                .AsNoTracking()
                .AsEnumerable();
            #endregion
            return new OkObjectResult(OrganizationalGroup);
        }

        // GET: plantilla/OrganizationalGroup/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(short id)
        {
            #region Get By ID
            var OrganizationalGroup = await _plantillaContext.OrganizationalGroup.FindAsync(id);
            #endregion
            return new OkObjectResult(OrganizationalGroup);
        }

        public async Task<OrganizationalGroup> GetOrganizationalGroupByCodeAsync(string code)
        {
            return await _plantillaContext.OrganizationalGroup
                .AsNoTracking()
                .ToAsyncEnumerable()
                .Where(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        // POST: plantilla/OrganizationalGroup
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationalGroup OrganizationalGroup)
        {
            using (var transaction = _plantillaContext.Database.BeginTransaction())
            {
                #region Insert 
                if (await GetOrganizationalGroupByCodeAsync(OrganizationalGroup.Code) != null)
                {
                    return BadRequest("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                _plantillaContext.Add(OrganizationalGroup);
                await SaveAsync();
                #endregion
                transaction.Commit();
                return CreatedAtAction(nameof(Get), new { id = OrganizationalGroup.ID }, OrganizationalGroup);
            }
        }

        // PUT: plantilla/OrganizationalGroup
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrganizationalGroup OrganizationalGroup)
        {
            if (OrganizationalGroup != null)
            {
                #region Update 
                var _OrganizationalGroup = await GetOrganizationalGroupByCodeAsync(OrganizationalGroup.Code);
                if (_OrganizationalGroup != null)
                {
                    if (_OrganizationalGroup.ID != OrganizationalGroup.ID)
                        return BadRequest("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                using (var transaction = _plantillaContext.Database.BeginTransaction())
                {
                    _plantillaContext.Entry(OrganizationalGroup).State = EntityState.Modified;
                    await SaveAsync();
                    transaction.Commit();
                    return new OkResult();
                }
                #endregion
            }
            return new NoContentResult();
        }

        // DELETE: plantilla/OrganizationalGroup/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            #region Delete 
            var OrganizationalGroup = await _plantillaContext.OrganizationalGroup.FindAsync(id);
            _plantillaContext.OrganizationalGroup.Remove(OrganizationalGroup);
            await SaveAsync();
            #endregion
            return new OkResult();
        }
        public async Task SaveAsync()
        {
            await _plantillaContext.SaveChangesAsync();
        }

        [HttpGet]
        [Route("getorganizationalgroupdropdown")]
        public async Task<IActionResult> GetOrganizationalGroupDropDown([FromQuery] int userid)
        {
            var result = _plantillaContext.OrganizationalGroup
                .AsNoTracking()
                .AsEnumerable();

            List<OrganizationalGroup> OrganizationalGroupList = null;

            if (result != null)
            {
                OrganizationalGroupList = result.Select(x => new OrganizationalGroup { ID = x.ID, Code = x.Code }).ToList();
            }

            return new OkObjectResult(OrganizationalGroupList);
        }
    }
}
