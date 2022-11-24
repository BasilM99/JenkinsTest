﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
namespace ArabyAds.AdFalcon.Domain.Repositories.Core
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
