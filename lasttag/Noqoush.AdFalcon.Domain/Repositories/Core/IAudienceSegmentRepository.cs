using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
   
    public interface IAudienceSegmentRepository : IKeyedRepository<AudienceSegment, int>
    {
        int GetCode(int Id);
        int GetSegmentIdByCode(int Code);

        AudienceSegment GetSegmentByCode(int Code);

        AudienceSegment GetSegmentById(int Id);

        int GeMaxCode();
        string GetSegmentDataProvider(int Id);
    }
}
