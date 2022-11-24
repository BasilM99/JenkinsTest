using System;

//namespace ArabyAds.AdFalcon.Domain.Common
//{
    public class EnumTextAttribute : Attribute
    {
        public string ResourceKey;
        public string ResourceSet;
        public EnumTextAttribute(string resourceKey, string resourceSet)
        {
            ResourceKey = resourceKey;
            ResourceSet = resourceSet;
        }
    }

  
//}
