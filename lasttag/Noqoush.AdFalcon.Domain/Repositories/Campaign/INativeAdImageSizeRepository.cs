using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public interface INativeAdImageSizeRepository : IKeyedRepository<NativeAdImageSize, int>
    {
    }
}
