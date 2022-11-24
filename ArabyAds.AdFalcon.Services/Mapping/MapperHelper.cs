using AutoMapper;
using org.apache.zookeeper.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Mapping
{
    public class MapperHelper
    {
        public static T Map<T>(object source)
        {
            return MappingRegister.Mapper.Map<T>(source);
        }

        public static DestT Map<SourceT,DestT>(SourceT source, DestT dest)
        {
            return MappingRegister.Mapper.Map(source, dest);
        }

    }
}
