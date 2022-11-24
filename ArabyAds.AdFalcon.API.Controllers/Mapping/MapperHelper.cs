using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.API.Controllers.Mapping
{
    public class MapperHelper
    {
        public static T Map<T>(object source)
        {
            return (T)AutoMapper.Mapper.DynamicMap(source, source.GetType(), typeof(T));
        }
        
    }
}
