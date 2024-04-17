using EMS_ManPowerServiceModel.MRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_ManPowerService.SharedClasses
{
    public class Common_Approver
    {

        /// <summary>
        /// Get records that exists in database and is missing from the updated form.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<MRFApprover> GetApproversToDelete(List<MRFApprover> left, List<MRFApprover> right)
        {
            return left.GroupJoin(
                    right,
                    x => new { x.MRFID, x.MRFSignatoriesID },
                    y => new { y.MRFID, y.MRFSignatoriesID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .SelectMany(x => x.newSet.DefaultIfEmpty(),
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.newSet == null)
                    .Select(x =>
                        new MRFApprover
                        {
                            ID = x.oldSet.oldSet.ID,
                            MRFID = x.oldSet.oldSet.MRFID,
                            MRFSignatoriesID = x.oldSet.oldSet.MRFSignatoriesID,
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
        /// <param name="MRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<MRFApprover> GetApproversToAdd(List<MRFApprover> left, List<MRFApprover> right, MRFHeader MRFHeader)
        {
            return right.GroupJoin(
               left,
                    x => new { x.MRFID, x.MRFSignatoriesID },
                    y => new { y.MRFID, y.MRFSignatoriesID },
               (x, y) => new { newSet = x, oldSet = y })
               .SelectMany(x => x.oldSet.DefaultIfEmpty(),
               (x, y) => new { newSet = x, oldSet = y })
               .Where(x => x.oldSet == null)
               .Select(x =>
                   new MRFApprover
                   {
                       MRFID = MRFHeader.ID,
                       MRFSignatoriesID = x.newSet.newSet.MRFSignatoriesID,
                       Status = x.newSet.newSet.Status,
                   }).ToList();
        }

        /// <summary>
        /// Get records with the same PartNumber and ClaimsId but with different 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="MRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<MRFApprover> GetApproversToUpdate(List<MRFApprover> left, List<MRFApprover> right, MRFHeader MRFHeader)
        {
            return left.Join(
                    right,
                    x => new { x.MRFID, x.MRFSignatoriesID },
                    y => new { y.MRFID, y.MRFSignatoriesID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.Status.Equals(x.newSet.Status))
                    .Select(y => new MRFApprover
                    {
                        ID = y.oldSet.ID,
                        MRFID = MRFHeader.ID,
                        MRFSignatoriesID = y.newSet.MRFSignatoriesID,
                        Status = y.newSet.Status,
                        ModifiedBy = MRFHeader.ModifiedBy.Value,
                        ModifiedDate = DateTime.Now,
                    }).ToList();
        }
    }
}
