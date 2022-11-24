using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Account.Discount
{
    public enum DiscountType
    {
        Fixed=1,
        Percentage=2
    }
    public class Discount
    {
        public virtual decimal Value { get; set; }
        public virtual DiscountType Type { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
    }
}
