using APIUtilities;
using EMS_PlantillaService.DBContexts;
using EMS_PlantillaServiceModel.NPRF;
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
    public class NPRFController : ControllerBase
    {
        private readonly PlantillaContext _plantillaContext;

        public NPRFController(PlantillaContext plantillaContext)
        {
            _plantillaContext = plantillaContext;
        }

        // GET: plantilla/nprf
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            #region Get List
            var nprf = _plantillaContext.NPRFHeader
                .AsNoTracking()
                .AsEnumerable();
            #endregion

            return new OkObjectResult(nprf);
        }

        // GET: plantilla/nprf/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(short id)
        {
            #region Get By ID
            var nprfHeader = await _plantillaContext.NPRFHeader.FindAsync(id);
            var nprfPositionList = await _plantillaContext.NPRFPosition.AsNoTracking().ToAsyncEnumerable().Where(x => x.NPRFID == nprfHeader.ID).ToList();
            var nprfApproverList = await _plantillaContext.NPRFApprover.AsNoTracking().ToAsyncEnumerable().Where(x => x.NPRFID == nprfHeader.ID).ToList();

            var nprf = new NPRF()
            {
                NPRFHeader = nprfHeader,
                NPRFPositionList = nprfPositionList,
                NPRFApproverList = nprfApproverList,
            };
            #endregion

            return new OkObjectResult(nprf);
        }

        // POST: plantilla/nprf
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NPRF nprf)
        {
            using (var transaction = _plantillaContext.Database.BeginTransaction())
            {
                #region Insert NPRF
                _plantillaContext.NPRFHeader.Add(nprf.NPRFHeader);

                await _plantillaContext.SaveChangesAsync();

                //Add NPRF Position
                if (nprf.NPRFPositionList != null)
                {
                    nprf.NPRFPositionList.Select(x =>
                    {
                        x.NPRFID = nprf.NPRFHeader.ID;
                        return x;
                    }).ToList();

                    _plantillaContext.NPRFPosition.AddRange(nprf.NPRFPositionList);
                }

                //Add NPRF Position
                if (nprf.NPRFApproverList != null)
                {
                    nprf.NPRFApproverList.Select(x =>
                    {
                        x.NPRFID = nprf.NPRFHeader.ID;
                        return x;
                    }).ToList();

                    _plantillaContext.AddRange(nprf.NPRFApproverList);
                }

                await _plantillaContext.SaveChangesAsync();
                #endregion

                transaction.Commit();
                return CreatedAtAction(nameof(Get), new { id = nprf.NPRFHeader.ID }, nprf.NPRFHeader);
            }
        }

        // PUT: plantilla/nprf
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NPRF nprf)
        {
            if (nprf != null)
            {
                using (var transaction = _plantillaContext.Database.BeginTransaction())
                {
                    #region Update 
                    _plantillaContext.Entry(nprf.NPRFHeader).State = EntityState.Modified;

                    #region NPRF Position Query
                    var oldnprfPositionList = await _plantillaContext.NPRFPosition.AsNoTracking().ToAsyncEnumerable().Where(x => x.NPRFID == nprf.NPRFHeader.ID).ToList();

                    List<NPRFPosition> PositionsToDelete = new EMS_PlantillaService.SharedClasses.Common_Position().GetPositionsToDelete(oldnprfPositionList, nprf.NPRFPositionList).ToList();
                    List<NPRFPosition> PositionsToAdd = new EMS_PlantillaService.SharedClasses.Common_Position().GetPositionsToAdd(oldnprfPositionList, nprf.NPRFPositionList, nprf.NPRFHeader).ToList();
                    List<NPRFPosition> PositionsToUpdate = new EMS_PlantillaService.SharedClasses.Common_Position().GetPositionsToUpdate(oldnprfPositionList, nprf.NPRFPositionList, nprf.NPRFHeader).ToList();

                    // Execute filtered records to their respective actions.
                    _plantillaContext.RemoveRange(PositionsToDelete);
                    _plantillaContext.AddRange(PositionsToAdd);
                    // Update query.
                    _plantillaContext.NPRFPosition.ToList().Join(
                        PositionsToUpdate,
                        x => new { x.NPRFID, x.PositionID, x.PositionLevelID },
                        y => new { y.NPRFID, y.PositionID, y.PositionLevelID },
                        (x, y) => {

                            x.ID = x.ID;
                            x.NPRFID = x.NPRFID;
                            x.PositionLevelID = x.PositionLevelID;
                            x.PositionID = x.PositionID;
                            x.HeadcoungExisting = y.HeadcoungExisting;
                            x.CreatedBy = x.CreatedBy;
                            x.CreatedDate = x.CreatedDate;
                            x.ModifiedBy = y.ModifiedBy;
                            x.ModifiedDate = y.ModifiedDate;
                            return x;
                        }).ToList();
                    #endregion

                    #region NPRF Approver Query
                    var oldnprfApproverList = await _plantillaContext.NPRFApprover.AsNoTracking().ToAsyncEnumerable().Where(x => x.NPRFID == nprf.NPRFHeader.ID).ToList();

                    List<NPRFApprover> ApproversToDelete = new EMS_PlantillaService.SharedClasses.Common_Approver().GetApproversToDelete(oldnprfApproverList, nprf.NPRFApproverList).ToList();
                    List<NPRFApprover> ApproversToAdd = new EMS_PlantillaService.SharedClasses.Common_Approver().GetApproversToAdd(oldnprfApproverList, nprf.NPRFApproverList, nprf.NPRFHeader).ToList();
                    List<NPRFApprover> ApproversToUpdate = new EMS_PlantillaService.SharedClasses.Common_Approver().GetApproversToUpdate(oldnprfApproverList, nprf.NPRFApproverList, nprf.NPRFHeader).ToList();

                    // Execute filtered records to their respective actions.
                    _plantillaContext.RemoveRange(ApproversToDelete);
                    _plantillaContext.AddRange(ApproversToAdd);
                    // Update query.
                    _plantillaContext.NPRFApprover.ToList().Join(
                        ApproversToUpdate,
                        x => new { x.NPRFID, x.NPRFSignatoriesID },
                        y => new { y.NPRFID, y.NPRFSignatoriesID },
                        (x, y) => {

                            x.ID = x.ID;
                            x.NPRFID = x.NPRFID;
                            x.NPRFSignatoriesID = x.NPRFSignatoriesID;
                            x.Status = y.Status;
                            x.ModifiedBy = y.ModifiedBy;
                            x.ModifiedDate = y.ModifiedDate;
                            return x;
                        }).ToList();
                    #endregion

                    await _plantillaContext.SaveChangesAsync();
                    #endregion
                    transaction.Commit();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        // DELETE: plantilla/nprf/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            #region Delete
            var nprf = await _plantillaContext.NPRFHeader.FindAsync(id);
            var nprfPositionList = _plantillaContext.NPRFPosition.Where(x => x.NPRFID == nprf.ID).AsEnumerable();
            if (nprfPositionList != null && nprfPositionList.Count() != 0)
            {
                _plantillaContext.NPRFPosition.RemoveRange(nprfPositionList);
            }

            var nprfApproverList = _plantillaContext.NPRFApprover.Where(x => x.NPRFID == nprf.ID).AsEnumerable();
            if (nprfApproverList != null && nprfApproverList.Count() != 0)
            {
                _plantillaContext.NPRFApprover.RemoveRange(nprfApproverList);
            }

            _plantillaContext.NPRFHeader.Remove(nprf);
            await _plantillaContext.SaveChangesAsync();
            #endregion
            return new OkResult();
        }
    }
}
