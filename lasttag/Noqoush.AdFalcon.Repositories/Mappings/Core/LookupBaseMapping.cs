using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    /*public class LookupBaseMapping : ClassMap<ManagedLookupBase>
    {
        public LookupBaseMapping()
        {
            // indicates that this class is the base
            // the values of its properties should
            // be united with the values of derived classes
            UseUnionSubclassForInheritanceMapping();
            // the Id function is used for identity
            // key mapping, it is possible to specify the type 
            // of the property, its access, the name
            // of the field in the table and its server type, 
            // facets and other mapping settings,
            // as well as to specify the class name to be used to 
            // generate the primary key for a new
            // record while saving a new record
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Currency'");
            References(x => x.Name, "NameId").Cascade.SaveUpdate();

        }
    }*/

    /*public class LookupBase2Mapping<t,e> : ClassMap<LookupBase<t,e>>
    {
        public LookupBase2Mapping()
        {
            // indicates that this class is the base
            // the values of its properties should
            // be united with the values of derived classes
            UseUnionSubclassForInheritanceMapping();
            // the Id function is used for identity
            // key mapping, it is possible to specify the type 
            // of the property, its access, the name
            // of the field in the table and its server type, 
            // facets and other mapping settings,
            // as well as to specify the class name to be used to 
            // generate the primary key for a new
            // record while saving a new record
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Currency'");
            References(x => x.Name, "NameId");

        }
    }*/
}