using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdCreativeAttributeRepository : IKeyedRepository<Model.Campaign.AdCreativeAttribute, int>
    {
    }
}
