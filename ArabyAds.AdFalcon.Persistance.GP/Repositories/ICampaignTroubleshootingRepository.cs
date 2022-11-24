using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.ReportsGP.Repositories
{
    public interface ICampaignTroubleshootingRepository : IRepository<ChartDto>
    {
        List<CampaignTroubleshootingDto> GetResult(CampaignTroubleshootingCriteria args);
    }
}
