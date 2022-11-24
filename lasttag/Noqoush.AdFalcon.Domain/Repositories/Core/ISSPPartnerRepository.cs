using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    
    public interface ISSPPartnerRepository : IKeyedRepository<SSPPartner, int>
    {

        bool CheckWeatherMeetRTBSetting(int AppSiteId);
        List<int> CheckWeatherNotMeetRTBSettings();
        List<int> CheckWeatherMeetRTBSettings();

        List<int> CheckWeatherNotMeetRTBSettingsSSPPartner();

        int CheckWeatherMeetGefoenceResricions(List<int> AppSiteIds);
    }
}
