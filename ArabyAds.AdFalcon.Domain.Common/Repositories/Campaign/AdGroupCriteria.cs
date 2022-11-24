using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign
{
    [ProtoContract]
    public class AdGroupCriteria 
    {
        public AdGroupCriteria()
        {
            CampaignType = CampaignType.Normal;
            CampaignOtherType = CampaignType.ProgrammaticGuaranteed;
        }

        //   private IAccountAdPermissionsRepository AdPermissionsRepository = IoC.Instance.Resolve<IAccountAdPermissionsRepository>();
        [ProtoMember(1)]
        public int CampaignId { get; set; }
        [ProtoMember(2)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(3)]
        public DateTime? DateTo { get; set; }
        [ProtoMember(4)]
        public string Name { get; set; }
        [ProtoMember(5)]
        public int Page { get; set; }
        [ProtoMember(6)]
        public int Size { get; set; }
        [ProtoMember(7)]
        public int AccountId { get; set; }

        [ProtoMember(8)]
        public int? AppSiteId { get; set; }

        [ProtoMember(9)]
        public List<int> Permissions { get; set; }

        [ProtoMember(10)]
        public CampaignType CampaignType { get; set; }

        [ProtoMember(11)]
        public CampaignType CampaignOtherType { get; set; }



    }
}
