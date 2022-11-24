using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    
    public interface ISegmentRepository : IKeyedRepository<Segment, int>
    { 
    
    }
    public interface IAudienceSegmentRepository : IKeyedRepository<AudienceSegment, int>
    {
        int GetCode(int Id);
        int GetSegmentIdByCode(int Code);

        AudienceSegment GetSegmentByCode(int Code);

        AudienceSegment GetSegmentById(int Id);

        int GeMaxCode();
        string GetSegmentDataProvider(int Id);
    }


    public interface IContextualSegmentRepository : IKeyedRepository<ContextualSegment, int>
    {
        int GetCode(int Id);
        int GetSegmentIdByCode(int Code);

        ContextualSegment GetSegmentByCode(int Code);

        ContextualSegment GetSegmentById(int Id);

        int GeMaxCode();
        string GetSegmentDataProvider(int Id);
    }
}
