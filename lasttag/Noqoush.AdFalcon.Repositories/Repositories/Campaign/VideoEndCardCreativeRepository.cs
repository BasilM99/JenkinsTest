using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
   


    public class VideoEndCardCreativeRepository : RepositoryBase<Domain.Model.Campaign.VideoEndCardCreative, int>, IVideoEndCardCreativeRepository
    {
        public VideoEndCardCreativeRepository(RepositoryImplBase<Domain.Model.Campaign.VideoEndCardCreative, int> repository)
            : base(repository)
        {
        }
    }
}
