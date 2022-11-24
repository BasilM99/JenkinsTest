using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core.Video
{

    public interface IInStreamPositionRepository : IKeyedRepository<InStreamPosition, int>
    {
    }
}
