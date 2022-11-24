using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class SaveAppSiteDtoResult
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PublisherId { get; set; }
    }
}