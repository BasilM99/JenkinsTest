using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.Framework;
using Noqoush.Framework.DataAnnotations;

using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP
{

    [DataContract]
    public class PMPDealDto
    {
        public virtual bool allowEdit
        {
            get
            {

                return !IsGlobal ? true :( AccountId == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator"))? true : false;

            }
        }
        [DataMember]
        public virtual bool IsLocked
        {
            get;set;
        }

        [DataMember]
        public virtual bool IsReadOnly
        {
            get; set;
        }
        [DataMember]
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

        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public int AdvertiserId { get; set; }
        [DataMember]
        public string AdvertiserName { get; set; }
        [DataMember]
        public string AdvertiserAccountName { get; set; }

        
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]

        public string Note { get; set; }

        [DataMember]
        [Required()]
        public string DealID { get; set; }

        [DataMember]
        [Required()]
        public virtual DealType Type { get; set; }

        [DataMember]
        [Required()]

        public string Name { get; set; }



        [DataMember]
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

        [DataMember]
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
        [DataMember]

        public virtual string DealTypeString
        {
            get
            {

                return Type.ToText();

            }
            set { }
        }


        [DataMember]

        public string StartDateString { get; set; }

        [DataMember]
        public string EndDateString { get; set; }

        [DataMember]

        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
     


        [DataMember]
        [Required()]
        [RegularExpression(@"^\$?\d+(\.(\d{1,3}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.###}")]
        public decimal? Price { get; set; }

        [DataMember]
        [StringLength(1024, ResourceName = "NoteLengthMsg")]
        public string Description { get; set; }


        [DataMember]
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


        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        [Required()]
        public int PublisherId { get; set; }

        [DataMember]
        [Required()]
        public int ExchangeId { get; set; }


        [DataMember]
        [Required]
        public string PublisherName { get; set; }

        [DataMember]

        public string ExchangeName { get; set; }



        //public User User { get; set; }


        //public Noqoush.AdFalcon.Domain.Common.Model.Account.Account Account { get; set; }

        //public Noqoush.AdFalcon.Domain.Common.Model.Account.Account Publisher { get; set; }


        //public SSPPartner Exchange { get; set; }


        [DataMember]

        public PMPTargetingSaveDto PMPTargetingSaveDto { get; set; }
        [DataMember]

        public PMPTargetingGetDto PMPTargetingGetDto { get; set; }
    }


    [DataContract]
    public class PMPDealSaveDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual IList<ErrorData> Warnings { get; set; }

    }


    [DataContract]
    public class ResultPMPDealDto
    {
        [DataMember]
        public List<PMPDealDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
