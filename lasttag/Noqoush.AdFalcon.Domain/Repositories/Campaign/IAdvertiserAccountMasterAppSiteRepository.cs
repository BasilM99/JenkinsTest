using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;
namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdvertiserAccountMasterAppSiteRepository : IKeyedRepository<AdvertiserAccountMasterAppSite, int>
    {
    }


    public interface IPixelRepository : IKeyedRepository<Pixel, int>
    {
    }

    public interface IAudienceSegmentPixelMapRepository : IKeyedRepository<AudienceSegmentPixelMap, int>
    {
    }

    

}
