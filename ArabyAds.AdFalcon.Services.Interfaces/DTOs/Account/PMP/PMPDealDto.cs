using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.DataAnnotations;

using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP
{

    [ProtoContract]
    public class PMPDealDto
    {
        public virtual bool allowEdit
        {
            get;
            set;
        }
       [ProtoMember(1)]
        public virtual bool IsLocked
        {
            get;set;
        }
       
        [ProtoMember(2)]
        public virtual bool IsReadOnly
        {
            get; set;
        }
       [ProtoMember(3)]
        public virtual bool IsGlobal
        {
            get;
            set;
        }

        public virtual string IsGlobalString
        {
            get
            {
                return !IsGlobal ?
                    ResourceManager.Instance.GetResource("False") :
                    ResourceManager.Instance.GetResource("True");
            }
        }

       [ProtoMember(4)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(5)]
        public int AdvertiserId { get; set; }
       [ProtoMember(6)]
        public string AdvertiserName { get; set; }
       [ProtoMember(7)]
        public string AdvertiserAccountName { get; set; }

        
       [ProtoMember(8)]
        public DateTime CreationDate { get; set; }
       [ProtoMember(9)]
        public int ID { get; set; }

       [ProtoMember(10)]
        public bool IsDeleted { get; set; }
       [ProtoMember(11)]

        public string Note { get; set; }

       [ProtoMember(12)]
        [Required()]
        public string DealID { get; set; }

       [ProtoMember(13)]
        [Required()]
        public virtual DealType Type { get; set; }

       [ProtoMember(14)]
        [Required()]

        public string Name { get; set; }



       [ProtoMember(15)]
        public string Scope
        {
            get
            {

                if (IsGlobal)
                {


                    return ResourceManager.Instance.GetResource("Global") ;
                }

                return ResourceManager.Instance.GetResource("Local");

            }
            set { }
        }

       [ProtoMember(16)]
        public string ScopeString
        {
            get
            {
                if (IsGlobal)
                    return "fa fa-globe";
        
                else
                    return "";

            

             

            }
            set { }
        }
       [ProtoMember(17)]

        public virtual string DealTypeString
        {
            get
            {

                return Type.ToText();

            }
            set { }
        }


       [ProtoMember(18)]

        public string StartDateString { get; set; }

       [ProtoMember(19)]
        public string EndDateString { get; set; }

       [ProtoMember(20)]

        public DateTime? StartDate { get; set; }

       [ProtoMember(21)]
        public DateTime? EndDate { get; set; }

       [ProtoMember(22)]
        public DateTime? StartTime { get; set; }
       [ProtoMember(23)]
        public DateTime? EndTime { get; set; }
     


       [ProtoMember(24)]
        [Required()]
        [RegularExpression(@"^\$?\d+(\.(\d{1,3}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.###}")]
        public decimal? Price { get; set; }

       [ProtoMember(25)]
        [StringLength(1024, ResourceName = "NoteLengthMsg")]
        public string Description { get; set; }


       [ProtoMember(26)]
        public string StatusDescription
        {
            get
            {




                if (IsDeleted)
                {

                    return ResourceManager.Instance.GetResource("StatusNotActive", "PMPDeals");
                }
                if (StartDate.HasValue)
                {
                    if (StartDate > Framework.Utilities.Environment.GetServerTime())
                    {
                        return ResourceManager.Instance.GetResource("StatusNotStarted", "PMPDeals");

                    }
                }

                if (EndDate.HasValue)
                {
                    if (EndDate < Framework.Utilities.Environment.GetServerTime())
                    {
                        return ResourceManager.Instance.GetResource("StatusExpried", "PMPDeals");

                    }
                }
                if (!StartDate.HasValue)
                {

                    return ResourceManager.Instance.GetResource("StatusActive", "PMPDeals");
                }
                if (StartDate <= EndDate)
                {

                    return ResourceManager.Instance.GetResource("StatusActive", "PMPDeals");
                }
                if (!EndDate.HasValue)
                {
                    if (StartDate.HasValue)
                    {
                        return ResourceManager.Instance.GetResource("StatusActive", "PMPDeals");

                    }

                }

                return string.Empty;

            }
            set { }
        }


       [ProtoMember(27)]
        public int UserId { get; set; }

       [ProtoMember(28)]
        public int AccountId { get; set; }

       [ProtoMember(29)]
        [Required()]
        public int PublisherId { get; set; }

       [ProtoMember(30)]
        [Required()]
        public int ExchangeId { get; set; }


       [ProtoMember(31)]
        [Required]
        public string PublisherName { get; set; }

       [ProtoMember(32)]

        public string ExchangeName { get; set; }



        //public User User { get; set; }


        //public ArabyAds.AdFalcon.Domain.Common.Model.Account.Account Account { get; set; }

        //public ArabyAds.AdFalcon.Domain.Common.Model.Account.Account Publisher { get; set; }


        //public SSPPartner Exchange { get; set; }


       [ProtoMember(33)]

        public PMPTargetingSaveDto PMPTargetingSaveDto { get; set; }
       [ProtoMember(34)]

        public PMPTargetingGetDto PMPTargetingGetDto { get; set; }
        [ProtoMember(35)]
        public string PriceString { get {

                if (this.Price.HasValue)
                {
                    return this.Price.Value.ToString("0.###");
                }
                return string.Empty;
            }
            set { }
        
        }

        [ProtoMember(36)]
        public virtual bool IsAdded
        {
            get; set;
        }
    }


    [ProtoContract]
    public class PMPDealSaveDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }
        [ProtoMember(2)]
        public virtual IList<ErrorData> Warnings { get; set; } = new List<ErrorData>();

    }


    [ProtoContract]
    public class ResultPMPDealDto
    {
        [ProtoMember(1)]
        public List<PMPDealDto> Items { get; set; } = new List<PMPDealDto>();
       [ProtoMember(2)]
        public long TotalCount { get; set; }

    }

    [ProtoContract]
    public class DropDownDto
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }

    }
}
