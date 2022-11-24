using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdCreativeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCreative>
    {
        public AdCreativeMapping()
        {
            Table("ads");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AdCreative'");
            Map(x => x.Name);
            Map(x => x.uId);

            //Map(FluentNHibernate.Reveal.Member<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCreative>("DataBid"));
            //Map(x => x.MaxDataBid);
           // Map(x => x.UpdatedbyPortal).Nullable();
            

            Map(FluentNHibernate.Reveal.Member<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCreative>("Bid"));
            Map(x => x.CretiveUnitDeviceType, "bannerType").CustomType<DeviceTypeEnum>().Nullable();
            Map(x => x.CreationDate);
            Map(x => x.IsDeleted);
            Map(x => x.IsSecureCompliant);
            Map(x => x.AdText);
            //Map(x => x.NameLower);
            HasMany(x => x.ClickTags).KeyColumn("AdId").Where(x => !x.IsDeleted).Cascade.All();
            References(x => x.Parent)
    .Column("ParentId");
            HasMany(d => d.AdCreativeUnits).KeyColumn("AdId").Cascade.AllDeleteOrphan().BatchSize(500).Where(x => !x.IsDeleted).Inverse();
            HasMany(d => d.AdCustomParameters).KeyColumn("AdId").Cascade.AllDeleteOrphan().Inverse();

            Map(x => x.EnvironmentType, "EnvironmentTypeId").CustomType(typeof(EnvironmentType)).Nullable();


           // Map(x => x.ClickMethod, "ClickMethod").CustomType(typeof(ClickMethod));
            Map(x => x.OrientationType, "OrientationTypeId").CustomType(typeof(OrientationType)).Nullable();
            References(x => x.Group, "AdGroupId");
            References(x => x.Language, "LanguageId").Nullable();
            HasOne(p => p.ActionValue).PropertyRef(p => p.AdCreative).Cascade.All();
            References(x => x.Status, "StatusId");
     
            References(x => x.PausedStatus, "PausedStatusId");
            HasMany(d => d.AppSiteAdQueues).KeyColumn("AdId").Cascade.AllDeleteOrphan().Inverse();
            Map(x => x.AdSubType, "SubTypeIdForPortal").CustomType(typeof(AdSubTypes));
            References(x => x.Type, "TypeIdForPortal").Not.Update().Not.Insert();

                        Map(x => x.AdSubTypeForPortal, "SubTypeId").CustomType(typeof(AdSubTypes));
            References(x => x.TypeForPortal, "TypeId");

            DiscriminateSubClassesOnColumn("TypeIdForPortal");
            //Map(x => x.DomainURL).Nullable();

            References(x => x.Keyword, "KeywordId").Nullable();


            Map(p => p.VerifyPrerequisiteEvents, "VerifyPrerequisiteEvents");
            Map(p => p.VerifyTargetingCriteria, "VerifyTargetingCriteria");
            Map(p => p.VerifyCampaignStartAndEndDate, "VerifyStartAndEndDate");
            Map(p => p.VerifyDailyBudget, "VerifyDailyBudget");
            Map(p => p.UpdateTags, "UpdateAudienceList");


            Map(p => p.UpdateEventsFrequency, "UpdateEventsFrequency");
            Map(p => p.VerifyEventsFrequency, "VerifyEventsFrequency");
            Map(p => p.ValidateRequestDeviceAndLocationData);
            HasOne(x => x.AdExtension).Cascade.All();
        }
    }
    public class BannersCreativeMapping : SubclassMap<BannerCreative>
    {
        public BannersCreativeMapping()
        {
            DiscriminatorValue(1);

        }
    }
    public class TextCreativeMapping : SubclassMap<TextCreative>
    {
        public TextCreativeMapping()
        {
            DiscriminatorValue(2);
            References(x => x.TileImage, "TileImageId").Cascade.All();
        }
    }

    public class PlainHtmlCreativeMapping : SubclassMap<PlainHtmlCreative>
    {
        public PlainHtmlCreativeMapping()
        {
            DiscriminatorValue(3);
        }
    }

    public class RichMediaCreativeMapping : SubclassMap<RichMediaCreative>
    {
        public RichMediaCreativeMapping()
        {
            DiscriminatorValue(4);
           
            //    References(d => d.RichMediaRequiredProtocol, "RichMediaRequiredProtocol");

        }
    }

    public class NativeAdCreativeMapping : SubclassMap<NativeAdCreative>
    {
        public NativeAdCreativeMapping()
        {
            DiscriminatorValue(5);
            Join("native_ads", w =>
            {
                w.KeyColumn("Id");
                w.Map(p => p.Description);
                w.Map(p => p.StarRating);
                w.Map(p => p.ShowIfInstalled);
                w.Map(p => p.AppOpenUrl);
                w.Map(p => p.ActionText);
                w.HasMany(p => p.Images).KeyColumn("NativeAdId").Cascade.AllDeleteOrphan().Inverse();
                w.HasMany(p => p.Icons).KeyColumn("NativeAdId").Cascade.AllDeleteOrphan().Inverse();

            });
        }
    }

    public class InStreamVideoCreativeMapping : SubclassMap<InStreamVideoCreative>
    {
        public InStreamVideoCreativeMapping()
        {
            DiscriminatorValue(6);
            Join("video_ads", w =>
            {
                w.KeyColumn("Id");
                w.Map(p => p.Description);
                w.Map(p => p.DurationInSeconds);
                w.Map(p => p.ThirdPartyTag);
                //   w.Map(p => p.IsXml);
                w.Map(p => p.IsDraft);
                w.Map(p => p.VideoEndCardFluid);

                w.Map(p => p.CreateOption, "CreateOptionId").CustomType<CreateOption>();



                HasMany(x => x.VideoEndCards).Cascade.AllDeleteOrphan()
    .Inverse() // Important!
    .KeyColumn("ParentId");


                HasMany(x => x.ThirdPartyTrackers).KeyColumn("VideoAdId").Where(x => !x.IsDeleted).Cascade.All();
             





            });
        }
    }
    public class VideoEndCardCreativeMapping : SubclassMap<VideoEndCardCreative>
    {
        public VideoEndCardCreativeMapping()
        {
            DiscriminatorValue(8);
            Join("companion_ads", w =>
            {
                w.KeyColumn("Id");
                w.Map(p => p.EnableAutoClose);
                w.Map(p => p.AutoCloseWaitInSeconds);

                //   w.Map(p => p.IsXml);

                w.Map(p => p.CardType, "TypeId").CustomType<VideoEndCardType>();







            });
        }
    }
    public class AdTrackerCreativeMapping : SubclassMap<AdTrackerCreative>
    {
        public AdTrackerCreativeMapping()
        {
            DiscriminatorValue(7);



            Join("tracking_ads", w =>
             {
                 w.KeyColumn("Id");

                 w.Map(p => p.ClickTrackerUrl);

                 w.Map(p => p.EnableEventsPostback, "EnableEventsPostback");
                 w.References(M => M.AppMarketingPartner, "AppMarketingPartnerId");

                 w.References(M => M.Platform, "PlatformId");
                 //w.HasOne(Reveal.Member<AdTrackerCreative, AppMarketingPartner>("AppMarketingPartner")).Constrained().ForeignKey("AppMarketingPartnerId").Constrained();
             });
        }
    }



    public class AdExtensionMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdExtension>
    {
        public AdExtensionMapping()
        {
            Table("ad_extension");
            Id(x => x.ID).GeneratedBy.Foreign("Creative"); ;
            Map(p => p.WrapperContent);
       
            HasOne(x => x.Creative).Constrained().ForeignKey();
            BatchSize(500);
           
        }
    }


}
 