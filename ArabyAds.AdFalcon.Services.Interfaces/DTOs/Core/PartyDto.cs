using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{

    [ProtoContract]
    public class PartyListResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<PartyDto> Items { get; set; } = new List<PartyDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100,typeof(EmployeeDto))]
    [ProtoInclude(101, typeof(BusinessPartnerDto))]
    public class PartyDto
    {
       [ProtoMember(1)]
        public int? ID { get; set; }
       [ProtoMember(2)]
        [Required(ResourceName = "RequiredMessage")]
        public int? AccountId { get; set; }
       [ProtoMember(3)]
        [Required(ResourceName = "RequiredMessage")]
        public string AccountName { get; set; }
       [ProtoMember(4)]
        [Required(ResourceName = "RequiredMessage")]
        public string Name { get; set; }

       [ProtoMember(5)]
        public string TypeNameString { get; set; }

       [ProtoMember(6)]
        public bool Visible { get; set; }

       [ProtoMember(7)]
        public string SiteProviderURL { get; set; }
       [ProtoMember(8)]
        public  bool IsFTPEnabled { get; set; }
       [ProtoMember(9)]
        public  string FTPURL { get; set; }

    }

    [ProtoContract]
    public class EmployeeDto : PartyDto
    {
       [ProtoMember(1)]
        [Required()]
        public int JobPositionId { get; set; }
    }


    [ProtoContract]
    public class WhitleListIPDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public int SSPPartnerId { get; set; }
        public byte[] IP { get; set; }

       [ProtoMember(3)]
        public string IPString { get; set; }

    }
    [ProtoContract]
    public class BusinessPartnerDto : PartyDto
    {

       [ProtoMember(1)]
        public IList<int> AccountList { get; set; } = new List<int>();
        [ProtoMember(2)]
        public IList<int> AdvertiserList { get; set; } = new List<int>();
        [ProtoMember(3)]
        public IList<int> WebCreativeFormatsList { get; set; } = new List<int>();
        [ProtoMember(4)]
        public IList<int> MobileCreativeFormatsList { get; set; } = new List<int>();
        [ProtoMember(5)]
        public string insertedIps { get; set; }
       [ProtoMember(6)]
        public string deletedIps { get; set; }
       [ProtoMember(7)]
        public string DoubleEncodedClickTrackerMacroName { get; set; }


       [ProtoMember(8)]
        public bool ProvideImpressionTrackersMechanism { get; set; }


       [ProtoMember(9)]
        public string AuctionPriceEncryptionKey { get; set; }
       [ProtoMember(10)]
        public string AuctionPriceIntegrityKey { get; set; }
       [ProtoMember(11)]
        public string AuctionPriceTestValue { get; set; }



        [Email(ResourceName = "InvalidEmail")]
       [ProtoMember(12)]
        //[Required]
        public virtual string Email { get; set; }
       [ProtoMember(13)]
        public virtual string ContactPerson { get; set; }

       [ProtoMember(14)]
        [Required]
        public virtual string Code { get; set; }

       [ProtoMember(15)]
        [Required]
        public virtual int BusinessPartnerTypeId { get; set; }

       [ProtoMember(16)]
        public virtual string AuctionPricePricingUnitId { get; set; }
       [ProtoMember(17)]
        public virtual string AuctionPriceEncryptionAlgorithmId { get; set; }

        //[Required]
        //public virtual int AppSiteId { get; set; }

       [ProtoMember(18)]
        public virtual string Address { get; set; }

       [ProtoMember(19)]

        public virtual string AppSiteName { get; set; }

       [ProtoMember(20)]
        [RegularExpression(@"^\+?[\d]{9,}$", ResourceName = "InvalidPhone")]
        public virtual string Phone { get; set; }
       [ProtoMember(21)]
        [Required]
        public int? AppSiteId { get; set; }

       [ProtoMember(22)]

        public IList<WhitleListIPDto> WhileIPs { get; set; } = new List<WhitleListIPDto>();

        [ProtoMember(23)]
        public string DefaultSeatId { get; set; }
       [ProtoMember(24)]
        [Required]
        public string OpenRtbVersion { get; set; }
       [ProtoMember(25)]
        public string EncryptionKey { get; set; }
       [ProtoMember(26)]
        public string IntegrityKey { get; set; }
       [ProtoMember(27)]
        public string AuctionPriceMacroName { get; set; }

       [ProtoMember(28)]
        public string WhitelistIPs { get; set; }
       [ProtoMember(29)]

        public int? documentId { get; set; }
       [ProtoMember(30)]
        public string ClickTrackerMacroName { get; set; }

       [ProtoMember(31)]
        public bool SupportMultipleClickTrackers { get; set; }


       [ProtoMember(32)]
        public int ImpressionTrackers { get; set; }
       [ProtoMember(33)]
        public int ClicksTrackers { get; set; }
       [ProtoMember(34)]
        public int GeofenceRadius { get; set; }
       [ProtoMember(35)]
        public bool TaggingAllowed { get; set; }
       [ProtoMember(36)]
        public bool SupportWinNotice { get; set; }
       [ProtoMember(37)]
        public bool DisallowGeofenceLessThanRadius { get; set; }


       [ProtoMember(38)]
        public bool FingerPrintAllowed { get; set; }
       [ProtoMember(39)]
        public bool AllowExchangeCreativeFormat { get; set; }


       [ProtoMember(40)]
        [Range(0, 100)]
        [Required]
        public int NumberOfSupportedVastWrapperLevels { get; set; }
       [ProtoMember(41)]
        [Range(0, 100)]
        [Required]

        public int NumberOfSupportedImpressionTrackersInPartnerMechanism { get; set; }
       [ProtoMember(42)]

        public bool ReportUnfilledRequests { get; set; }
       [ProtoMember(43)]

        public string DeviceOSIdsIncludeValidUserId { get; set; }


       [ProtoMember(44)]
        public virtual bool IsExternalProvider { get; set; }
       [ProtoMember(45)]
        public virtual bool AdMarkupLogRequired { get; set; }

       [ProtoMember(46)]
        public virtual bool AllowImpressionTrackers { get; set; }

       [ProtoMember(47)]
        public virtual string BlockedDomains { get; set; }
        

        //[DataMember]
        //public virtual string SiteProviderURL { get; set; }



    }




    [ProtoContract]
    [ProtoInclude(100,typeof(EmployeeSaveDto))]
    [ProtoInclude(101,typeof(BusinessPartnerSaveDto))]
    public class PartySaveDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public string Name { get; set; }
    }

    [ProtoContract]
    public class EmployeeSaveDto : PartySaveDto
    {
       [ProtoMember(1)]
        [Required(ResourceName = "RequiredMessage")]
        public int JobPositionId { get; set; }
    }

    [ProtoContract]
    public class BusinessPartnerSaveDto : PartySaveDto
    {
       [ProtoMember(1)]
        public virtual string Email { get; set; }
       [ProtoMember(2)]
        public virtual string ContactPerson { get; set; }
       [ProtoMember(3)]
        public virtual string Address { get; set; }
       [ProtoMember(4)]
        public virtual string Phone { get; set; }
    }
}
