using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class ValidateAdvertiserRequest
    {
        [ProtoMember(1)]
        public int AdvertiserId { get; set; }
        [ProtoMember(2)]
        [DefaultValue(false)]
        public bool StatusCheck { get; set; }

        public override string ToString()
        {
            return $"{AdvertiserId}_{StatusCheck}";
        }


    }
}
