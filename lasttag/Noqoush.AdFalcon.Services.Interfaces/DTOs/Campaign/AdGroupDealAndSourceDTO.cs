using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupDealAndSourceDTO
    {
        [DataMember]
        public AdGroupSettingsDto AdGroupSettings { get; set; }

           [DataMember]
        public CampaignBidConfigModelDto BidConfigs{get; set;}
    [DataMember]
        public IList<PMPDealDto>  Deals { get; set; }
        [DataMember]
        public InventorySourceModelDto  Sources { get; set; }
    }
}
