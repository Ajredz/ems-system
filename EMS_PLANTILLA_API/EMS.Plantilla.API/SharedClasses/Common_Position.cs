using EMS_PlantillaServiceModel.NPRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_PlantillaService.SharedClasses
{
    public class Common_Position
    {

        /// <summary>
        /// Get records that exists in database and is missing from the updated form.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<NPRFPosition> GetPositionsToDelete(List<NPRFPosition> left, List<NPRFPosition> right)
        {
            return left.GroupJoin(
                    right,
                    x => new { x.NPRFID, x.PositionID, x.PositionLevelID },
                    y => new { y.NPRFID, y.PositionID, y.PositionLevelID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .SelectMany(x => x.newSet.DefaultIfEmpty(),
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.newSet == null)
                    .Select(x =>
                        new NPRFPosition
                        {
                            ID = x.oldSet.oldSet.ID,
                            NPRFID = x.oldSet.oldSet.NPRFID,
                            PositionLevelID = x.oldSet.oldSet.PositionLevelID,
                            PositionID = x.oldSet.oldSet.PositionID,
                            HeadcoungExisting = x.oldSet.oldSet.HeadcoungExisting,
                            CreatedBy = x.oldSet.oldSet.CreatedBy,
                            CreatedDate = x.oldSet.oldSet.CreatedDate,
                            ModifiedDate = x.oldSet.oldSet.ModifiedDate,
                            ModifiedBy = x.oldSet.oldSet.ModifiedBy,
                        }).ToList();
        }

        /// <summary>
        /// Get records that are added on the form and doesn't exists in database.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="NPRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<NPRFPosition> GetPositionsToAdd(List<NPRFPosition> left, List<NPRFPosition> right, NPRFHeader NPRFHeader)
        {
            return right.GroupJoin(
               left,
                    x => new { x.NPRFID, x.PositionID, x.PositionLevelID },
                    y => new { y.NPRFID, y.PositionID, y.PositionLevelID },
               (x, y) => new { newSet = x, oldSet = y })
               .SelectMany(x => x.oldSet.DefaultIfEmpty(),
               (x, y) => new { newSet = x, oldSet = y })
               .Where(x => x.oldSet == null)
               .Select(x =>
                   new NPRFPosition
                   {
                       NPRFID = NPRFHeader.ID,
                       PositionLevelID = x.newSet.newSet.PositionLevelID,
                       PositionID = x.newSet.newSet.PositionID,
                       HeadcoungExisting = x.newSet.newSet.HeadcoungExisting,
                       CreatedBy = NPRFHeader.ModifiedBy.Value,
                   }).ToList();
        }

        /// <summary>
        /// Get records with the same PartNumber and ClaimsId but with different 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="NPRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<NPRFPosition> GetPositionsToUpdate(List<NPRFPosition> left, List<NPRFPosition> right, NPRFHeader NPRFHeader)
        {
            return left.Join(
                    right,
                    x => new { x.NPRFID, x.PositionID, x.PositionLevelID },
                    y => new { y.NPRFID, y.PositionID, y.PositionLevelID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.HeadcoungExisting.Equals(x.newSet.HeadcoungExisting))
                    .Select(y => new NPRFPosition
                    {
                        ID = y.oldSet.ID,
                        NPRFID = NPRFHeader.ID,
                        PositionLevelID = y.newSet.PositionLevelID,
                        PositionID = y.newSet.PositionID,
                        HeadcoungExisting = y.newSet.HeadcoungExisting,
                        CreatedBy = y.newSet.CreatedBy,
                        CreatedDate = y.newSet.CreatedDate,
                        ModifiedBy = NPRFHeader.ModifiedBy.Value,
                        ModifiedDate = DateTime.Now,
                    }).ToList();
        }
    }
}
