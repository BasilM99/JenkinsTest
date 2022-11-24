using ArabyAds.AdFalcon.Domain.Common.Model.Account.DPP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP
{
    [ProtoContract]

    public class ImpressionLogDto
    {
       [ProtoMember(1)]

        public int ID { set; get; }
       [ProtoMember(2)]

        public BusinessPartnerDto Provider { set; get; }
       [ProtoMember(3)]

        public DateTime CreationDate
        {
            get;
            set;
        }
       [ProtoMember(4)]

        public DateTime LastUpdate
        {
            get;
            set;
        }
       [ProtoMember(5)]

        public DateTime Day { set; get; }
       [ProtoMember(6)]

        public bool IsDeleted { get; set; }

       [ProtoMember(7)]

        public string Path { get; set; }

       [ProtoMember(8)]

        public ImpressionLogType Type { get; set; }

       [ProtoMember(9)]

        public string LogTypeString { get
            {
                if (Type==ImpressionLogType.AdMarkup)
                {
                    return "Ad Markup";
                }
                else
                {
                    return "Impression";
                }


            } set { } }
    }

    [ProtoContract]
    public class ImpressionLogListResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<ImpressionLogDto> Items { get; set; } = new List<ImpressionLogDto>();
       [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
}
