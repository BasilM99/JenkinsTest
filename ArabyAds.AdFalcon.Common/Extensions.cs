
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace ArabyAds.AdFalcon.Common
//{
    public static class EnumExtender
    {
        public static string ToText(this Enum enumeration)
        {
            var memberInfo = enumeration.GetType().GetMember(enumeration.ToString());
            if (memberInfo.Length <= 0) return enumeration.ToString();

            var attributes = memberInfo[0].GetCustomAttributes(typeof(EnumTextAttribute), false);
            if (attributes.Length > 0)
            {

                return ArabyAds.Framework.Resources.ResourceManager.Instance.GetResource(((EnumTextAttribute)attributes[0]).ResourceKey, ((EnumTextAttribute)attributes[0]).ResourceSet);

            }
            else
            {
                return enumeration.ToString();


            }




        }
    }
//}
