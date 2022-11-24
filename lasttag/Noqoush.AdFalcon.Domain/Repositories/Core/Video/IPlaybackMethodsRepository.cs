using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core.Video;

namespace Noqoush.AdFalcon.Domain.Repositories.Core.Video
{

    public interface IPlaybackMethodsRepository : IKeyedRepository<PlaybackMethods, int>
    {
    }
}
