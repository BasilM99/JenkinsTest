using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetCreativeUnitByCriteriaWithDimensionsRequest: GetCreativeUnitByCriteriaRequest
    {
        [ProtoMember(1)]
        public int Width { get; set; }
        [ProtoMember(2)]
        public int Height { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{Width}_{Height}";
        }

    }
}
