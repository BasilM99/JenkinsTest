using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model;
using FluentNHibernate;
namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdCreativeUnitMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdCreativeUnit>
    {
        public AdCreativeUnitMapping()
        {
            Table("adcreativeunits");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdCreativeUnit'");
            Map(x => x.IsDeleted);
            Map(x => x.UniqueId, "UniqueId");
            Map(x => x.Content, "Content");
            Map(x => x.ImageType);
            Map(x => x.SnapshotUrl);
            Map(x => x.Version);

            Map(p => p.KeepShapshot);
            References(x => x.CreativeUnit, "CreativeUnitId");
            References(x => x.AdCreative, "AdId");
            References(x => x.Document, "DocumentId");
            References(x => x.SnapshotDocument, "SnapshotDocumentId");
            HasMany(p => p.MediaFiles).KeyColumn("VideoAdCreativeUnitId").Cascade.AllDeleteOrphan().Inverse();
            //  HasOne(x => x.AdCreativeUnitVendor).PropertyRef(M=>M.Unit).Cascade.All();
            // HasManyToMany(x => x.Attributes).Table("ad_creative_unit_attributes").ParentKeyColumn("AdCreativeUnitId").ChildKeyColumn("CreativeAttributeId").AsSet();

            HasMany(p => p.AttributesMapping).KeyColumn("AdCreativeUnitId").Cascade.AllDeleteOrphan().Inverse();

            HasMany(x => x.Trackers).KeyColumn("AdCreativeUnitId").Where(x => !x.IsDeleted).Cascade.All();
            HasOne(x => x.InStreamVideoCreativeUnit).Cascade.All();
            HasMany(p => p.AdCreativeUnitVendorList).KeyColumn("AdCreativeUnitId").Cascade.AllDeleteOrphan().Inverse();
            References(x => x.Protocol, "CreativeProtocolId");

            
            // References(x => x.InStreamVideoCreativeUnit, "ID");
            //DiscriminateSubClassesOnColumn("",this.);
        }
    }

    public class InStreamVideoCreativeUnitsMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.InStreamVideoCreativeUnit>
    {
        public InStreamVideoCreativeUnitsMapping()
        {
            Table("video_ad_creative_unit_extension");
            Id(x => x.ID).GeneratedBy.Foreign("AdCreativeUnit"); ;
            Map(p => p.BitRate);
            Map(p => p.Width);
            Map(p => p.Height);
            References(p => p.OriginalCreativeUnit, "OriginalCreativeUnitId");
            References(x => x.ThumbnailDoc, "ThumbnailDocId");
           // Map(p => p.ThumbnailDocId);
            References(x => x.VideoType, "MIMETypeId");
            References(x => x.DeliveryMethod, "DeliveryMethodId");
            HasOne(x => x.AdCreativeUnit).Constrained().ForeignKey();
            //  References(x => x.AdCreativeUnit, "ID");
            //   DiscriminatorValue(1);
            //Join("video_ad_creative_units", w =>
            //{
            //    w.KeyColumn("Id");
            //    w.Map(p => p.BitRate);
            //    w.Map(p => p.Width);
            //    w.Map(p => p.Height);
            //    References(x => x.VideoType, "VideoTypeId");
            //  //  References(x => x.DeliveryMethod, "DeliveryMethodId");

            //});
        }
    }



}
