using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{

    [DataContract]
    public class AdvertiserAccountMasterAppSiteItemResultDto
    {
        
        [DataMember]
        public IEnumerable<AdvertiserAccountMasterAppSiteItemDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    [DataContract]
    public class AdvertiserAccountMasterAppSiteItemDto
    {

        [DataMember]

        public int Id { get; set; }
        [DataMember]

        public int LinkId { get; set; }


        [DataMember]

        public string AppSiteID { get; set; }

        [DataMember]
        public MasterAppSiteItemType Type { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]

        public string BundleID { get; set; }
        [DataMember]

        public string Domain { get; set; }


        [DataMember]
        public string AppSiteName { get; set; }

        [DataMember]
        public string TypeString { get { return Type.ToText(); } set { } }
        [DataMember]

        public int UserId { get; set; }
        [DataMember]

        public int AccountId { get; set; }
    }


    [DataContract]
    public class AudienceSegmentResultResultDto
    {

        [DataMember]
        public IEnumerable<AudienceSegmentDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

}
