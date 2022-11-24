using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class HouseAdBaseDto
    {

        [ProtoMember(1)]
        public int ID { get; set; }
        [ProtoMember(2)]
        public CampaignListDto Campaign { get; set; }

    }
}