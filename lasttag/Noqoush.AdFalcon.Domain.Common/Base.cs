using System;

//namespace Noqoush.AdFalcon.Domain.Common
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
