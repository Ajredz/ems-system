using EMS_ManPowerServiceModel.MRF;
using EMS_ManPowerServiceModel.MRFSignatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_ManPowerService.SharedClasses
{
    public class Common_Signatories
    {

        /// <summary>
        /// Get records that exists in database and is missing from the updated form.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<MRFSignatories> GetSignatoriesToDelete(List<MRFSignatories> left, List<MRFSignatories> right)
        {
            if (right == null || right.Count == 0)
                return left;
            else
                return left.GroupJoin(
                    right,
                    x => new { x.UserId, x.PositionId, x.ApproverId },
                    y => new { y.UserId, y.PositionId, y.ApproverId },
                    (x, y) => new { oldSet = x, newSet = y })
                    .SelectMany(x => x.newSet.DefaultIfEmpty(),
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.newSet == null)
                    .Select(x =>
                        new MRFSignatories
                        {
                            ID = x.oldSet.oldSet.ID,
                            UserId = x.oldSet.oldSet.UserId,
                            PositionId = x.oldSet.oldSet.PositionId,
                            ApproverId = x.oldSet.oldSet.ApproverId,
                            ApproverDescription = x.oldSet.oldSet.ApproverDescription,
                            TAT = x.oldSet.oldSet.TAT,
                            Order = x.oldSet.oldSet.Order,
                            CreatedDate = x.oldSet.oldSet.CreatedDate,
                            CreatedBy = x.oldSet.oldSet.CreatedBy,
                            ModifiedDate = x.oldSet.oldSet.ModifiedDate,
                            ModifiedBy = x.oldSet.oldSet.ModifiedBy,
                            CompanyID = x.oldSet.oldSet.CompanyID,
                        }).ToList();
        }

        /// <summary>
        /// Get records that are added on the form and doesn't exists in database.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="MRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<MRFSignatories> GetSignatoriesToAdd(List<MRFSignatories> left, List<MRFSignatories> right, int UserId)
        {
            if (right == null || right.Count == 0)
                return new List<MRFSignatories>();
            else
                return right.GroupJoin(
               left,
                    x => new { x.UserId, x.PositionId, x.ApproverId },
                    y => new { y.UserId, y.PositionId, y.ApproverId },
               (x, y) => new { newSet = x, oldSet = y })
               .SelectMany(x => x.oldSet.DefaultIfEmpty(),
               (x, y) => new { newSet = x, oldSet = y })
               .Where(x => x.oldSet == null)
               .Select(x =>
                   new MRFSignatories
                   {
                       //ID = x.newSet.newSet.ID,
                       UserId = x.newSet.newSet.UserId,
                       PositionId = x.newSet.newSet.PositionId,
                       ApproverId = x.newSet.newSet.ApproverId,
                       ApproverDescription = x.newSet.newSet.ApproverDescription,
                       TAT = x.newSet.newSet.TAT,
                       Order = x.newSet.newSet.Order,
                       CreatedBy = (short)UserId,
                       CreatedDate = DateTime.Now,
                   }).ToList();
        }

        /// <summary>
        /// Get records with the same PartNumber and ClaimsId but with different 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="MRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<MRFSignatories> GetSignatoriesToUpdate(List<MRFSignatories> left, List<MRFSignatories> right, int UserId)
        {
            if (right == null || right.Count == 0)
                return new List<MRFSignatories>();
            else
                return left.Join(
                    right,
                    x => new { x.UserId, x.PositionId, x.ApproverId },
                    y => new { y.UserId, y.PositionId, y.ApproverId },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.ApproverDescription.Equals(x.newSet.ApproverDescription) || !x.oldSet.TAT.Equals(x.newSet.TAT) || !x.oldSet.Order.Equals(x.newSet.Order))
                    .Select(y => new MRFSignatories
                    {
                        ID = y.oldSet.ID,
                        UserId = y.oldSet.UserId,
                        PositionId = y.oldSet.PositionId,
                        ApproverId = y.oldSet.ApproverId,
                        ApproverDescription = y.newSet.ApproverDescription,
                        TAT = y.newSet.TAT,
                        Order = y.newSet.Order,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = (short)UserId,
                        ModifiedDate = DateTime.Now,
                        CompanyID = y.oldSet.CompanyID
                    }).ToList();
        }
    }
}
