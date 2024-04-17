using System.Collections.Generic;
using System.Linq;

namespace Utilities.API.ReferenceMaintenance
{
    public class Common_Reference
    {
        public IEnumerable<ReferenceValue> GetReferenceToDelete(List<ReferenceValue> left, List<ReferenceValue> right)
        {
            return left.GroupJoin(
                    right,
                    x => new { x.RefCode, x.Value },
                    y => new { y.RefCode, y.Value },
                    (x, y) => new { oldSet = x, newSet = y })
                    .SelectMany(x => x.newSet.DefaultIfEmpty(),
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.newSet == null)
                    .Select(x =>
                        new ReferenceValue
                        {
                            ID = x.oldSet.oldSet.ID
                        }).ToList();
        }

        public IEnumerable<ReferenceValue> GetReferenceToAdd(List<ReferenceValue> left, List<ReferenceValue> right)
        {
            return right.GroupJoin(
               left,
                    x => new { x.RefCode, x.Value },
                    y => new { y.RefCode, y.Value },
               (x, y) => new { newSet = x, oldSet = y })
               .SelectMany(x => x.oldSet.DefaultIfEmpty(),
               (x, y) => new { newSet = x, oldSet = y })
               .Where(x => x.oldSet == null)
               .Select(x =>
                   new ReferenceValue
                   {
                       RefCode = x.newSet.newSet.RefCode,
                       Value = x.newSet.newSet.Value,
                       Description = x.newSet.newSet.Description,
                       UserID = x.newSet.newSet.UserID
                   }).ToList();
        }

        public IEnumerable<ReferenceValue> GetReferenceToUpdate(List<ReferenceValue> left, List<ReferenceValue> right)
        {
            return left.Join(
                    right,
                    x => new { x.RefCode, x.Value },
                    y => new { y.RefCode, y.Value },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.Description.Equals(x.newSet.Description))
                    .Select(y => new ReferenceValue
                    {
                        ID = y.oldSet.ID,
                        RefCode = y.newSet.RefCode,
                        Value = y.newSet.Value,
                        Description = y.newSet.Description,
                        UserID = y.newSet.UserID
                    }).ToList();
        }
    }
}