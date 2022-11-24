
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Campaign.Targeting
{
    /* public class OperationResult
     {
         public List<AudienceSegment> UsedSegments { get; set; } = new List<AudienceSegment>();
         public List<AudienceSegment> BilledSegments { get; set; } = new List<AudienceSegment>();
         public decimal TotalPrice { get; set; }
         public void CalculateTotalPrice()
         {
             TotalPrice = 0;
             if (BilledSegments!=null)
             {
                 for (int i = 0; i < BilledSegments.Count; i++)
                 {
                     TotalPrice += BilledSegments[i].Price;
                 }
             }
         }

         public static OperationResult MergeAnd(OperationResult left, OperationResult right)
         {
             var operationResult = new OperationResult();
             for (int i = 0; i < left.BilledSegments.Count; i++)
             {
                 var lseg = left.BilledSegments[i];
                 var rseg = right.BilledSegments.Find((x) => x.Provider.ID == lseg.Provider.ID);

                 if (rseg != null)
                 {
                     right.BilledSegments.Remove(rseg);
                     operationResult.BilledSegments.Add(MaxPrice(lseg, rseg));
                 }
                 else
                 {
                     operationResult.BilledSegments.Add(lseg);
                 }
             }
             // remaining segments in right side
             operationResult.BilledSegments.AddRange(right.BilledSegments);

             // add all left used segments
             operationResult.UsedSegments.AddRange(left.UsedSegments);

             if (right.UsedSegments!=null)
             {
                 // add only right segments not exists in left segments
                 for (int i = 0; i < right.UsedSegments.Count; i++)
                 {
                     var rseg = right.UsedSegments[i];
                     var lseg = left.UsedSegments.Find((x) => x.Code == rseg.Code);
                     if (lseg == null)
                     {
                         operationResult.UsedSegments.Add(rseg);
                     }
                 }
             }
             operationResult.CalculateTotalPrice();

             return operationResult;
         }
         public static OperationResult MergeOr(OperationResult result1, OperationResult result2)
         {
             return result1.TotalPrice <= result2.TotalPrice ? result1 : result2;
         }

         public static OperationResult MergeOrMax(OperationResult result1, OperationResult result2)
         {
             return result1.TotalPrice >= result2.TotalPrice ? result1 : result2;
         }
         private static AudienceSegment MaxPrice(AudienceSegment seg1, AudienceSegment seg2)
         {
             return seg1.Price >= seg2.Price ? seg1 : seg2;
         }
     }*/
    [Serializable]
    [DataContract()]
    [Flags]
    public enum AudienceSegmentTargetingCategroyFlags

    {
        [EnumMember]
        Undefined = 0,

        [EnumMember]

        FirstPart = 1,
        [EnumMember]

        ThirdParty = 2,
        [EnumMember]

        ExternalParty = 4,



    }


}
