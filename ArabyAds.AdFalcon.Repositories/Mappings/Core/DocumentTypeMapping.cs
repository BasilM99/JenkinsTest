using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class DocumentTypeMapping : ClassMap<DocumentType>
    {
        public DocumentTypeMapping()
        {
            Table("documenttypes");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'DocumentType'");
            Map(p => p.Code);
            References(x => x.Name, "NameId");
            HasManyToMany(p => p.MIMETypes).Table("mimetypes_documentypes").ParentKeyColumn("DocumentTypeId").ChildKeyColumn("MIMETypeId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}