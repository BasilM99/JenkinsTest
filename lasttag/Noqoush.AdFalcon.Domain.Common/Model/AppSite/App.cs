﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

namespace Noqoush.AdFalcon.Domain.Common.Model.AppSite
{
    public enum AppSubType
    {
        [EnumText("IPad", "AppSite")]
        iPad =3,
        [EnumText("IPhone", "AppSite")]
        iPhone =2,
        [EnumText("Universal", "AppSite")]
        Unversal =1
    }
   /* public class App : AppSite
    {
        public virtual int SubType { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Url)]
        public virtual string MarketURL
        {
            get;
            set;
        }

        public virtual bool IsUseLocation
        {
            get;
            set;
        }

        public override string GetURL()
        {
            return MarketURL;
        }
        public  virtual string GetSubTypeDescrription(string SubType)
        {
            if (!string.IsNullOrEmpty(SubType))
            {
                Enum enumTobe = (Enum)Enum.Parse(typeof(AppSubType), SubType.ToString());
                if (Convert.ToInt32(SubType) > 0)
                {
                    return enumTobe.ToText();
                }
                return string.Empty;


            }
            else
            {
                return string.Empty;

            }
        }
        public virtual bool ValidateMarketplace()
        {
            throw new System.NotImplementedException();
        }

    }*/
}
