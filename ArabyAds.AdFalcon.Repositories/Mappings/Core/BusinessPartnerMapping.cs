using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class BusinessPartnerMapping : SubclassMap<BusinessPartner>
    {
        public BusinessPartnerMapping()
        {
            //     this.Join("business_partners", x =>
            //{
            //    x.KeyColumn("Id");
            //    x.Map(p => p.Address);
            //    x.Map(p => p.Phone);
            //    x.Map(p => p.Email);
            //    x.Map(p => p.ContactPerson);
            //    x.References(p => p.Type, "TypeId").LazyLoad().Cascade.None();
            //    x.Map(p => p.IsDeleted);
            //});

            Table("business_partners");
            KeyColumn("Id");
            Map(x => x.Address);
            Map(x => x.Phone);
            Map(x => x.Code).Unique();
            Map(x => x.Email);
            Map(x => x.Order, "`order`").Not.Update();

            Map(x => x.ContactPerson);
            Map(x => x.HasAccountWhite);
            Map(x => x.HasAdvertiserBlock);

            Map(p => p.IsExternalProvider);
            References(p => p.Type, "TypeId").Cascade.None();
            References(p => p.Icon, "ICONID");
            HasMany(p => p.AdvertiserBlockList).KeyColumn("PartnerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(p => p.DomainBlockList).KeyColumn("PartnerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(p => p.AccountWhiteList).KeyColumn("PartnerId").Cascade.AllDeleteOrphan().Inverse();
            Map(p => p.AdMarkupLogRequired);
            Map(p => p.AllowImpressionTrackers);

            References(x => x.Account, "AccountId").LazyLoad().Cascade.None();
            Map(p => p.APISiteProviderURL).Not.Update();
            Map(p => p.APIKey).Not.Update();
            Map(p => p.APISecret).Not.Update();

            Map(p => p.CertPath).Not.Update();
            Map(p => p.CertPass).Not.Update();

            //Map(x => x.IsDeleted);
            // Map(p => p.BlockedDomains);

        }
    }

    public class SSPPartnerMapping : SubclassMap<SSPPartner>
    {
        public SSPPartnerMapping()
        {
            //     this.Join("business_partners", x =>
            //{
            //    x.KeyColumn("Id");
            //    x.Map(p => p.Address);
            //    x.Map(p => p.Phone);
            //    x.Map(p => p.Email);
            //    x.Map(p => p.ContactPerson);
            //    x.References(p => p.Type, "TypeId").LazyLoad().Cascade.None();
            //    x.Map(p => p.IsDeleted);
            //});

            Table("ssp_partners");
            KeyColumn("Id");
           // Map(p => p.DefaultSeatId).Nullable();
            Map(p => p.OpenRtbVersion).Nullable();
            //Map(p => p.EncryptionKey);
            // Map(p => p.IntegrityKey);
            Map(p => p.AuctionPriceEncryptionKey).Nullable();
            Map(p => p.AuctionPriceIntegrityKey).Nullable();
            Map(p => p.AuctionPriceTestValue).Nullable();
            Map(p => p.AuctionPriceEncryptionAlgorithmId).Nullable();
            Map(p => p.AuctionPricePricingUnitId).Nullable();

            Map(p => p.DoubleEncodedClickTrackerMacroName).Nullable();
            //Map(p => p.ProvideImpressionTrackersMechanism);

            Map(p => p.NumberOfSupportedClickTrackersInNative).Default("100");
            Map(p => p.NumberOfSupportedImpressionTrackersInNative).Default("100");

            Map(p => p.AuctionPriceMacroName);
            Map(p => p.ClickTrackerMacroName, "EncodedClickTrackerMacroName").Nullable();
           // Map(p => p.SupportMultipleClickTrackers);


            //Map(p => p.WhitelistIPs);

            HasMany(p => p.WhileIPs).KeyColumn("PartnerId").Cascade.AllDeleteOrphan().Inverse();
            References(x => x.AppSite, "AppSiteId").LazyLoad().Cascade.None();

            HasMany(p => p.MobileCreativeFormatsList).KeyColumn("PartnerId").Where("EnvironmentType = 2").Cascade.AllDeleteOrphan().Inverse();
            HasMany(p => p.WebCreativeFormatsList).KeyColumn("PartnerId").Where("EnvironmentType = 1").Cascade.AllDeleteOrphan().Inverse();

            Map(p => p.TaggingAllowed);

            //Map(p => p.AllowExchangeCreativeFormat, "AllowCreativeFormat");

            Map(p => p.FingerPrintAllowed);

            Map(p => p.DisallowGeofenceLessThanRadius);

            Map(p => p.SupportWinNotice);

            Map(p => p.GeofenceRadius);

            Map(p => p.NumberOfSupportedVastWrapperLevels);

            Map(p => p.NumberOfSupportedImpressionTrackersInPartnerMechanism);

            Map(p => p.ReportUnfilledRequests);

            Map(p => p.DeviceOSIdsIncludeValidUserId);
            BatchSize(200);

          

            //Map(x => x.IsDeleted);

        }
    }


    public class DSPPartnerMapping : SubclassMap<DSPPartner>
    {
        public DSPPartnerMapping()
        {
            //     this.Join("business_partners", x =>
            //{
            //    x.KeyColumn("Id");
            //    x.Map(p => p.Address);
            //    x.Map(p => p.Phone);
            //    x.Map(p => p.Email);
            //    x.Map(p => p.ContactPerson);
            //    x.References(p => p.Type, "TypeId").LazyLoad().Cascade.None();
            //    x.Map(p => p.IsDeleted);
            //});

            Table("dsp_partners");
            KeyColumn("Id");

            References(x => x.AppSite, "AppSiteId").LazyLoad().Cascade.None();
            //Map(x => x.IsDeleted);

        }
    }

    public class DPPartnerMapping : SubclassMap<DPPartner>
    {
        public DPPartnerMapping()
        {
            //     this.Join("business_partners", x =>
            //{
            //    x.KeyColumn("Id");
            //    x.Map(p => p.Address);
            //    x.Map(p => p.Phone);
            //    x.Map(p => p.Email);
            //    x.Map(p => p.ContactPerson);
            //    x.References(p => p.Type, "TypeId").LazyLoad().Cascade.None();
            //    x.Map(p => p.IsDeleted);
            //});

            Table("dp_partners");
            KeyColumn("Id");

           

            Map(p => p.SiteProviderURL);
         

            Map(p => p.FTPURL);
            Map(p => p.IsFTPEnabled);

            
               
           // Map(x => x.Order, "`order`").Not.Update();
            //References(x => x.AppSite, "AppSiteId").LazyLoad().Cascade.None();
            //Map(x => x.IsDeleted);

        }
    }


    public class ContextualPartnerMapping : SubclassMap<ContextualPartner>
    {
        public ContextualPartnerMapping()
        {
      
            Table("contextual_partners");
            KeyColumn("Id");

            

        }
    }
}