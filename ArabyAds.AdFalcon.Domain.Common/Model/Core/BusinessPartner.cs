using System;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using System.Linq;
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;

namespace Noqoush.AdFalcon.Domain.Model.Core
{

    public class BusinessPartner : Party
    {
        private static IBusinessPartnerRepository _BusinessPartnerRepository = null;
        private static IBusinessPartnerRepository BusinessPartnerRepository
        {
            get
            {
                if (_BusinessPartnerRepository == null)
                {
                    _BusinessPartnerRepository = Framework.IoC.Instance.Resolve<IBusinessPartnerRepository>();
                }
                return _BusinessPartnerRepository;
            }
        }
        public BusinessPartner()
        {
        }
        public virtual IList<SSPPartnerSupportedCreativeFormats> WebCreativeFormatsList { get; set; }
        public virtual IList<SSPPartnerSupportedCreativeFormats> MobileCreativeFormatsList { get; set; }

        public virtual IList<BusinessPartnerAdvertiserBlock> AdvertiserBlockList { get; set; }
        public virtual IList<BusinessPartnerDomainBlock> DomainBlockList { get; set; }
        
        public virtual IList<BusinessPartnerAccountWhite> AccountWhiteList { get; set; }
        public virtual string Code { get; set; }
        public virtual BusinessPartnerType Type { get; set; }
        public virtual string Email { get; set; }
        public virtual string ContactPerson { get; set; }
        public virtual Model.Account.Account Account { get; set; }
        public virtual string Address { get; set; }
        public virtual string Phone { get; set; }
        public virtual string BlockedDomains { get; set; }
        
        public virtual bool HasAdvertiserBlock { get; set; }
        public virtual bool HasAccountWhite { get; set; }
        public virtual Document Icon { get; set; }
        public virtual void Delete()
        {

            this.IsDeleted = true;
        }
        public override string GetDescription()
        {
            return Name;
        }
        public virtual string AccountName
        {
            get
            {
                return Account != null ? Account.GetName() : null;

            }
            set
            {

            }
        }
        public virtual AppSite.AppSite AppSite { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (IsValid)
            {
                IsValid = false;
                //create business Exception to hold error data list 
                var error = new BusinessException();
                var result = BusinessPartnerRepository.Query(M => !M.IsDeleted && M.Code == this.Code).ToList();
                int busPartnerCount = 0;
                if (this.Account != null)
                    busPartnerCount = BusinessPartnerRepository.Query(M => !M.IsDeleted && M.Account.ID == this.Account.ID && M.ID != ID).Count();

           //     var test = BusinessPartnerRepository.Query(M => !M.IsDeleted && M.Account.ID == this.Account.ID && M.ID != ID).ToList();
                //validate Name
                if (result != null && result.Count > 0)
                {
                    var err = new ErrorData { ID = "CodeBusinessPartnerBR" };
                    error.Errors.Add(err);
                    if (result.Count == 1)
                    {
                        if (result[0].ID == this.ID)
                        {
                            error.Errors.Remove(err);
                        }
                    }
                }

                if (busPartnerCount > 0)
                {
                    var err = new ErrorData { ID = "AccountBusinessPartnerBR" };
                    error.Errors.Add(err);
                }
                if (error.Errors.Count > 0)
                {
                    IsValid = false;
                    throw (error);
                }
                IsValid = DataAnnotationsValidator.TryValidate(this);
            }
        }
    }


    public class SSPPartner : BusinessPartner
    {

        public virtual int NumberOfSupportedClickTrackersInNative { get; set; }
        public virtual int NumberOfSupportedImpressionTrackersInNative { get; set; }

        public virtual string AuctionPriceEncryptionKey { get; set; }
        public virtual string AuctionPriceIntegrityKey { get; set; }
        public virtual string AuctionPriceTestValue { get; set; }
        public virtual string AuctionPricePricingUnitId { get; set; }
        public virtual string AuctionPriceEncryptionAlgorithmId { get; set; }

        public virtual IList<SSPPartnerWhiteIP> WhileIPs { get; set; }


        public virtual string DefaultSeatId { get; set; }
        public virtual string OpenRtbVersion { get; set; }
        public virtual string EncryptionKey { get; set; }
        public virtual string IntegrityKey { get; set; }
        public virtual string AuctionPriceMacroName { get; set; }
        public virtual string WhitelistIPs { get; set; }


        public virtual string ClickTrackerMacroName { get; set; }
        public virtual bool SupportMultipleClickTrackers { get; set; }


        public virtual string DoubleEncodedClickTrackerMacroName { get; set; }
        public virtual bool ProvideImpressionTrackersMechanism { get; set; }



        public virtual bool TaggingAllowed { get; set; }
        public virtual bool DisallowGeofenceLessThanRadius { get; set; }



        public virtual bool FingerPrintAllowed { get; set; }
        public virtual bool AllowExchangeCreativeFormat { get; set; }

        public virtual bool SupportWinNotice { get; set; }
        public virtual int GeofenceRadius { get; set; }

        public virtual int NumberOfSupportedVastWrapperLevels { get; set; }

        public virtual int NumberOfSupportedImpressionTrackersInPartnerMechanism { get; set; }
        public virtual bool ReportUnfilledRequests { get; set; }
        public virtual string DeviceOSIdsIncludeValidUserId { get; set; }






    }


    public class DSPPartner : BusinessPartner
    {

        //public AppSite.AppSite AppSite { get; set; }
    }


    public class DPPartner : BusinessPartner
    {

        //public AppSite.AppSite AppSite { get; set; }
        public virtual bool AdMarkupLogRequired { get; set; }
        public virtual bool AllowImpressionTrackers { get; set; }

        public virtual bool IsFTPEnabled { get; set; }
        public virtual string FTPURL { get; set; }
        public virtual bool IsExternalProvider { get; set; }
        public virtual string SiteProviderURL { get; set; }
        public virtual string APISiteProviderURL { get; set; }
        public virtual string APIKey { get; set; }

        public virtual string APISecret { get; set; }
        public virtual string CertPath { get; set; }

        public virtual string CertPass { get; set; }

        public virtual int Order { get; set; }
    }
}
