using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core.Video;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core.Video
{
    public class PlaybackMethodsRepository : RepositoryBase<PlaybackMethods, int>, IPlaybackMethodsRepository
    {
        public PlaybackMethodsRepository(RepositoryImplBase<PlaybackMethods, int> repository)
            : base(repository)
        {
        }

    }
}
