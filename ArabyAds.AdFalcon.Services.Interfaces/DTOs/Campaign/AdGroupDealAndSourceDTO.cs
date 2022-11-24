using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupDealAndSourceDTO
    {
       [ProtoMember(1)]
        public AdGroupSettingsDto AdGroupSettings { get; set; }

          [ProtoMember(2)]
        public CampaignBidConfigModelDto BidConfigs{get; set;}
   [ProtoMember(3)]
        public IList<PMPDealDto>  Deals { get; set; }
       [ProtoMember(4)]
        public InventorySourceModelDto  Sources { get; set; }
    }
}
