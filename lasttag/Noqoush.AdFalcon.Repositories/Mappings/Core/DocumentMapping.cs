using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class DocumentMapping : ClassMap<Document>
    {
        public DocumentMapping()
        {
            Table("documents");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Document'");
            References(x => x.DocumentType, "DocumentTypeId");
            Map(x => x.Name);
            Map(x => x.Extension);
            Map(x => x.Size);
            Map(x => x.UploadedDate);
            Map(x => x.Content);//.LazyLoad();
            Map(x => x.IsDeleted);
            Map(x => x.IsWebHDFS);
            
            Map(x => x.Usage, "`Usage`");
        }
    }
}