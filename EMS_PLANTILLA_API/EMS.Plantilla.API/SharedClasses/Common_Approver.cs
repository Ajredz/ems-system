using EMS_PlantillaServiceModel.NPRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_PlantillaService.SharedClasses
{
    public class Common_Approver
    {

        /// <summary>
        /// Get records that exists in database and is missing from the updated form.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<NPRFApprover> GetApproversToDelete(List<NPRFApprover> left, List<NPRFApprover> right)
        {
            return left.GroupJoin(
                    right,
                    x => new { x.NPRFID, x.NPRFSignatoriesID },
                    y => new { y.NPRFID, y.NPRFSignatoriesID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .SelectMany(x => x.newSet.DefaultIfEmpty(),
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.newSet == null)
                    .Select(x =>
                        new NPRFApprover
                        {
                            ID = x.oldSet.oldSet.ID,
                            NPRFID = x.oldSet.oldSet.NPRFID,
                            NPRFSignatoriesID = x.oldSet.oldSet.NPRFSignatoriesID,
                            Status = x.oldSet.oldSet.Status,
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
        public IEnumerable<NPRFApprover> GetApproversToAdd(List<NPRFApprover> left, List<NPRFApprover> right, NPRFHeader NPRFHeader)
        {
            return right.GroupJoin(
               left,
                    x => new { x.NPRFID, x.NPRFSignatoriesID},
                    y => new { y.NPRFID, y.NPRFSignatoriesID},
               (x, y) => new { newSet = x, oldSet = y })
               .SelectMany(x => x.oldSet.DefaultIfEmpty(),
               (x, y) => new { newSet = x, oldSet = y })
               .Where(x => x.oldSet == null)
               .Select(x =>
                   new NPRFApprover
                   {
                       NPRFID = NPRFHeader.ID,
                       NPRFSignatoriesID = x.newSet.newSet.NPRFSignatoriesID,
                       Status = x.newSet.newSet.Status,
                   }).ToList();
        }

        /// <summary>
        /// Get records with the same PartNumber and ClaimsId but with different 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="NPRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<NPRFApprover> GetApproversToUpdate(List<NPRFApprover> left, List<NPRFApprover> right, NPRFHeader NPRFHeader)
        {
            return left.Join(
                    right,
                    x => new { x.NPRFID, x.NPRFSignatoriesID },
                    y => new { y.NPRFID, y.NPRFSignatoriesID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.Status.Equals(x.newSet.Status))
                    .Select(y => new NPRFApprover
                    {
                        ID = y.oldSet.ID,
                        NPRFID = NPRFHeader.ID,
                        NPRFSignatoriesID = y.newSet.NPRFSignatoriesID,
                        Status = y.newSet.Status,
                        ModifiedBy = NPRFHeader.ModifiedBy.Value,
                        ModifiedDate = DateTime.Now,
                    }).ToList();
        }
    }
}
