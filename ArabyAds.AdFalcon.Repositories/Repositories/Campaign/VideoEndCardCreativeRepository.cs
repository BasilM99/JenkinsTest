using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
   


    public class VideoEndCardCreativeRepository : RepositoryBase<Domain.Model.Campaign.VideoEndCardCreative, int>, IVideoEndCardCreativeRepository
    {
        public VideoEndCardCreativeRepository(RepositoryImplBase<Domain.Model.Campaign.VideoEndCardCreative, int> repository)
            : base(repository)
        {
        }
    }
}
