using EMS_ManPowerServiceModel.MRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_ManPowerService.SharedClasses
{
    public class Common_Applicant
    {

        /// <summary>
        /// Get records that exists in database and is missing from the updated form.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<MRFApplicant> GetApplicantsToDelete(List<MRFApplicant> left, List<MRFApplicant> right)
        {
            return left.GroupJoin(
                    right,
                    x => new { x.MRFID, x.ApplicantID },
                    y => new { y.MRFID, y.ApplicantID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .SelectMany(x => x.newSet.DefaultIfEmpty(),
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.newSet == null)
                    .Select(x =>
                        new MRFApplicant
                        {
                            ID = x.oldSet.oldSet.ID,
                            MRFID = x.oldSet.oldSet.MRFID,
                            ApplicantID = x.oldSet.oldSet.ApplicantID,
                            Status = x.oldSet.oldSet.Status,
                            Stage = x.oldSet.oldSet.Stage,
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
        /// <param name="MRFHeader"></param>
        /// <returns></returns>
        public IEnumerable<MRFApplicant> GetApplicantsToAdd(List<MRFApplicant> left, List<MRFApplicant> right, MRFHeader MRFHeader)
        {
            return right.GroupJoin(
               left,
                    x => new { x.MRFID, x.ApplicantID },
                    y => new { y.MRFID, y.ApplicantID },
               (x, y) => new { newSet = x, oldSet = y })
               .SelectMany(x => x.oldSet.DefaultIfEmpty(),
               (x, y) => new { newSet = x, oldSet = y })
               .Where(x => x.oldSet == null)
               .Select(x =>
                   new MRFApplicant
                   {
                       MRFID = MRFHeader.ID,
                       ApplicantID = x.newSet.newSet.ApplicantID,
                       Status = x.newSet.newSet.Status,
                       Stage = x.newSet.newSet.Stage,
                       CreatedBy = MRFHeader.ModifiedBy.Value,
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
        public IEnumerable<MRFApplicant> GetApplicantsToUpdate(List<MRFApplicant> left, List<MRFApplicant> right, MRFHeader MRFHeader)
        {
            return left.Join(
                    right,
                    x => new { x.MRFID, x.ApplicantID },
                    y => new { y.MRFID, y.ApplicantID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.Status.Equals(x.newSet.Status) || !x.oldSet.Stage.Equals(x.newSet.Stage))
                    .Select(y => new MRFApplicant
                    {
                        ID = y.oldSet.ID,
                        MRFID = MRFHeader.ID,
                        ApplicantID = y.newSet.ApplicantID,
                        Status = y.newSet.Status,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = MRFHeader.ModifiedBy.Value,
                        ModifiedDate = DateTime.Now,
                    }).ToList();
        }
    }
}
