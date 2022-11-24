using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class SaveAppSiteDtoResult
    {
       [ProtoMember(1)]
        public int Id { get; set; }

       [ProtoMember(2)]
        public string PublisherId { get; set; }
    }
}