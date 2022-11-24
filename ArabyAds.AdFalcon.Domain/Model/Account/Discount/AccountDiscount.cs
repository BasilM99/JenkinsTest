﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.DomainServices;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Common;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Model.Account.Discount
{
    public class AccountDiscount : IEntity<int>
    {
        public virtual DiscountType Discount_Type { get; set; }
        public virtual string Discount_ValueDescriper { get; set; }
        private const string _format = "{0}:{1}";
        public virtual Account Account { get; set; }
        public virtual Core.Discount Discount { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string GetDescription()
        {
            return string.Format(_format, Discount.Type.ToText() ,(Discount.GetValueDescription()));
        }
        public virtual void DeActive()
        {
            this.Discount.DeActive();
        }
    }
}
