﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    public class TextAdTheme : LookupBase<TextAdTheme, int>
    {
        public virtual string TextColor
        {
            get;
            set;
        }

        public virtual string BackgroundColor
        {
            get;
            set;
        }

        public virtual bool IsCustom
        {
            get;
            set;
        }
        public override string GetDescription()
        {
            if (this.Name != null)
                return Name.ToString();
            else if(IsCustom)
                {
                return ArabyAds.Framework.Resources.ResourceManager.Instance.GetResource("CustomTextAdTheme");
            }

            return string.Empty;

        }

    }
}

