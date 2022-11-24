using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{

    [DataContract]
    public class PartyListResultDto
    {
        [DataMember]
        public IEnumerable<PartyDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

    [DataContract]
    public class PartyDto
    {
        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public int? AccountId { get; set; }
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string AccountName { get; set; }
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string Name { get; set; }

        [DataMember]
        public string TypeNameString { get; set; }

        [DataMember]
        public bool Visible { get; set; }

        [DataMember]
        public string SiteProviderURL { get; set; }
        [DataMember]
        public  bool IsFTPEnabled { get; set; }
        [DataMember]
        public  string FTPURL { get; set; }

    }

    [DataContract]
    public class EmployeeDto : PartyDto
    {
        [DataMember]
        [Required()]
        public int JobPositionId { get; set; }
    }


    [DataContract]
    public class WhitleListIPDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int SSPPartnerId { get; set; }
        public byte[] IP { get; set; }

        [DataMember]
        public string IPString { get; set; }

    }
    public class BusinessPartnerDto : PartyDto
    {

        [DataMember]
        public IList<int> AccountList { get; set; }
        [DataMember]
        public IList<int> AdvertiserList { get; set; }
        [DataMember]
        public IList<int> WebCreativeFormatsList { get; set; }
        [DataMember]
        public IList<int> MobileCreativeFormatsList { get; set; }
        [DataMember]
        public string insertedIps { get; set; }
        [DataMember]
        public string deletedIps { get; set; }
        [DataMember]
        public string DoubleEncodedClickTrackerMacroName { get; set; }


        [DataMember]
        public bool ProvideImpressionTrackersMechanism { get; set; }


        [DataMember]
        public string AuctionPriceEncryptionKey { get; set; }
        [DataMember]
        public string AuctionPriceIntegrityKey { get; set; }
        [DataMember]
        public string AuctionPriceTestValue { get; set; }



        [Email(ResourceName = "InvalidEmail")]
        [DataMember]
        //[Required]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string ContactPerson { get; set; }

        [DataMember]
        [Required]
        public virtual string Code { get; set; }

        [DataMember]
        [Required]
        public virtual int BusinessPartnerTypeId { get; set; }

        [DataMember]
        public virtual string AuctionPricePricingUnitId { get; set; }
        [DataMember]
        public virtual string AuctionPriceEncryptionAlgorithmId { get; set; }

        //[Required]
        //public virtual int AppSiteId { get; set; }

        [DataMember]
        public virtual string Address { get; set; }

        [DataMember]

        public virtual string AppSiteName { get; set; }

        [DataMember]
        [RegularExpression(@"^\+?[\d]{9,}$", ResourceName = "InvalidPhone")]
        public virtual string Phone { get; set; }
        [DataMember]
        [Required]
        public int? AppSiteId { get; set; }

        [DataMember]

        public IList<WhitleListIPDto> WhileIPs { get; set; }

        [DataMember]
        public string DefaultSeatId { get; set; }
        [DataMember]
        [Required]
        public string OpenRtbVersion { get; set; }
        [DataMember]
        public string EncryptionKey { get; set; }
        [DataMember]
        public string IntegrityKey { get; set; }
        [DataMember]
        public string AuctionPriceMacroName { get; set; }

        [DataMember]
        public string WhitelistIPs { get; set; }
        [DataMember]

        public int? documentId { get; set; }
        [DataMember]
        public string ClickTrackerMacroName { get; set; }

        [DataMember]
        public bool SupportMultipleClickTrackers { get; set; }


        [DataMember]
        public int ImpressionTrackers { get; set; }
        [DataMember]
        public int ClicksTrackers { get; set; }
        [DataMember]
        public int GeofenceRadius { get; set; }
        [DataMember]
        public bool TaggingAllowed { get; set; }
        [DataMember]
        public bool SupportWinNotice { get; set; }
        [DataMember]
        public bool DisallowGeofenceLessThanRadius { get; set; }


        [DataMember]
        public bool FingerPrintAllowed { get; set; }
        [DataMember]
        public bool AllowExchangeCreativeFormat { get; set; }


        [DataMember]
        [Range(0, 100)]
        [Required]
        public int NumberOfSupportedVastWrapperLevels { get; set; }
        [DataMember]
        [Range(0, 100)]
        [Required]

        public int NumberOfSupportedImpressionTrackersInPartnerMechanism { get; set; }
        [DataMember]

        public bool ReportUnfilledRequests { get; set; }
        [DataMember]

        public string DeviceOSIdsIncludeValidUserId { get; set; }


        [DataMember]
        public virtual bool IsExternalProvider { get; set; }
        [DataMember]
        public virtual bool AdMarkupLogRequired { get; set; }

        [DataMember]
        public virtual bool AllowImpressionTrackers { get; set; }

        [DataMember]
        public virtual string BlockedDomains { get; set; }
        

        //[DataMember]
        //public virtual string SiteProviderURL { get; set; }



    }




    [DataContract]
    public class PartySaveDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class EmployeeSaveDto : PartySaveDto
    {
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public int JobPositionId { get; set; }
    }

    [DataContract]
    public class BusinessPartnerSaveDto : PartySaveDto
    {
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string ContactPerson { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual string Phone { get; set; }
    }
}
